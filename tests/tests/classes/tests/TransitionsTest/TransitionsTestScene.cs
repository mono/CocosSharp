using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

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
                case 0: return CCTransitionJumpZoom.Create(t, s);

                case 1: return CCTransitionProgressRadialCCW.Create(t, s);
                case 2: return CCTransitionProgressRadialCW.Create(t, s);
                case 3: return CCTransitionProgressHorizontal.Create(t, s);
                case 4: return CCTransitionProgressVertical.Create(t, s);
                case 5: return CCTransitionProgressInOut.Create(t, s);
                case 6: return CCTransitionProgressOutIn.Create(t, s);

                case 7: return CCTransitionCrossFade.Create(t, s);

                case 8: return PageTransitionForward.Create(t, s);
                case 9: return PageTransitionBackward.Create(t, s);
                case 10: return CCTransitionFadeTR.Create(t, s);
                case 11: return CCTransitionFadeBL.Create(t, s);
                case 12: return CCTransitionFadeUp.Create(t, s);
                case 13: return CCTransitionFadeDown.Create(t, s);

                case 14: return CCTransitionTurnOffTiles.Create(t, s);

                case 15: return CCTransitionSplitRows.Create(t, s);
                case 16: return CCTransitionSplitCols.Create(t, s);

                case 17: return CCTransitionFade.Create(t, s);
                case 18: return FadeWhiteTransition.Create(t, s);

                case 19: return FlipXLeftOver.Create(t, s);
                case 20: return FlipXRightOver.Create(t, s);
                case 21: return FlipYUpOver.Create(t, s);
                case 22: return FlipYDownOver.Create(t, s);
                case 23: return FlipAngularLeftOver.Create(t, s);
                case 24: return FlipAngularRightOver.Create(t, s);

                case 25: return ZoomFlipXLeftOver.Create(t, s);
                case 26: return ZoomFlipXRightOver.Create(t, s);
                case 27: return ZoomFlipYUpOver.Create(t, s);
                case 28: return ZoomFlipYDownOver.Create(t, s);
                case 29: return ZoomFlipAngularLeftOver.Create(t, s);
                case 30: return ZoomFlipAngularRightOver.Create(t, s);

                case 31: return CCTransitionShrinkGrow.Create(t, s);
                case 32: return CCTransitionRotoZoom.Create(t, s);

                case 33: return CCTransitionMoveInL.Create(t, s);
                case 34: return CCTransitionMoveInR.Create(t, s);
                case 35: return CCTransitionMoveInT.Create(t, s);
                case 36: return CCTransitionMoveInB.Create(t, s);

                case 37: return CCTransitionSlideInL.Create(t, s);
                case 38: return CCTransitionSlideInR.Create(t, s);
                case 39: return CCTransitionSlideInT.Create(t, s);
                case 40: return CCTransitionSlideInB.Create(t, s);

                default: break;
            }

            return null;
        }
    }
}
