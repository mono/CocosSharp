using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SceneTestScene : TestScene
    {
		public const int GROSSINI_TAG = 1;
		public static CCActionInterval rotate = new CCRotateBy (2, 360);
		public static string grossini = "Images/grossini";

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
            CCLayer pLayer = new SceneTestLayer1();
            AddChild(pLayer);

            Director.ReplaceScene(this);
        }
    }
}
