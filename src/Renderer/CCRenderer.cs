using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    internal class RenderQueuePriority : IComparer<long>
    {

        public int Compare(long first, long other)
        {
            // 64 - 57 : Group id (byte)
            // 56 - 25 : Global depth (float)
            // 24 - 1 : Material id (24 bit)

            var group1 = first & ((long)(byte.MaxValue) << 56);
            var group2 = other & ((long)(byte.MaxValue) << 56);

            int compareValue = group1.CompareTo(group2);

            if(compareValue == 0)
            {
                var depth1 = first >> 24;
                var depth2 = other >> 24;

                compareValue = depth1.CompareTo(depth2);
            }

            return compareValue;
        }
    }

    // Implementation based off of discussion "Implementing a Render Queue for Games" http://ploobs.com.br/?p=2378 
    internal class CCRenderer
    {      
        [Flags]
        internal enum CCCommandType
        {
            None = 0x0,
            Quad = 0x1,
            Custom = 0x2,
            Primitive = 0x4,
        }

        byte currentGroupId;
        CCCommandType currentCommandType;
        CCRawList<CCV3F_C4B_T2F_Quad> currentBatchedQuads;
        List<CCQuadCommand> quadCommands;
        CCRenderQueue<long, CCRenderCommand> renderQueue;
        CCDrawManager drawManager;


        #region Constructors

        internal CCRenderer(CCDrawManager drawManagerIn)
        {
            currentBatchedQuads = new CCRawList<CCV3F_C4B_T2F_Quad>();
            quadCommands = new List<CCQuadCommand>();
            renderQueue = new CCRenderQueue<long, CCRenderCommand>(new RenderQueuePriority());
            drawManager = drawManagerIn;
        }

        #endregion Constructors

        public void AddCommand(CCRenderCommand command)
        {
            command.Group = currentGroupId;
            renderQueue.Enqueue(command.RenderId, command);
        }

        public void PushGroup()
        {
            currentGroupId += 1;
        }

        public void PopGroup()
        {
            currentGroupId -= 1;
        }

        internal void VisitRenderQueue()
        {
            currentCommandType = CCCommandType.None;
            currentGroupId = 0;

            while (renderQueue.HasItems)
            {
                var command = renderQueue.Dequeue();
                command.RequestRenderCommand(this);
            }

            // Flush any remaining render commands
            Flush();
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

