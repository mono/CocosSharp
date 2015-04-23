using System;
using System.Collections.Generic;

namespace CocosSharp
{
    internal class RenderQueuePriority : IComparer<long>
    {

        public int Compare(long first, long other)
        {
            // 2D - We could probably always return 0 here because our children should already be sorted.
            // but just to make sure if something changes later
            // When we implement 3D we will need to take other factores into account.
            var depth1 = first >> 24;
            var depth2 = other >> 24;

            if (depth1 < depth2)
                return 1;
            else if (depth1 > depth2)
                return -1;
            else return 0;
               
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
            Batch  = 0x4,
        }

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
            renderQueue.Enqueue(command.RenderId, command);
        }

        internal void VisitRenderQueue()
        {
            currentCommandType = CCCommandType.None;

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

            quadCommands.Add(quadCommand);

            var quads = quadCommand.Quads;
            for(int i = 0, N = quadCommand.QuadCount; i < N; ++i)
                currentBatchedQuads.Add(worldTransform.Transform(quads[i]));

            // We've changed command types so render previous sequence of commands
            if((currentCommandType | CCCommandType.Quad) == CCCommandType.None)
                Flush();

            currentCommandType = CCCommandType.Quad;
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

