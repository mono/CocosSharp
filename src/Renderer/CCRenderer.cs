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

        int currentViewportIdIndex, currentLayerGroupIdIndex, currentGroupIdIndex;
        uint currentArrivalIndex;
        byte currentViewportGroupId, maxViewportGroupId;
        byte currentLayerGroupId, maxLayerGroupId;
        byte currentGroupId, maxGroupId;
        CCCommandType currentCommandType;
        CCRawList<CCV3F_C4B_T2F_Quad> currentBatchedQuads;
        CCRawList<CCQuadCommand> quadCommands;
        CCRawList<CCRenderCommand> renderQueue;
        CCDrawManager drawManager;

        const uint MaxStackDepth = 100;
        readonly Viewport[] viewportGroupStack;
        readonly Matrix[] layerGroupViewMatrixStack;
        readonly Matrix[] layerGroupProjMatrixStack;
        readonly byte[] viewportGroupIdStack, layerGroupIdStack, groupIdStack;


        #region Properties

        internal bool UsingDepthTest { get; set; }

        #endregion Properties


        #region Constructors

        internal CCRenderer(CCDrawManager drawManagerIn)
        {
            currentBatchedQuads = new CCRawList<CCV3F_C4B_T2F_Quad>(256, true);
            quadCommands = new CCRawList<CCQuadCommand>(256, true);
            renderQueue = new CCRawList<CCRenderCommand>();
            drawManager = drawManagerIn;

            viewportGroupStack = new Viewport[MaxStackDepth];
            layerGroupViewMatrixStack = new Matrix[MaxStackDepth];
            layerGroupProjMatrixStack = new Matrix[MaxStackDepth];
            viewportGroupIdStack = new byte[MaxStackDepth];
            layerGroupIdStack = new byte[MaxStackDepth];
            groupIdStack = new byte[MaxStackDepth];
        }

        #endregion Constructors

        public void AddCommand(CCRenderCommand command)
        {
            // Render command might be used multiple times per draw loop
            // e.g. within render texture
            if(currentGroupId != 0)
                command = command.Copy();

            command.Group = currentGroupId;
            command.ViewportGroup = currentViewportGroupId;
            command.LayerGroup = currentLayerGroupId;
            command.ArrivalIndex = ++currentArrivalIndex;
            command.UsingDepthTest = UsingDepthTest;

            renderQueue.Push(command);
        }

        internal void PushViewportGroup(ref Viewport viewport)
        {
            currentViewportGroupId = ++maxViewportGroupId;
            viewportGroupIdStack[++currentViewportIdIndex] = currentViewportGroupId;
            viewportGroupStack[currentViewportGroupId] = viewport;
        }

        internal void PopViewportGroup()
        {
            currentViewportGroupId = viewportGroupIdStack[--currentViewportIdIndex];
        }

        internal void PushLayerGroup(ref Matrix viewMatrix, ref Matrix projMatrix)
        {
            if(currentLayerGroupId == MaxStackDepth - 1)
            {
                Debug.Assert(false,String.Format("Maximum layer depth of {0} reached", MaxStackDepth));
                return;
            }

            currentLayerGroupId = ++maxLayerGroupId;
            layerGroupIdStack[++currentLayerGroupIdIndex] = currentLayerGroupId;
            layerGroupViewMatrixStack[currentLayerGroupId] = viewMatrix;
            layerGroupProjMatrixStack[currentLayerGroupId] = projMatrix;
        }

        internal void PopLayerGroup()
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
            currentViewportGroupId = 0;
            currentViewportIdIndex = 0;
            currentLayerGroupId = 0;
            currentLayerGroupIdIndex = 0;
            currentGroupId = 0;
            currentGroupIdIndex = 0;
            currentArrivalIndex = 0;
            maxViewportGroupId = 0;
            maxLayerGroupId = 0;
            maxGroupId = 0;

            Array.Sort<CCRenderCommand>(renderQueue.Elements, 0, renderQueue.Count);

            drawManager.ViewMatrix = Matrix.Identity;
            drawManager.ProjectionMatrix = Matrix.Identity;

            foreach(CCRenderCommand command in renderQueue)
            {
                byte viewportGroupId = command.ViewportGroup;

                if(viewportGroupId != currentViewportGroupId)
                {
                    // We're about to change viewport
                    // So flush any pending render commands which use previous viewport
                    Flush();

                    currentViewportGroupId = viewportGroupId;
                    drawManager.Viewport = viewportGroupStack[currentViewportGroupId];
                }

                byte layerGroupId = command.LayerGroup;

                if(layerGroupId != currentLayerGroupId) 
                {
                    // We're about to change view/proj matrices
                    // So flush any pending render commands which use previous MVP state
                    Flush();

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

            currentViewportGroupId = 0;
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
            bool originalDepthTestState = drawManager.DepthTest;
            bool usingDepthTest = originalDepthTestState;

            drawManager.PushMatrix();
            drawManager.SetIdentityMatrix();

            CCQuadCommand prevCommand = null;

            foreach (CCQuadCommand command in quadCommands)
            {
                var newMaterialID = command.MaterialId;
                bool commandUsesDepthTest = command.UsingDepthTest;

                if (lastMaterialId != newMaterialID || commandUsesDepthTest != usingDepthTest)
                {
                    if (numOfQuads > 0 && prevCommand != null)
                    {
                        prevCommand.UseMaterial(drawManager);
                        drawManager.DrawQuads(currentBatchedQuads, startIndex, numOfQuads);

                        startIndex += numOfQuads;
                        numOfQuads = 0;
                    }

                    lastMaterialId = newMaterialID;
                    usingDepthTest = commandUsesDepthTest;

                    drawManager.DepthTest = usingDepthTest;
                }

                numOfQuads += command.QuadCount;
                prevCommand = command;
            }

            // Draw any remaining quads
            if (numOfQuads > 0 && prevCommand != null)
            {
                prevCommand.UseMaterial(drawManager);
                drawManager.DrawQuads(currentBatchedQuads, startIndex, numOfQuads);
            }

            quadCommands.Clear();
            currentBatchedQuads.Clear();

            drawManager.PopMatrix();

            drawManager.DepthTest = originalDepthTestState;
        }
    }
}

