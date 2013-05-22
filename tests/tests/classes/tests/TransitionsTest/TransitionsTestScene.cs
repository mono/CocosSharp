using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public enum kCCiOSVersion
    {
        kCCiOSVersion_3_0 = 0x03000000,
        kCCiOSVersion_3_1 = 0x03010000,
        kCCiOSVersion_3_1_1 = 0x03010100,
        kCCiOSVersion_3_1_2 = 0x03010200,
        kCCiOSVersion_3_1_3 = 0x03010300,
        kCCiOSVersion_3_2 = 0x03020000,
        kCCiOSVersion_3_2_1 = 0x03020100,
        kCCiOSVersion_4_0 = 0x04000000,
        kCCiOSVersion_4_0_1 = 0x04000100,
        kCCiOSVersion_4_1 = 0x04010000,
        kCCiOSVersion_4_2 = 0x04020000,
        kCCiOSVersion_4_3 = 0x04030000,
        kCCiOSVersion_4_3_1 = 0x04030100,
        kCCiOSVersion_4_3_2 = 0x04030200,
        kCCiOSVersion_4_3_3 = 0x04030300,
    }

    public class TransitionsTestScene : TestScene
    {
        public static int s_nSceneIdx = 0;
        public static int MAX_LAYER = 41;
        public static float TRANSITION_DURATION = 1.2f;
        public static string s_back1 = "Images/background1";
        public static string s_back2 = "Images/background2";

        public static string s_pPathB1 = "Images/b1";
        public static string s_pPathB2 = "Images/b2";
        public static string s_pPathR1 = "Images/r1";
        public static string s_pPathR2 = "Images/r2";
        public static string s_pPathF1 = "Images/f1";
        public static string s_pPathF2 = "Images/f2";

        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }

        public static string[] transitions = new string[]
            {
                "CCTransitionJumpZoom",

                "CCTransitionProgressRadialCCW",
                "CCTransitionProgressRadialCW",
                "CCTransitionProgressHorizontal",
                "CCTransitionProgressVertical",
                "CCTransitionProgressInOut",
                "CCTransitionProgressOutIn",

                "CCTransitionCrossFade",
                "TransitionPageForward",
                "TransitionPageBackward",
                "CCTransitionFadeTR",
                "CCTransitionFadeBL",
                "CCTransitionFadeUp",
                "CCTransitionFadeDown",
                "CCTransitionTurnOffTiles",
                "CCTransitionSplitRows",
                "CCTransitionSplitCols",

                "CCTransitionFade",
                "FadeWhiteTransition",

                "FlipXLeftOver",
                "FlipXRightOver",
                "FlipYUpOver",
                "FlipYDownOver",
                "FlipAngularLeftOver",
                "FlipAngularRightOver",

                "ZoomFlipXLeftOver",
                "ZoomFlipXRightOver",
                "ZoomFlipYUpOver",
                "ZoomFlipYDownOver",
                "ZoomFlipAngularLeftOver",
                "ZoomFlipAngularRightOver",

                "CCTransitionShrinkGrow",
                "CCTransitionRotoZoom",

                "CCTransitionMoveInL",
                "CCTransitionMoveInR",
                "CCTransitionMoveInT",
                "CCTransitionMoveInB",
                "CCTransitionSlideInL",
                "CCTransitionSlideInR",
                "CCTransitionSlideInT",
                "CCTransitionSlideInB",
            };

        public override void runThisTest()
        {
            CCLayer pLayer = new TestLayer1();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }

        public static CCTransitionScene createTransition(int nIndex, float t, CCScene s)
        {
            // fix bug #486, without setDepthTest(false), FlipX,Y will flickers
            CCDirector.SharedDirector.SetDepthTest(false);

            switch (nIndex)
            {
                case 0: return new CCTransitionJumpZoom(t, s);

                case 1: return new CCTransitionProgressRadialCCW(t, s);
                case 2: return new CCTransitionProgressRadialCW(t, s);
                case 3: return new CCTransitionProgressHorizontal(t, s);
                case 4: return new CCTransitionProgressVertical(t, s);
                case 5: return new CCTransitionProgressInOut(t, s);
                case 6: return new CCTransitionProgressOutIn(t, s);

                case 7: return new CCTransitionCrossFade(t, s);

                case 8: return new PageTransitionForward(t, s);
                case 9: return new PageTransitionBackward(t, s);
                case 10: return new CCTransitionFadeTR(t, s);
                case 11: return new CCTransitionFadeBL(t, s);
                case 12: return new CCTransitionFadeUp(t, s);
                case 13: return new CCTransitionFadeDown(t, s);

                case 14: return new CCTransitionTurnOffTiles(t, s);

                case 15: return new CCTransitionSplitRows(t, s);
                case 16: return new CCTransitionSplitCols(t, s);

                case 17: return new CCTransitionFade(t, s);
                case 18: return new FadeWhiteTransition(t, s);

                case 19: return new FlipXLeftOver(t, s);
                case 20: return new FlipXRightOver(t, s);
                case 21: return new FlipYUpOver(t, s);
                case 22: return new FlipYDownOver(t, s);
                case 23: return new FlipAngularLeftOver(t, s);
                case 24: return new FlipAngularRightOver(t, s);

                case 25: return new ZoomFlipXLeftOver(t, s);
                case 26: return new ZoomFlipXRightOver(t, s);
                case 27: return new ZoomFlipYUpOver(t, s);
                case 28: return new ZoomFlipYDownOver(t, s);
                case 29: return new ZoomFlipAngularLeftOver(t, s);
                case 30: return new ZoomFlipAngularRightOver(t, s);

                case 31: return new CCTransitionShrinkGrow(t, s);
                case 32: return new CCTransitionRotoZoom(t, s);

                case 33: return new CCTransitionMoveInL(t, s);
                case 34: return new CCTransitionMoveInR(t, s);
                case 35: return new CCTransitionMoveInT(t, s);
                case 36: return new CCTransitionMoveInB(t, s);

                case 37: return new CCTransitionSlideInL(t, s);
                case 38: return new CCTransitionSlideInR(t, s);
                case 39: return new CCTransitionSlideInT(t, s);
                case 40: return new CCTransitionSlideInB(t, s);

                default: break;
            }

            return null;
        }
    }
}
