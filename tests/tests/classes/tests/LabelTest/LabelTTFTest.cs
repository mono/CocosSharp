using System;
using cocos2d;

namespace tests
{
    public class LabelTTFTest : AtlasDemo
    {
        private CCTextAlignment m_eHorizAlign;
        private CCVerticalTextAlignment m_eVertAlign;
        private CCLabelTTF m_plabel;

        public LabelTTFTest()
        {
            var blockSize = new CCSize(200, 160);
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLayerColor colorLayer = new CCLayerColor(new CCColor4B(100, 100, 100, 255), blockSize.Width, blockSize.Height);
            colorLayer.AnchorPoint = new CCPoint(0, 0);
            colorLayer.Position = new CCPoint((s.Width - blockSize.Width) / 2, (s.Height - blockSize.Height) / 2);

            AddChild(colorLayer);

            CCMenuItemFont.FontSize = 30;
            CCMenu menu = new CCMenu(
                CCMenuItemFont.Create("Left", setAlignmentLeft),
                CCMenuItemFont.Create("Center", setAlignmentCenter),
                CCMenuItemFont.Create("Right", setAlignmentRight)
                );
            menu.AlignItemsVerticallyWithPadding(4);
            menu.Position = new CCPoint(50, s.Height / 2 - 20);
            AddChild(menu);

            menu = new CCMenu(
                CCMenuItemFont.Create("Top", setAlignmentTop),
                CCMenuItemFont.Create("Middle", setAlignmentMiddle),
                CCMenuItemFont.Create("Bottom", setAlignmentBottom)
                );
            menu.AlignItemsVerticallyWithPadding(4);
            menu.Position = new CCPoint(s.Width - 50, s.Height / 2 - 20);
            AddChild(menu);

            m_plabel = null;
            m_eHorizAlign = CCTextAlignment.CCTextAlignmentLeft;
            m_eVertAlign = CCVerticalTextAlignment.CCVerticalTextAlignmentTop;

            updateAlignment();
        }

        private void updateAlignment()
        {
            var blockSize = new CCSize(200, 160);
            CCSize s = CCDirector.SharedDirector.WinSize;

            if (m_plabel != null)
            {
                m_plabel.RemoveFromParentAndCleanup(true);
            }

            m_plabel = new CCLabelTTF(getCurrentAlignment(), "Marker Felt", 32,
                                         blockSize, m_eHorizAlign, m_eVertAlign);

            m_plabel.AnchorPoint = new CCPoint(0, 0);
            m_plabel.Position = new CCPoint((s.Width - blockSize.Width) / 2, (s.Height - blockSize.Height) / 2);

            AddChild(m_plabel);
        }

        private void setAlignmentLeft(object pSender)
        {
            m_eHorizAlign = CCTextAlignment.CCTextAlignmentLeft;
            updateAlignment();
        }

        private void setAlignmentCenter(object pSender)
        {
            m_eHorizAlign = CCTextAlignment.CCTextAlignmentCenter;
            updateAlignment();
        }

        private void setAlignmentRight(object pSender)
        {
            m_eHorizAlign = CCTextAlignment.CCTextAlignmentRight;
            updateAlignment();
        }

        private void setAlignmentTop(object pSender)
        {
            m_eVertAlign = CCVerticalTextAlignment.CCVerticalTextAlignmentTop;
            updateAlignment();
        }

        private void setAlignmentMiddle(object pSender)
        {
            m_eVertAlign = CCVerticalTextAlignment.CCVerticalTextAlignmentCenter;
            updateAlignment();
        }

        private void setAlignmentBottom(object pSender)
        {
            m_eVertAlign = CCVerticalTextAlignment.CCVerticalTextAlignmentBottom;
            updateAlignment();
        }

        private string getCurrentAlignment()
        {
            string vertical = "";
            string horizontal = "";
            switch (m_eVertAlign)
            {
                case CCVerticalTextAlignment.CCVerticalTextAlignmentTop:
                    vertical = "Top";
                    break;
                case CCVerticalTextAlignment.CCVerticalTextAlignmentCenter:
                    vertical = "Middle";
                    break;
                case CCVerticalTextAlignment.CCVerticalTextAlignmentBottom:
                    vertical = "Bottom";
                    break;
            }
            switch (m_eHorizAlign)
            {
                case CCTextAlignment.CCTextAlignmentLeft:
                    horizontal = "Left";
                    break;
                case CCTextAlignment.CCTextAlignmentCenter:
                    horizontal = "Center";
                    break;
                case CCTextAlignment.CCTextAlignmentRight:
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