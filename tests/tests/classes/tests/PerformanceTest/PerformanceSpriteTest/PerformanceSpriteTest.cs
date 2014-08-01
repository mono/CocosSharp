using CocosSharp;

namespace tests
{
    public class PerformanceSpriteTest
    {
        public static int kMaxNodes = 5000;
        public static int kNodesIncrease = 50;
        public static int TEST_COUNT = 7;

        public static int kTagInfoLayer = 1;
        public static int kTagMainLayer = 2;
        public static int kTagMenuLayer = (kMaxNodes + 1000);

        public static int s_nSpriteCurCase = 0;

        public static void runSpriteTest()
        {
            SpriteMainScene pScene = new SpritePerformTest1();
            pScene.initWithSubTest(1, 50);
            AppDelegate.SharedWindow.DefaultDirector.ReplaceScene(pScene);
        }
    }
}