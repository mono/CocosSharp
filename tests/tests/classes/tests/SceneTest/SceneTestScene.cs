using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SceneTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = new SceneTestLayer1();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}
