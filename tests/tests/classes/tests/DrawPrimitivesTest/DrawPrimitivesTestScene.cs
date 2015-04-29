using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class DrawPrimitivesTestScene : TestScene
    {
        private static int sceneIdx = -1;
        private static int MAX_LAYER = 9;

        public DrawPrimitivesTestScene () : base()
        {
            MAX_LAYER = createPrimitiveLayerFunctions.Length;
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextTestAction();
            AddChild(pLayer);
            Scene.Director.ReplaceScene(this);
        }

        public static CCLayer createTestLayer(int index)
        {
            return createPrimitiveLayerFunctions[index]();
        }

        static Func<CCLayer>[] createPrimitiveLayerFunctions =
            { 
                () => new DrawPrimitivesTest(),
                () => new DrawNodeTest(),
                () => new DrawNodeTest1(),
                () => new DrawPrimitivesWithRenderTextureTest(),
                () => new DrawPrimitivesWithRenderTextureTest1(),
                () => new DrawPrimitivesWithRenderTextureTest2(),
                () => new DrawPrimitivesWithRenderTextureTest3(),
                () => new GeometryBatchTest1 (),
                () => new GeometryBatchTest2(),
            };
        
        protected override void NextTestCase()
        {
            nextTestAction();
        }

        protected override void PreviousTestCase()
        {
            backTestAction();
        }

        protected override void RestTestCase()
        {
            restartTestAction();
        }

        public static CCLayer nextTestAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;
            CCLayer pLayer = createTestLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer backTestAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;
            CCLayer pLayer = createTestLayer(sceneIdx);
            return pLayer;
        }

        public static CCLayer restartTestAction()
        {
            CCLayer pLayer = createTestLayer(sceneIdx);
            return pLayer;
        }
    }
}
