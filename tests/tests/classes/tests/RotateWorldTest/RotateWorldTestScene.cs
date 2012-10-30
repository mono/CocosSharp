using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class RotateWorldTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer = RotateWorldMainLayer.node();
            AddChild(pLayer);
            RunAction(CCRotateBy.Create(4, -360));
            CCDirector.SharedDirector.ReplaceScene(this);
        }
    }
}
