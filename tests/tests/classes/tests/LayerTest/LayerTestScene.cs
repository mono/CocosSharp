using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

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
        static int MAX_LAYER = 5;

        public static CCLayer createTestLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new LayerTest1();
                case 1: return new LayerTest2();
                case 2: return new LayerTestBlend();
                case 3: return new LayerGradient();
                case 4: return new LayerScaleTest();
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
