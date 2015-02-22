using System;
using System.Collections.Generic;

namespace CocosSharp
{
    internal enum QUEUE_GROUP
    {
        GLOBALZ_NEG = 0,
        GLOBALZ_ZERO = 1,
        GLOBALZ_POS = 2,
        QUEUE_COUNT = 3,
    };

    /// <summary>
    /// Implementation of a render queue as explained in the following:
    /// 
    /// Implementing a Render Queue for Games -> http://ploobs.com.br/?p=2378
    /// Order your graphics draw calls around -> http://realtimecollisiondetection.net/blog/?p=86
    /// Cocos2D-X Render Pipeline Documentation -> http://www.cocos2d-x.org/wiki/Cocos2d_v30_renderer_pipeline_roadmap
    /// 
    /// </summary>
    public class CCRenderer
    {
        CCDrawManager DrawManager { get; set; }

        CCRawList<CCV3F_C4B_T2F_Quad> quads;

        RenderQueue<CCRenderQueueId, CCRenderCommand> RenderQueue { get; set; }

        int LastMaterialID { get; set; }
        int FilledVertex { get; set; }
        int FilledIndex { get; set; }
        int NumberQuads { get; set; }
        bool IsRendering { get; set; }
        bool isDepthTestFor2D { get; set; }

        int Group { get; set; }

        List<CCRenderCommand> batchCommands;
        List<CCQuadCommand> batchQuadCommands;
        int numberQuads = 0;

        static int BATCH_QUADCOMMAND_RESEVER_SIZE = 64;
        internal const int MATERIAL_ID_DO_NOT_BATCH = 0;

        internal CCRenderer(CCDrawManager drawManager)
        {
            DrawManager = drawManager;

            RenderQueue = new RenderQueue<CCRenderQueueId, CCRenderCommand>();

            quads = new CCRawList<CCV3F_C4B_T2F_Quad>();

            batchCommands = new List<CCRenderCommand>(BATCH_QUADCOMMAND_RESEVER_SIZE);
            batchQuadCommands = new List<CCQuadCommand>(BATCH_QUADCOMMAND_RESEVER_SIZE);
        }

        public void AddComand (CCRenderCommand command)
        {
            var renderQueueId = new CCRenderQueueId(Group, 0, (int)command.Depth, command.MaterialId);
            RenderQueue.Enqueue(renderQueueId, command);
        }

        internal void VisitRenderQueue()
        {

            IsRendering = true;
            quads.Clear();

            while (RenderQueue.HasItems)
            {
                var command = RenderQueue.Dequeue();
                ExecuteRenderCommand(command);
            }

            Flush();

            IsRendering = false;
        }

        void DrawBatchedQuads()
        {

            //TODO: we can improve the draw performance by insert material switching command before hand.

            int indexToDraw = 0;
            int startIndex = 0;

            //Upload buffer to graphics device
            if(numberQuads <= 0 || batchQuadCommands.Count == 0)
            {
                return;
            }

            //Start drawing verties in batch
            var drawManager = DrawManager;

            var quadElements = quads.Elements;

            foreach (CCQuadCommand cmd in batchQuadCommands)
            {
                var newMaterialID = cmd.MaterialId;
                if (LastMaterialID != newMaterialID || newMaterialID == MATERIAL_ID_DO_NOT_BATCH)
                {
                    if (indexToDraw > 0)
                    {

                        drawManager.DrawQuads(quads, startIndex, indexToDraw);
                        //drawnBatches++;
                        //drawnVertices += indexToDraw;

                        startIndex += indexToDraw;
                        indexToDraw = 0;

                    }

                    LastMaterialID = cmd.MaterialId;
                }

                cmd.UseMaterial(drawManager);
                indexToDraw += cmd.QuadCount;
            }

            // Draw any remaining quad
            if (indexToDraw > 0)
            {
                drawManager.DrawQuads(quads, startIndex, indexToDraw);
            }



            batchQuadCommands.Clear();
            numberQuads = 0;

        }

        void FlushQuads()
        {

            if (quads.Count > 0)
            {
                DrawBatchedQuads();
                LastMaterialID = 0;
            }

        }

        void ExecuteRenderCommand (CCRenderCommand command)
        {
            if (command is CCQuadCommand)
            {
                Flush3D();
                FlushTriangles();

                //Console.WriteLine(((CCQuadCommand)command).QuadCount);
                var quadCommand = (CCQuadCommand)command;

                var drawManager = DrawManager;

                //Draw batched quads if necessary
                // TODO: Check for buffer size
                if(command.IsSkipBatching) // || (numberQuads + cmd->getQuadCount()) * 4 > VBO_SIZE )
                {

                    // TODO: Check for buffer size
                    //CCASSERT(cmd->getQuadCount()>= 0 && cmd->getQuadCount() * 4 < VBO_SIZE, "VBO for vertex is not big enough, please break the data down or use customized render command");

                    //Draw batched quads if VBO is full
                    DrawBatchedQuads();
                }

                var mv = command.ModelViewTransform;

                batchQuadCommands.Add(quadCommand);

                foreach (var quad in quadCommand.Quads)
                {
                    quads.Add(mv.Transform(quad));
                }
                numberQuads += quadCommand.QuadCount;

                if (quadCommand.IsSkipBatching)
                {
                    DrawBatchedQuads();
                }
            }
            else
            {
                Flush();
                command.Execute(DrawManager);
            }

        }

        void Flush()
        {
            Flush2D();
            Flush3D();
        }

        void Flush2D()
        {
            FlushQuads();
            FlushTriangles();
        }

        void Flush3D()
        {
        }

        void FlushTriangles()
        {

        }

    }
}

