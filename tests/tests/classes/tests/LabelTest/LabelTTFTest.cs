using System;
using CocosSharp;

namespace tests
{
    public class LabelTTFTest : AtlasDemo
    {
        private CCTextAlignment m_eHorizAlign;
        private CCVerticalTextAlignment m_eVertAlign;
        private CCLabelTtf m_plabel;

        public LabelTTFTest()
        {
            var blockSize = new CCSize(200, 160);
			var s = CCDirector.SharedDirector.WinSize;

			var colorLayer = new CCLayerColor(new CCColor4B(100, 100, 100, 255), blockSize.Width, blockSize.Height);
			colorLayer.AnchorPoint = CCPoint.Zero;
            colorLayer.Position = new CCPoint((s.Width - blockSize.Width) / 2, (s.Height - blockSize.Height) / 2);

            AddChild(colorLayer);

			CCMenuItemFont.FontSize = 32;
			CCMenuItemFont.FontName = "MarkerFelt";

			var menu = new CCMenu(
				new CCMenuItemFont("Left", setAlignmentLeft),
				new CCMenuItemFont("Center", setAlignmentCenter),
				new CCMenuItemFont("Right", setAlignmentRight)
                );
            menu.AlignItemsVertically(4);
            menu.Position = new CCPoint(50, s.Height / 2 - 20);
            AddChild(menu);

            menu = new CCMenu(
				new CCMenuItemFont("Top", setAlignmentTop),
				new CCMenuItemFont("Middle", setAlignmentMiddle),
				new CCMenuItemFont("Bottom", setAlignmentBottom)
                );
            menu.AlignItemsVertically(4);
            menu.Position = new CCPoint(s.Width - 50, s.Height / 2 - 20);
            AddChild(menu);

            m_plabel = null;
            m_eHorizAlign = CCTextAlignment.Left;
            m_eVertAlign = CCVerticalTextAlignment.Top;

            updateAlignment();
        }

        private void updateAlignment()
        {
            var blockSize = new CCSize(200, 160);
			var s = CCDirector.SharedDirector.WinSize;

            if (m_plabel != null)
            {
                m_plabel.RemoveFromParent(true);
            }

            m_plabel = new CCLabelTtf(getCurrentAlignment(), "MarkerFelt", 32,
                                         blockSize, m_eHorizAlign, m_eVertAlign);

            m_plabel.AnchorPoint = new CCPoint(0, 0);
            m_plabel.Position = new CCPoint((s.Width - blockSize.Width) / 2, (s.Height - blockSize.Height) / 2);

            AddChild(m_plabel);
        }

        private void setAlignmentLeft(object pSender)
        {
            m_eHorizAlign = CCTextAlignment.Left;
            updateAlignment();
        }

        private void setAlignmentCenter(object pSender)
        {
            m_eHorizAlign = CCTextAlignment.Center;
            updateAlignment();
        }

        private void setAlignmentRight(object pSender)
        {
            m_eHorizAlign = CCTextAlignment.Right;
            updateAlignment();
        }

        private void setAlignmentTop(object pSender)
        {
            m_eVertAlign = CCVerticalTextAlignment.Top;
            updateAlignment();
        }

        private void setAlignmentMiddle(object pSender)
        {
            m_eVertAlign = CCVerticalTextAlignment.Center;
            updateAlignment();
        }

        private void setAlignmentBottom(object pSender)
        {
            m_eVertAlign = CCVerticalTextAlignment.Bottom;
            updateAlignment();
        }

        private string getCurrentAlignment()
        {
			var vertical = "";
			var horizontal = "";
            switch (m_eVertAlign)
            {
                case CCVerticalTextAlignment.Top:
                    vertical = "Top";
                    break;
                case CCVerticalTextAlignment.Center:
                    vertical = "Middle";
                    break;
                case CCVerticalTextAlignment.Bottom:
                    vertical = "Bottom";
                    break;
            }
            switch (m_eHorizAlign)
            {
                case CCTextAlignment.Left:
                    horizontal = "Left";
                    break;
                case CCTextAlignment.Center:
                    horizontal = "Center";
                    break;
                case CCTextAlignment.Right:
                    horizontal = "Right";
                    break;
            }

            return String.Format("Alignment {0} {1}", vertical, horizontal);
        }

        public override string title()
        {
            return "Testing CCLabelTTF";
        }

        public override string subtitle()
        {
            return "You should see 3 labels aligned left, center and right";
        }
    }
}