using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LayerScaleTest : LayerTest
    {
        int kTagLayer = 1;
        int kCCMenuTouchPriority = -128;

        public override void OnEnter()
        {
            base.OnEnter();

            this.TouchEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;
            CCLayerColor layer = CCLayerColor.Create(new CCColor4B(0xFF, 0x00, 0x00, 0x80), s.Width * 0.75f, s.Height * 0.75f);

            layer.IgnoreAnchorPointForPosition = false;
            layer.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            AddChild(layer, 1, kTagLayer);
            //
            // Add two labels using BM label class
            // CCLabelBMFont
            CCLabelBMFont label1 = CCLabelBMFont.Create("LABEL1", "fonts/konqa32.fnt");
            layer.AddChild(label1);
            label1.Position = new CCPoint(layer.ContentSize.Width / 2, layer.ContentSize.Height * 0.75f);
            CCLabelBMFont label2 = CCLabelBMFont.Create("LABEL2", "fonts/konqa32.fnt");
            layer.AddChild(label2);
            label2.Position = new CCPoint(layer.ContentSize.Width / 2, layer.ContentSize.Height * 0.25f);
            //
            // Do the sequence of actions in the bug report
            float waitTime = 3f;
            float runTime = 12f;
            layer.Visible = false;
            CCHide hide = CCHide.Create();
            CCScaleTo scaleTo1 = new CCScaleTo(0.0f, 0.0f);
            CCShow show = CCShow.Create();
            CCDelayTime delay = new CCDelayTime (waitTime);
            CCScaleTo scaleTo2 = new CCScaleTo(runTime * 0.25f, 1.2f);
            CCScaleTo scaleTo3 = new CCScaleTo(runTime * 0.25f, 0.95f);
            CCScaleTo scaleTo4 = new CCScaleTo(runTime * 0.25f, 1.1f);
            CCScaleTo scaleTo5 = new CCScaleTo(runTime * 0.25f, 1.0f);

            CCFiniteTimeAction seq = CCSequence.FromActions(hide, scaleTo1, show, delay, scaleTo2, scaleTo3, scaleTo4, scaleTo5);

            layer.RunAction(seq);


        }

        public override string title()
        {
            return "Layer Scale With BM Font";
        }

    }
}
