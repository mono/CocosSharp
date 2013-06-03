using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

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
            CCLayerColor layer = new CCLayerColor(new CCColor4B(0xFF, 0x00, 0x00, 0x80), s.Width * 0.75f, s.Height * 0.75f);

            layer.IgnoreAnchorPointForPosition = false;
            layer.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            AddChild(layer, 1, kTagLayer);
            //
            // Add two labels using BM label class
            // CCLabelBMFont
            CCLabelBMFont label1 = new CCLabelBMFont("LABEL1", "fonts/konqa32.fnt");
            layer.AddChild(label1);
            label1.Position = new CCPoint(layer.ContentSize.Width / 2, layer.ContentSize.Height * 0.75f);
            CCLabelBMFont label2 = new CCLabelBMFont("LABEL2", "fonts/konqa32.fnt");
            layer.AddChild(label2);
            label2.Position = new CCPoint(layer.ContentSize.Width / 2, layer.ContentSize.Height * 0.25f);
            //
            // Do the sequence of actions in the bug report
            float waitTime = 3f;
            float runTime = 12f;
            layer.Visible = false;
            CCHide hide = new CCHide();
            CCScaleTo scaleTo1 = new CCScaleTo(0.0f, 0.0f);
            CCShow show = new CCShow();
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

    public class LayerClipScissor : LayerTest
    {
        protected CCLayer m_pInnerLayer;

        public override void OnEnter()
        {
            base.OnEnter();

            this.TouchEnabled = true;

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLayerColor layer1 = new CCLayerColor(new CCColor4B(0xFF, 0xFF, 0x00, 0x80), s.Width * 0.75f, s.Height * 0.75f);
            layer1.IgnoreAnchorPointForPosition = false;
            layer1.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            layer1.ChildClippingMode = CCClipMode.ClipBounds;
            AddChild(layer1, 1);

            s = layer1.ContentSize;

            m_pInnerLayer = new CCLayerColor(new CCColor4B(0xFF, 0x00, 0x00, 0x80), s.Width * 0.5f, s.Height * 0.5f);
            m_pInnerLayer.IgnoreAnchorPointForPosition = false;
            m_pInnerLayer.Position = (new CCPoint(s.Width / 2, s.Height / 2));
            m_pInnerLayer.ChildClippingMode = CCClipMode.ClipBounds;
            
            layer1.AddChild(m_pInnerLayer, 1);
            
            //
            // Add two labels using BM label class
            // CCLabelBMFont
            CCLabelBMFont label1 = new CCLabelBMFont("LABEL1", "fonts/konqa32.fnt");
            label1.Position = new CCPoint(m_pInnerLayer.ContentSize.Width, m_pInnerLayer.ContentSize.Height * 0.75f);
            m_pInnerLayer.AddChild(label1);
            
            CCLabelBMFont label2 = new CCLabelBMFont("LABEL2", "fonts/konqa32.fnt");
            label2.Position = new CCPoint(0, m_pInnerLayer.ContentSize.Height * 0.25f);
            m_pInnerLayer.AddChild(label2);

            float runTime = 12f;

            CCScaleTo scaleTo2 = new CCScaleTo(runTime * 0.25f, 3.0f);
            CCScaleTo scaleTo3 = new CCScaleTo(runTime * 0.25f, 1.0f);

            CCFiniteTimeAction seq = new CCRepeatForever(
                CCSequence.FromActions(scaleTo2, scaleTo3)
                );

            m_pInnerLayer.RunAction(seq);

            CCSize size = CCDirector.SharedDirector.WinSize;

            var move1 = new CCMoveTo(2, new CCPoint(size.Width / 2, size.Height));
            var move2 = new CCMoveTo(2, new CCPoint(size.Width, size.Height / 2));
            var move3 = new CCMoveTo(2, new CCPoint(size.Width / 2, 0));
            var move4 = new CCMoveTo(2, new CCPoint(0, size.Height / 2));

            layer1.RunAction(new CCRepeatForever(CCSequence.FromActions(move1, move2, move3, move4)));

        }

        public override string title()
        {
            return "Layer Clipping With Scissor";
        }
    }

    public class LayerClippingTexture : LayerClipScissor
    {
        public override void OnEnter()
        {
            base.OnEnter();

            m_pInnerLayer.ChildClippingMode = CCClipMode.ClipBoundsWithRenderTarget;

            var rotateBy = new CCRotateBy(3, 90);

            m_pInnerLayer.RunAction(new CCRepeatForever(rotateBy));
        }

        public override string title()
        {
            return "Layer Clipping With Texture";
        }
    }
}
