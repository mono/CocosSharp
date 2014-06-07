using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PerformanceTestScene : TestScene
    {
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
        public override void runThisTest()
        {
            CCLayer pLayer = new PerformanceMainLayer();
            AddChild(pLayer);

            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(this);
        }

        public static int MAX_COUNT = 5;
        public static int LINE_SPACE = 40;
        public static int kItemTagBasic = 1000;
        public static string[] testsName = new string[5] {
            "PerformanceNodeChildrenTest",
            "PerformanceParticleTest",
            "PerformanceSpriteTest",
            "PerformanceTextureTest",
            "PerformanceTouchesTest"};
    }
}
