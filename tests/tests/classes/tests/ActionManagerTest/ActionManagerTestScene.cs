using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ActionManagerTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = ActionManagerTest.nextActionManagerAction();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}
