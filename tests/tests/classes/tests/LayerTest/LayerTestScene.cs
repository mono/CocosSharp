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
            CCDirector.SharedDirector.ReplaceScene(this);
        }

        static int sceneIdx = -1;
        static int MAX_LAYER = 14;

        public static CCLayer createTestLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new LayerTestCascadingOpacityA();
                case 1: return new LayerTestCascadingOpacityB();
                case 2: return new LayerTestCascadingOpacityC();
                case 3: return new LayerTestCascadingColorA();
                case 4: return new LayerTestCascadingColorB();
                case 5: return new LayerTestCascadingColorC();
                case 6: return new LayerTest1();
                case 7: return new LayerTest2();
                case 8: return new LayerTestBlend();
                case 9: return new LayerGradient();
                case 10: return new LayerScaleTest();
                case 11: return new LayerClipScissor();
                case 12: return new LayerClippingTexture();
                case 13: return new LayerMultiplexTest(); // In layertest.cs
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
