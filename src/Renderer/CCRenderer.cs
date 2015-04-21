using System;
using System.Collections.Generic;

namespace CocosSharp
{
    // Implementation based off of discussion "Implementing a Render Queue for Games" http://ploobs.com.br/?p=2378 
    internal class CCRenderer
    {      
        [Flags]
        internal enum CCCommandType
        {
            None = 0x0,
            Quad = 0x1,
            Custom = 0x2
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
            renderQueue = new CCRenderQueue<long, CCRenderCommand>();
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
                        drawManager.DrawQuads(currentBatchedQuads, startIndex, numOfQuads);

                        startIndex += numOfQuads;
                        numOfQuads = 0;
                    }

                    lastMaterialId = command.MaterialId;
                }

                command.UseMaterial(drawManager);
                numOfQuads += command.Quads.Length;
            }

            // Draw any remaining quads
            if (numOfQuads > 0)
                drawManager.DrawQuads(currentBatchedQuads, startIndex, numOfQuads);

            quadCommands.Clear();
            currentBatchedQuads.Clear();
        }
    }
}

