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

            var s = CCDirector.SharedDirector.WinSize;
            var layer1 = new CCLayerColor(new CCColor4B(255, 255, 0, 80), 100, s.Height - 50);
            layer1.Position = (new CCPoint(s.Width / 3, s.Height / 2));
            layer1.IgnoreAnchorPointForPosition = false;
            AddChild(layer1, 1);

            var layer2 = new CCLayerColor(new CCColor4B(0, 0, 255, 255), 100, s.Height - 50);
            layer2.Position = (new CCPoint((s.Width / 3) * 2, s.Height / 2));
            layer2.IgnoreAnchorPointForPosition = false;
            AddChild(layer2, 1);

            var actionTint = new CCTintBy (2, -255, -127, 0);
            var actionTintBack = actionTint.Reverse();
            var seq1 = new CCSequence(actionTint, actionTintBack);
            layer1.RunAction(seq1);

            var actionFade = new CCFadeOut(2.0f);
            var actionFadeBack = actionFade.Reverse();
            var seq2 = new CCSequence(actionFade, actionFadeBack);
            layer2.RunAction(seq2);
        }

        public override string title()
        {
            return "ColorLayer: fade and tint";
        }
    }
}
