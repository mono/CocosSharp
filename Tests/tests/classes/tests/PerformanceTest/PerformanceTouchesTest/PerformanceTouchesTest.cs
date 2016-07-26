using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PerformanceTouchesTest
    {

        public static int TEST_COUNT = 2;
        public static int s_nTouchCurCase = 0;

        public static void runTouchesTest()
        {
            s_nTouchCurCase = 0;
            CCScene pScene = new CCScene(AppDelegate.SharedWindow);
            CCLayer pLayer = new TouchesPerformTest1(true, TEST_COUNT, s_nTouchCurCase);

            pScene.AddChild(pLayer);

            AppDelegate.SharedWindow.Director.ReplaceScene(pScene);
        }
    }
}
