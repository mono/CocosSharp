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

        public static CCLayer createTestLayer(int nIndex)
        {
            switch (nIndex)
            {
                // These are not applicable anymore
//                case 0: return new LayerTestCascadingOpacityA();
//                case 1: return new LayerTestCascadingOpacityB();
//                case 2: return new LayerTestCascadingOpacityC();
//                case 3: return new LayerTestCascadingColorA();
//                case 4: return new LayerTestCascadingColorB();
//                case 5: return new LayerTestCascadingColorC();
                case 0: return new LayerTest1();
                case 1: return new LayerTest2();
                case 2: return new LayerTestBlend();
                case 3: return new LayerGradient();
                case 4: return new LayerScaleTest();
                case 5: return new LayerClipScissor();
                case 6: return new LayerClippingTexture();
                case 7: return new LayerMultiplexTest(); // In layertest.cs
            }
            return null;
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
