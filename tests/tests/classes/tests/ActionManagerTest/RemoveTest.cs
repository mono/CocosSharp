using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public enum KTag
    {
        kTagNode,
        kTagGrossini,
        kTagSequence,
    }
    public class RemoveTest : ActionManagerTest
    {
        int kTagGrossini = 1;
        string s_pPathGrossini = "Images/grossini";

        public override string title() 
        {
            return "Remove Test";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF l = CCLabelTTF.Create("Should not crash", "arial", 16);
            AddChild(l);
            l.Position = (new CCPoint(s.width / 2, 245));

            CCMoveBy pMove = CCMoveBy.Create(2, new CCPoint(200, 0));
            CCCallFunc pCallback = CCCallFunc.Create(stopAction);
            CCActionInterval pSequence = (CCActionInterval)CCSequence.Create(pMove, pCallback);
            pSequence.Tag = (int)KTag.kTagSequence;

            CCSprite pChild = CCSprite.Create(s_pPathGrossini);
            pChild.Position = (new CCPoint(200, 200));

            AddChild(pChild, 1, kTagGrossini);
            pChild.RunAction(pSequence);
        }

        public void stopAction()
        {
            CCNode pSprite = GetChildByTag(kTagGrossini);
            pSprite.StopActionByTag((int)KTag.kTagSequence);
        }
    }
}
