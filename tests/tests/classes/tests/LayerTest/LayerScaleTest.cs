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

    //=============================== MarqueeLayer ================================

    public class MarqueeLayerTest : LayerTest
    {
        public override void OnEnter()
        {
            base.OnEnter();

            var layer = new MarqueeLayer();
            layer.IgnoreAnchorPointForPosition = false;
            
            AddChild(layer);

            var size = CCDirector.SharedDirector.WinSize;

            layer.Position = new CCPoint(0, size.Height / 2);

            var move1 = new CCMoveTo(2, new CCPoint(size.Width / 2, size.Height));
            var move2 = new CCMoveTo(2, new CCPoint(size.Width, size.Height / 2));
            var move3 = new CCMoveTo(2, new CCPoint(size.Width / 2, 0));
            var move4 = new CCMoveTo(2, new CCPoint(0, size.Height / 2));

            layer.RunAction(new CCRepeatForever(CCSequence.FromActions(move1, move2, move3, move4)));
        }

        public override string title()
        {
            return "issues - 173 Marquee Layer";
        }
    }

    public partial class MarqueeLayer : CCLayerColor
    {
        private bool m_didOnEnter = false;
        private bool m_didOnExit = false;
        protected const int kTopBorder = 1;
        private CCSprite TopBorder;
        protected const int kBottomBorder = 2;
        private CCSprite BottomBorder;
        public MarqueeLayer()
            : base(new CCColor4B(0x00, 0x00, 0x00, 0xFF), 320f, 422f)
        {
            ChildClippingMode = CCClipMode.ClipBounds;
            Layer_Constructed();
            TopBorder = new CCSprite("Images/blocks");
            TopBorder.Position = new CCPoint(320 / 2, 422 - 32 / 2);
            TopBorder.ContentSize = new CCSize(320, 32);
            TopBorder.Visible = true;
            TopBorder.Scale = 1;
            this.AddChild(TopBorder, 5, kTopBorder);
            BottomBorder = new CCSprite("Images/blocks");
            BottomBorder.Position = new CCPoint(320 / 2, 32 / 2);
            BottomBorder.ContentSize = new CCSize(320, 32);
            BottomBorder.Visible = true;
            BottomBorder.Scale = 1;
            BottomBorder.FlipY = true;
            this.AddChild(BottomBorder, 5, kBottomBorder);
        }
        public override void OnEnter()
        {
            if (!m_didOnEnter)
            {
                m_didOnEnter = true;
            }
            base.OnEnter();
            Layer_OnEnter();
        }
        public override void OnExit()
        {
            if (!m_didOnExit)
            {
                m_didOnExit = true;
            }
            base.OnExit();
            Layer_OnExit();
        }
        partial void Layer_Constructed();
        partial void Layer_OnEnter();
        partial void Layer_OnExit();
    }

    public partial class MarqueeLayer : CCLayerColor
    {
        protected Action<float> MarqueeUpdater;
        private float _MarqueeRate = 15f; // 15 pixels per second
        private List<CCNode> _Labels = new List<CCNode>();
        private string[] _LabelsToShow = new string[] { 
        "Game Art: Chelsea Crist",
        "Game Art: Emily So",
        "Backgrounds: Jonny Klein",
        "Programming: Jacob Aderson",
        "Design: Jacob Anderson",
        "Writing: Jacob Anderson",
        "MonoGame - Cross Platform",
        "Cocos2D XNA - Platform"
    };
        private int _LastNode = 0;

        partial void Layer_Constructed()
        {
            MarqueeUpdater = MarqueeUpdate;
            Schedule(MarqueeUpdater, 1f / 30f);
            for (int i = 0; i < 2; i++)
            {
                foreach (string s in _LabelsToShow)
                {
                    CCLabelTTF label = new CCLabelTTF(s, "Eccentric", 18);
                    label.Color = new CCColor3B(255, 253, 119);
                    label.HorizontalAlignment = CCTextAlignment.CCTextAlignmentCenter;
                    AddChild(label);
                    _Labels.Add(label);
                    //       label.AnchorPoint = new CCPoint(0.5f, 0f);
                }
            }
            _bPositionsAreDirty = true;
        }

        partial void Layer_OnEnter()
        {
            LayoutNodes();
        }

        private bool _bPositionsAreDirty = true;

        public override CCSize ContentSize
        {
            get
            {
                return base.ContentSize;
            }
            set
            {
                base.ContentSize = value;
                _bPositionsAreDirty = true;
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            if (_bPositionsAreDirty)
            {
                // Reset the positions
                LayoutNodes();
            }
        }

        private float m_MarqueeBorder = 0f; // 32f;
        private float m_Padding = 7f;

        private void LayoutNodes()
        {
            _bPositionsAreDirty = false;
            float ymax = ContentSize.Height - m_MarqueeBorder;
            float xmid = ContentSize.Width / 2f;
            foreach (CCNode node in _Labels)
            {
                node.PositionY = ymax - node.ContentSize.Height;
                node.PositionX = xmid;
                ymax -= node.ContentSize.Height + m_Padding;
            }
            _LastNode = _Labels.Count - 1;
        }

        private void MarqueeUpdate(float dt)
        {
            if (_bPositionsAreDirty)
            {
                LayoutNodes();
                return;
            }
            float ymax = ContentSize.Height - m_MarqueeBorder;
            foreach (CCNode node in _Labels)
            {
                node.PositionY += _MarqueeRate * dt;
                // If any nodes are not visible, then move them to the bottom or top
                if (node.BoundingBox.MinY > ymax)
                {
                    // Place this at the bottom of the list
                    float y = _Labels[_LastNode].PositionY;
                    y -= node.ContentSize.Height + m_Padding;
                    node.PositionY = y;
                    _LastNode = (_LastNode + 1) % _Labels.Count;
                }
            }

        }
    }

    //=============================== MarqueeLayer ================================

}
