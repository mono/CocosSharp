using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class EffectAdvanceScene : TestScene
    {
        public static int kTagTextLayer = 1;

        public static int kTagSprite1 = 1;
        public static int kTagSprite2 = 2;
        public static int kTagBackground = 1;
        public static int kTagLabel = 2;
        public enum ThreeVariable
        {
            IDC_NEXT = 100,
            IDC_BACK,
            IDC_RESTART
        }
        public static int sceneIdx = -1;
        public static int MAX_LAYER = 6;

        public static CCLayer createEffectAdvanceLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new Effect3();
                case 1: return new Effect2();
                case 2: return new Effect1();
                case 3: return new Effect4();
                case 4: return new Effect5();
                case 5: return new Issue631();
            }
            return null;
        }

        public static CCLayer nextEffectAdvanceAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createEffectAdvanceLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backEffectAdvanceAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createEffectAdvanceLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartEffectAdvanceAction()
        {
            CCLayer pLayer = createEffectAdvanceLayer(sceneIdx);

            return pLayer;
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextEffectAdvanceAction();

            AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}
