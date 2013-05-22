using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class LayerTest2 : LayerTest
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;
            CCLayerColor layer1 = new CCLayerColor(new CCColor4B(255, 255, 0, 80), 100, 300);
            layer1.Position = (new CCPoint(s.Width / 3, s.Height / 2));
            layer1.IgnoreAnchorPointForPosition = false;
            AddChild(layer1, 1);

            CCLayerColor layer2 = new CCLayerColor(new CCColor4B(0, 0, 255, 255), 100, 300);
            layer2.Position = (new CCPoint((s.Width / 3) * 2, s.Height / 2));
            layer2.IgnoreAnchorPointForPosition = false;
            AddChild(layer2, 1);

            CCActionInterval actionTint = new CCTintBy (2, -255, -127, 0);
            CCActionInterval actionTintBack = (CCActionInterval)actionTint.Reverse();
            CCActionInterval seq1 = (CCActionInterval)CCSequence.FromActions(actionTint, actionTintBack);
            layer1.RunAction(seq1);

            CCActionInterval actionFade = new CCFadeOut  (2.0f);
            CCActionInterval actionFadeBack = (CCActionInterval)actionFade.Reverse();
            CCActionInterval seq2 = (CCActionInterval)CCSequence.FromActions(actionFade, actionFadeBack);
            layer2.RunAction(seq2);
        }

        public override string title()
        {
            return "ColorLayer: fade and tint";
        }
    }
}
