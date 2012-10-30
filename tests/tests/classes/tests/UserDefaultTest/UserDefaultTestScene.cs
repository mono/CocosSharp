using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class UserDefaultTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = new UserDefaultTest();
            AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}
