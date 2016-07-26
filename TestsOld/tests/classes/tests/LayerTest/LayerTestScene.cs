using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LayerTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = nextTestAction();
            AddChild(pLayer);

            Director.ReplaceScene(this);
        }

        static int sceneIdx = -1;
        static int MAX_LAYER = 8;

        static Func<CCLayer>[] layerTestCreateFunctions =
{
                () => new LayerTest1(),
                () => new LayerTest2(),
                () => new LayerTestBlend(),
                () => new LayerGradient(),
                () => new LayerScaleTest(),
                () => new LayerClipScissor(),
                () => new LayerClippingTexture(),
                () => new LayerMultiplexTest(),

        };

        public static CCLayer createTestLayer(int index)
        {
            return layerTestCreateFunctions[index]();
        }
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
