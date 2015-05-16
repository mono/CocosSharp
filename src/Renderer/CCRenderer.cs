using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    // Implementation loosely based off of discussion "Implementing a Render Queue for Games" http://ploobs.com.br/?p=2378 
    public class CCRenderer
    {      
        [Flags]
        internal enum CCCommandType
        {
            None = 0x0,
            Quad = 0x1,
            Custom = 0x2,
            Primitive = 0x4,
        }

        int currentLayerGroupIdIndex, currentGroupIdIndex;
        uint currentArrivalIndex;
        byte currentLayerGroupId, maxLayerGroupId;
        byte currentGroupId, maxGroupId;
        CCCommandType currentCommandType;
        CCRawList<CCV3F_C4B_T2F_Quad> currentBatchedQuads;
        CCRawList<CCQuadCommand> quadCommands;
        CCRawList<CCRenderCommand> renderQueue;
        CCDrawManager drawManager;

        const uint MaxLayerDepth = 20;
        readonly Matrix[] layerGroupViewMatrixStack;
        readonly Matrix[] layerGroupProjMatrixStack;
        readonly byte[] layerGroupIdStack, groupIdStack;


        #region Constructors

        internal CCRenderer(CCDrawManager drawManagerIn)
        {
            currentBatchedQuads = new CCRawList<CCV3F_C4B_T2F_Quad>(256, true);
            quadCommands = new CCRawList<CCQuadCommand>(256, true);
            renderQueue = new CCRawList<CCRenderCommand>();
            drawManager = drawManagerIn;

            layerGroupViewMatrixStack = new Matrix[MaxLayerDepth];
            layerGroupProjMatrixStack = new Matrix[MaxLayerDepth];
            layerGroupIdStack = new byte[MaxLayerDepth];
            groupIdStack = new byte[MaxLayerDepth];
        }

        #endregion Constructors

        public void AddCommand(CCRenderCommand command)
        {
            // Render command might be used multiple times per draw loop
            // e.g. within render texture
            if(renderQueue.Contains(command))
                command = command.Copy();

            command.Group = currentGroupId;
            command.LayerGroup = currentLayerGroupId;
            command.ArrivalIndex = ++currentArrivalIndex;

            renderQueue.Push(command);
        }

        public void PushLayerGroup(ref Matrix viewMatrix, ref Matrix projMatrix)
        {
            if(currentLayerGroupId == MaxLayerDepth - 1)
            {
                Debug.Fail(String.Format("Maximum layer depth of {0} reached", MaxLayerDepth));
                return;
            }

            currentLayerGroupId = ++maxLayerGroupId;
            layerGroupIdStack[++currentLayerGroupIdIndex] = currentLayerGroupId;
            layerGroupViewMatrixStack[currentLayerGroupId] = viewMatrix;
            layerGroupProjMatrixStack[currentLayerGroupId] = projMatrix;
        }

        public void PopLayerGroup()
        {
            currentLayerGroupId = layerGroupIdStack[--currentLayerGroupIdIndex];
        }

        public void PushGroup()
        {
            currentGroupId = ++maxGroupId;
            groupIdStack[++currentGroupIdIndex] = currentGroupId;
        }

        public void PopGroup()
        {
            currentGroupId = groupIdStack[--currentGroupIdIndex];
        }

        internal void VisitRenderQueue()
        {
            currentCommandType = CCCommandType.None;
            currentLayerGroupId = 0;
            currentLayerGroupIdIndex = 0;
            currentGroupId = 0;
            currentGroupIdIndex = 0;
            currentArrivalIndex = 0;
            maxLayerGroupId = 0;
            maxGroupId = 0;

            Array.Sort<CCRenderCommand>(renderQueue.Elements, 0, renderQueue.Count);

            drawManager.ViewMatrix = Matrix.Identity;
            drawManager.ProjectionMatrix = Matrix.Identity;

            foreach(CCRenderCommand command in renderQueue)
            {
                byte layerGroupId = command.LayerGroup;

                if(layerGroupId != currentLayerGroupId) 
                {
                    currentLayerGroupId = layerGroupId;
                    drawManager.ViewMatrix = layerGroupViewMatrixStack[currentLayerGroupId];
                    drawManager.ProjectionMatrix = layerGroupProjMatrixStack[currentLayerGroupId];
                }

                command.RequestRenderCommand(this);
            }

            // This only resets the count of the queue so is inexpensive
            renderQueue.Clear();

            // Flush any remaining render commands
            Flush();

            currentLayerGroupId = 0;
        }

        #region Processing render commands

        internal void ProcessQuadRenderCommand(CCQuadCommand quadCommand)
        {
            var worldTransform = quadCommand.WorldTransform;
            var identity = worldTransform == CCAffineTransform.Identity;

            quadCommands.Add(quadCommand);

            var quads = quadCommand.Quads;
            for (int i = 0, N = quadCommand.QuadCount; i < N; ++i)
            {
                if (identity)
                    currentBatchedQuads.Add(quads[i]);
                else
                    currentBatchedQuads.Add(worldTransform.Transform(quads[i]));
            }

            // We're changing command types so render any pending sequence of commandss
            // e.g. Batched quad commands
            if((currentCommandType & CCCommandType.Quad) == CCCommandType.None)
                Flush();

            currentCommandType = CCCommandType.Quad;
        }

        internal void ProcessPrimitiveRenderCommand <T, T2> (CCPrimitiveCommand <T, T2> primitiveCommand)
            where T : struct, IVertexType
            where T2 : struct
           
        {
            var worldTransform = primitiveCommand.WorldTransform;

            // We're changing command types so render any pending sequence of commands
            // e.g. Batched quad commands
            if((currentCommandType & CCCommandType.Primitive) == CCCommandType.None)
                Flush();

            primitiveCommand.RenderPrimitive(drawManager);

            currentCommandType = CCCommandType.Primitive;

        }

        internal void ProcessCustomRenderCommand(CCCustomCommand customCommand)
        {
            // We're changing command types so render any pending sequence of commands
            // e.g. Batched quad commands
            if((currentCommandType & CCCommandType.Custom) == CCCommandType.None)
                Flush();

            customCommand.RenderCustomCommand(drawManager);

            currentCommandType = CCCommandType.Custom;
        }

        #endregion Processing render commands


        void Flush()
        {
            switch(currentCommandType)
            {
                case CCCommandType.Quad:
                    DrawBatchedQuads();
                    break;
                default:
                    break;
            }
        }

        void DrawBatchedQuads()
        {
            int numOfQuads = 0;
            int startIndex = 0;

            if(currentBatchedQuads.Count <= 0 || quadCommands.Count == 0)
                return;

            var quadElements = currentBatchedQuads.Elements;
            uint lastMaterialId = 0;

            drawManager.PushMatrix();
            drawManager.SetIdentityMatrix();

            foreach (CCQuadCommand command in quadCommands)
            {
                var newMaterialID = command.MaterialId;
                if (lastMaterialId != newMaterialID)
                {
                    if (numOfQuads > 0)
                    {
                        drawManager.DrawQuads(currentBatchedQuads, startIndex, numOfQuads);

                        startIndex += numOfQuads;
                        numOfQuads = 0;
                    }

                    lastMaterialId = command.MaterialId;
                }

                command.UseMaterial(drawManager);
                numOfQuads += command.QuadCount;
            }

            // Draw any remaining quads
            if (numOfQuads > 0)
                drawManager.DrawQuads(currentBatchedQuads, startIndex, numOfQuads);

            quadCommands.Clear();
            currentBatchedQuads.Clear();

            drawManager.PopMatrix();
        }
    }
}

