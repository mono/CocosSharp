using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class PerformanceTouchesTest
    {

        public static int TEST_COUNT = 2;
        public static int s_nTouchCurCase = 0;

        public static void runTouchesTest()
        {
            s_nTouchCurCase = 0;
            CCScene pScene = new CCScene();
            CCLayer pLayer = new TouchesPerformTest1(true, TEST_COUNT, s_nTouchCurCase);

            pScene.AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(pScene);
        }
    }
}
