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

            CCLayerColor colorLayer = CCLayerColor.Create(new ccColor4B(100, 100, 100, 255), blockSize.width, blockSize.height);
            colorLayer.AnchorPoint = new CCPoint(0, 0);
            colorLayer.Position = new CCPoint((s.width - blockSize.width) / 2, (s.height - blockSize.height) / 2);

            AddChild(colorLayer);

            CCMenuItemFont.FontSize = 30;
            CCMenu menu = CCMenu.Create(
                CCMenuItemFont.Create("Left", setAlignmentLeft),
                CCMenuItemFont.Create("Center", setAlignmentCenter),
                CCMenuItemFont.Create("Right", setAlignmentRight)
                );
            menu.AlignItemsVerticallyWithPadding(4);
            menu.Position = new CCPoint(50, s.height / 2 - 20);
            AddChild(menu);

            menu = CCMenu.Create(
                CCMenuItemFont.Create("Top", setAlignmentTop),
                CCMenuItemFont.Create("Middle", setAlignmentMiddle),
                CCMenuItemFont.Create("Bottom", setAlignmentBottom)
                );
            menu.AlignItemsVerticallyWithPadding(4);
            menu.Position = new CCPoint(s.width - 50, s.height / 2 - 20);
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

            m_plabel = CCLabelTTF.Create(getCurrentAlignment(), "Marker Felt", 32,
                                         blockSize, m_eHorizAlign, m_eVertAlign);

            m_plabel.AnchorPoint = new CCPoint(0, 0);
            m_plabel.Position = new CCPoint((s.width - blockSize.width) / 2, (s.height - blockSize.height) / 2);

            AddChild(m_plabel);
        }

        private void setAlignmentLeft(CCObject pSender)
        {
            m_eHorizAlign = CCTextAlignment.CCTextAlignmentLeft;
            updateAlignment();
        }

        private void setAlignmentCenter(CCObject pSender)
        {
            m_eHorizAlign = CCTextAlignment.CCTextAlignmentCenter;
            updateAlignment();
        }

        private void setAlignmentRight(CCObject pSender)
        {
            m_eHorizAlign = CCTextAlignment.CCTextAlignmentRight;
            updateAlignment();
        }

        private void setAlignmentTop(CCObject pSender)
        {
            m_eVertAlign = CCVerticalTextAlignment.CCVerticalTextAlignmentTop;
            updateAlignment();
        }

        private void setAlignmentMiddle(CCObject pSender)
        {
            m_eVertAlign = CCVerticalTextAlignment.CCVerticalTextAlignmentCenter;
            updateAlignment();
        }

        private void setAlignmentBottom(CCObject pSender)
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