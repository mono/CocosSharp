using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LogicTest : ActionManagerTest
    {
        string s_pPathGrossini = "Images/grossini";

        public override string title()
        {
            return "Logic test";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSprite grossini = CCSprite.Create(s_pPathGrossini);
            AddChild(grossini, 0, 2);
            grossini.Position = (new CCPoint(200, 200));

            grossini.RunAction(CCSequence.Create(
                                                        new CCMoveBy (1, new CCPoint(150, 0)),
                                                        CCCallFuncN.Create(bugMe))
                                );
        }

        public void bugMe(CCNode node)
        {
            node.StopAllActions(); //After this stop next action not working, if remove this stop everything is working
            node.RunAction(CCScaleTo.Create(2, 2));
        }
    }
}
