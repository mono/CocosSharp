using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{

    public class CrashTest : ActionManagerTest
    {
        string s_pPathGrossini = "Images/grossini";

        public override string title()
        {
            return "Test 1. Should not crash";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSprite child = new CCSprite(s_pPathGrossini);
            child.Position = (new CCPoint(200, 200));
            AddChild(child, 1);

            //Sum of all action's duration is 1.5 second.
            child.RunAction(new CCRotateBy (1.5f, 90));
            child.RunAction(CCSequence.FromActions(
                                                    new CCDelayTime (1.4f),
                                                    new CCFadeOut  (1.1f))
                            );

            //After 1.5 second, self will be removed.
            RunAction(CCSequence.FromActions(
                                            new CCDelayTime (1.4f),
                                            CCCallFunc.Create((removeThis)))
                     );
        }

        public void removeThis()
        {
            m_pParent.RemoveChild(this, true);

            nextCallback(this);
        }
    }
}
