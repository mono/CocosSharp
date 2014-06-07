using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class BugsTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = new BugsTestMainLayer();
            AddChild(pLayer);

            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(this);
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

        public static int MAX_COUNT = 9;
        public static int LINE_SPACE = 40;
        public static int kItemTagBasic = 5432;
        public static CCPoint s_tCurPos = new CCPoint(0, 0);

        public static string[] testsName = new string[] 
            { "Bug-350",
              "Bug-422",
              "Bug-458",
              "Bug-624",
              "Bug-886",
              "Bug-899",
              "Bug-914",
              "Bug-1159",
              "Bug-1174"
            };
    }
}
