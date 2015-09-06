using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

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
        public static int MAX_LAYER = 0;
        public static float TRANSITION_DURATION = 1.2f;
        public static string s_back1 = "Images/background1";
        public static string s_back2 = "Images/background2";

        public static string s_pPathB1 = "Images/b1";
        public static string s_pPathB2 = "Images/b2";
        public static string s_pPathR1 = "Images/r1";
        public static string s_pPathR2 = "Images/r2";
        public static string s_pPathF1 = "Images/f1";
        public static string s_pPathF2 = "Images/f2";

        public TransitionsTestScene()
        {
            MAX_LAYER = transitions.Count;
        }
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }

        public static Dictionary<string, Func<float, CCScene, CCTransitionScene>> transitions = new Dictionary<string, Func<float, CCScene, CCTransitionScene>> 
        {
            {"CCTransitionJumpZoom", (time, scene) => new CCTransitionJumpZoom(time, scene)},
            {"CCTransitionProgressRadialCCW", (time, scene) => new CCTransitionProgressRadialCCW(time, scene)},
            {"CCTransitionProgressRadialCW", (time, scene) => new CCTransitionProgressRadialCW(time, scene)},
            {"CCTransitionProgressHorizontal", (time, scene) => new CCTransitionProgressHorizontal(time, scene)},
            {"CCTransitionProgressVertical", (time, scene) => new CCTransitionProgressVertical(time, scene)},
            {"CCTransitionProgressInOut", (time, scene) => new CCTransitionProgressInOut(time, scene)},
            {"CCTransitionProgressOutIn", (time, scene) => new CCTransitionProgressOutIn(time, scene)},

            {"CCTransitionCrossFade", (time, scene) => new CCTransitionCrossFade(time, scene)},

            {"TransitionPageForward", (time, scene) => new PageTransitionForward(time, scene)},
            {"TransitionPageBackward", (time, scene) => new PageTransitionBackward(time, scene)},
            {"CCTransitionFadeTR", (time, scene) => new CCTransitionFadeTR(time, scene)},
            {"CCTransitionFadeBL", (time, scene) => new CCTransitionFadeBL(time, scene)},
            {"CCTransitionFadeUp", (time, scene) => new CCTransitionFadeUp(time, scene)},
            {"CCTransitionFadeDown", (time, scene) => new CCTransitionFadeDown(time, scene)},

            {"CCTransitionTurnOffTiles", (time, scene) => new CCTransitionTurnOffTiles(time, scene)},

            {"CCTransitionSplitRows", (time, scene) => new CCTransitionSplitRows(time, scene)},
            {"CCTransitionSplitCols", (time, scene) => new CCTransitionSplitCols(time, scene)},

            {"CCTransitionFade", (time, scene) => new CCTransitionFade(time, scene)},
            {"FadeWhiteTransition", (time, scene) => new FadeWhiteTransition(time, scene)},

            {"FlipXLeftOver", (time, scene) => new FlipXLeftOver(time, scene)},
            {"FlipXRightOver", (time, scene) => new FlipXRightOver(time, scene)},
            {"FlipYUpOver", (time, scene) => new FlipYUpOver(time, scene)},
            {"FlipYDownOver", (time, scene) => new FlipYDownOver(time, scene)},
            {"FlipAngularLeftOver", (time, scene) => new FlipAngularLeftOver(time, scene)},
            {"FlipAngularRightOver", (time, scene) => new FlipAngularRightOver(time, scene)},

            {"ZoomFlipXLeftOver", (time, scene) => new ZoomFlipXLeftOver(time, scene)},
            {"ZoomFlipXRightOver", (time, scene) => new ZoomFlipXRightOver(time, scene)},
            {"ZoomFlipYUpOver", (time, scene) => new ZoomFlipYUpOver(time, scene)},
            {"ZoomFlipYDownOver", (time, scene) => new ZoomFlipYDownOver(time, scene)},
            {"ZoomFlipAngularLeftOver", (time, scene) => new ZoomFlipAngularLeftOver(time, scene)},
            {"ZoomFlipAngularRightOver", (time, scene) => new ZoomFlipAngularRightOver(time, scene)},

            {"CCTransitionShrinkGrow", (time, scene) => new CCTransitionShrinkGrow(time, scene)},
            {"CCTransitionRotoZoom", (time, scene) => new CCTransitionRotoZoom(time, scene)},

            {"CCTransitionMoveInL", (time, scene) => new CCTransitionMoveInL(time, scene)},
            {"CCTransitionMoveInR", (time, scene) => new CCTransitionMoveInR(time, scene)},
            {"CCTransitionMoveInT", (time, scene) => new CCTransitionMoveInT(time, scene)},
            {"CCTransitionMoveInB", (time, scene) => new CCTransitionMoveInB(time, scene)},
            {"CCTransitionSlideInL", (time, scene) => new CCTransitionSlideInL(time, scene)},
            {"CCTransitionSlideInR", (time, scene) => new CCTransitionSlideInR(time, scene)},
            {"CCTransitionSlideInT", (time, scene) => new CCTransitionSlideInT(time, scene)},
            {"CCTransitionSlideInB", (time, scene) => new CCTransitionSlideInB(time, scene)},
        };

        public override void runThisTest()
        {
            CCLayer pLayer = new TestLayer1();
            AddChild(pLayer);

            Director.ReplaceScene(this);
        }

        public static CCTransitionScene createTransition(int index, float time, CCScene scene)
        {
            // fix bug #486, without setDepthTest(false), FlipX,Y will flickers
            scene.GameView.DepthTesting = false;

            return transitions.Values.ElementAt(index)(time,scene);
        }
    }
}
