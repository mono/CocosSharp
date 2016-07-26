using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

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
        public static int MAX_LAYER = 0;

        public EffectAdvanceScene() : base()
        {
            MAX_LAYER = createEffectAdvanceLayerFunctions.Length;
        }

        static Func<CCLayer>[] createEffectAdvanceLayerFunctions =
            { 
                () => new Effect6(),
                () => new Effect3(),
                () => new Effect2(),
                () => new Effect1(),
                () => new Effect4(),
                () => new Effect5(),
                () => new Issue631(),
            };
        

        public static CCLayer createEffectAdvanceLayer(int index)
        {
            return createEffectAdvanceLayerFunctions[index]();
        }

        protected override void NextTestCase()
        {
            nextEffectAdvanceAction();
        }
        protected override void PreviousTestCase()
        {
            backEffectAdvanceAction();
        }
        protected override void RestTestCase()
        {
            restartEffectAdvanceAction();
        }

        public static CCLayer nextEffectAdvanceAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

			var pLayer = createEffectAdvanceLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer backEffectAdvanceAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

			var pLayer = createEffectAdvanceLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer restartEffectAdvanceAction()
        {
			var pLayer = createEffectAdvanceLayer(sceneIdx);

            return pLayer;
        }

        public override void runThisTest()
        {
			var pLayer = nextEffectAdvanceAction();

            AddChild(pLayer);
            Director.ReplaceScene(this);
        }
    }
}
