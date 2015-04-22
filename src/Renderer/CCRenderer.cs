using System;
using System.Collections.Generic;

namespace CocosSharp
{

    internal class RenderQueuePriority : IComparer<long>
    {

        public int Compare(long first, long other)
        {
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
        internal CCDrawManager DrawManager { get; set; }


        #region Constructors

        internal CCRenderer(CCDrawManager drawManagerIn)
        {
            currentBatchedQuads = new CCRawList<CCV3F_C4B_T2F_Quad>();
            quadCommands = new List<CCQuadCommand>();
            renderQueue = new CCRenderQueue<long, CCRenderCommand>(new RenderQueuePriority());
            DrawManager = drawManagerIn;
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

            foreach(var quad in quadCommand.Quads)
                currentBatchedQuads.Add(worldTransform.Transform(quad));

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

            foreach (CCQuadCommand command in quadCommands)
            {
                var newMaterialID = command.MaterialId;
                if (lastMaterialId != newMaterialID)
                {
                    if (numOfQuads > 0)
                    {
                        DrawManager.DrawQuads(currentBatchedQuads, startIndex, numOfQuads);

                        startIndex += numOfQuads;
                        numOfQuads = 0;
                    }

                    lastMaterialId = command.MaterialId;
                }

                command.UseMaterial(DrawManager);
                numOfQuads += command.Quads.Length;
            }

            // Draw any remaining quads
            if (numOfQuads > 0)
                DrawManager.DrawQuads(currentBatchedQuads, startIndex, numOfQuads);

            quadCommands.Clear();
            currentBatchedQuads.Clear();
        }
    }
}

