using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class DrawPrimitivesTestScene : TestScene
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
            CCLayer pLayer = new DrawPrimitivesTest();
            AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}
