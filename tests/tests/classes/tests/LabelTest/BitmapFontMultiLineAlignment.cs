using System;
using System.Collections.Generic;
using cocos2d;

namespace tests
{
    public class BitmapFontMultiLineAlignment : AtlasDemo
    {
        private const string LongSentencesExample =
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

        private const string LineBreaksExample = "Lorem ipsum dolor\nsit amet\nconsectetur adipisicing elit\nblah\nblah";
        private const string MixedExample = "ABC\nLorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt\nDEF";

        private const float ArrowsMax = 0.95f;
        private const float ArrowsMin = 0.7f;

        private const int LeftAlign = 0;
        private const int CenterAlign = 1;
        private const int RightAlign = 2;

        private const int LongSentences = 0;
        private const int LineBreaks = 1;
        private const int Mixed = 2;

        private static float alignmentItemPadding = 50f;
        private static float menuItemPaddingCenter = 50f;

        private readonly CCSprite m_pArrowsBarShouldRetain;
        private readonly CCSprite m_pArrowsShouldRetain;
        private readonly CCLabelBMFont m_pLabelShouldRetain;
        private bool m_drag;
        private CCMenuItemFont m_pLastAlignmentItem;
        private CCMenuItemFont m_pLastSentenceItem;

        public BitmapFontMultiLineAlignment()
        {
            TouchEnabled = true;

            // ask director the the window size
            CCSize size = CCDirector.SharedDirector.WinSize;

            // create and initialize a Label
            m_pLabelShouldRetain = CCLabelBMFont.Create(LongSentencesExample, "fonts/markerFelt.fnt", size.Width / 1.5f,
                                                        CCTextAlignment.CCTextAlignmentCenter);

            m_pArrowsBarShouldRetain = CCSprite.Create("Images/arrowsBar");
            m_pArrowsShouldRetain = CCSprite.Create("Images/arrows");

            CCMenuItemFont.FontSize = 20;
            CCMenuItemFont longSentences = CCMenuItemFont.Create("Long Flowing Sentences", stringChanged);
            CCMenuItemFont lineBreaks = CCMenuItemFont.Create("Short Sentences With Intentional Line Breaks", stringChanged);
            CCMenuItemFont mixed = CCMenuItemFont.Create("Long Sentences Mixed With Intentional Line Breaks", stringChanged);
            CCMenu stringMenu = CCMenu.Create(longSentences, lineBreaks, mixed);
            stringMenu.AlignItemsVertically();

            longSentences.Color = CCTypes.CCRed;
            m_pLastSentenceItem = longSentences;
            longSentences.Tag = LongSentences;
            lineBreaks.Tag = LineBreaks;
            mixed.Tag = Mixed;

            CCMenuItemFont.FontSize = 30;

            CCMenuItemFont left = CCMenuItemFont.Create("Left", alignmentChanged);
            CCMenuItemFont center = CCMenuItemFont.Create("Center", alignmentChanged);
            CCMenuItemFont right = CCMenuItemFont.Create("Right", alignmentChanged);
            CCMenu alignmentMenu = CCMenu.Create(left, center, right);
            alignmentMenu.AlignItemsHorizontallyWithPadding(alignmentItemPadding);

            center.Color = CCTypes.CCRed;
            m_pLastAlignmentItem = center;
            left.Tag = (LeftAlign);
            center.Tag = (CenterAlign);
            right.Tag = (RightAlign);

            // position the label on the center of the screen
            m_pLabelShouldRetain.Position = new CCPoint(size.Width / 2, size.Height / 2);

            m_pArrowsBarShouldRetain.Visible = (false);

            float arrowsWidth = (ArrowsMax - ArrowsMin) * size.Width;
            m_pArrowsBarShouldRetain.ScaleX = (arrowsWidth / m_pArrowsBarShouldRetain.ContentSize.Width);
            m_pArrowsBarShouldRetain.Position = new CCPoint(((ArrowsMax + ArrowsMin) / 2) * size.Width, m_pLabelShouldRetain.Position.y);

            snapArrowsToEdge();

            stringMenu.Position = new CCPoint(size.Width / 2, size.Height - menuItemPaddingCenter);
            alignmentMenu.Position = new CCPoint(size.Width / 2, menuItemPaddingCenter + 15);

            AddChild(m_pLabelShouldRetain);
            AddChild(m_pArrowsBarShouldRetain);
            AddChild(m_pArrowsShouldRetain);
            AddChild(stringMenu);
            AddChild(alignmentMenu);
        }

        private void stringChanged(CCObject sender)
        {
            var item = (CCMenuItemFont) sender;
            item.Color = CCTypes.CCRed;
            m_pLastAlignmentItem.Color = CCTypes.CCWhite;
            m_pLastAlignmentItem = item;

            switch (item.Tag)
            {
                case LongSentences:
                    m_pLabelShouldRetain.SetString(LongSentencesExample);
                    break;
                case LineBreaks:
                    m_pLabelShouldRetain.SetString(LineBreaksExample);
                    break;
                case Mixed:
                    m_pLabelShouldRetain.SetString(MixedExample);
                    break;

                default:
                    break;
            }

            snapArrowsToEdge();
        }

        private void alignmentChanged(CCObject sender)
        {
            var item = (CCMenuItemFont) sender;
            item.Color = CCTypes.CCRed;
            m_pLastAlignmentItem.Color = CCTypes.CCWhite;
            m_pLastAlignmentItem = item;

            switch (item.Tag)
            {
                case LeftAlign:
                    m_pLabelShouldRetain.SetAlignment(CCTextAlignment.CCTextAlignmentLeft);
                    break;
                case CenterAlign:
                    m_pLabelShouldRetain.SetAlignment(CCTextAlignment.CCTextAlignmentCenter);
                    break;
                case RightAlign:
                    m_pLabelShouldRetain.SetAlignment(CCTextAlignment.CCTextAlignmentRight);
                    break;

                default:
                    break;
            }

            snapArrowsToEdge();
        }

        public override void TouchesBegan(List<CCTouch> pTouches, CCEvent pEvent)
        {
            CCTouch touch = pTouches[0];
            CCPoint location = touch.LocationInView;

            if (m_pArrowsShouldRetain.BoundingBox.ContainsPoint(location))
            {
                m_drag = true;
                m_pArrowsBarShouldRetain.Visible = true;
            }
        }

        public override void TouchesEnded(List<CCTouch> pTouches, CCEvent pEvent)
        {
            m_drag = false;
            snapArrowsToEdge();

            m_pArrowsBarShouldRetain.Visible = false;
        }

        public override void TouchesMoved(List<CCTouch> pTouches, CCEvent pEvent)
        {
            if (!m_drag)
            {
                return;
            }

            CCTouch touch = pTouches[0];
            CCPoint location = touch.LocationInView;

            CCSize winSize = CCDirector.SharedDirector.WinSize;

            m_pArrowsShouldRetain.Position = new CCPoint(Math.Max(Math.Min(location.x, ArrowsMax * winSize.Width), ArrowsMin * winSize.Width),
                                                         m_pArrowsShouldRetain.Position.y);

            float labelWidth = Math.Abs(m_pArrowsShouldRetain.Position.x - m_pLabelShouldRetain.Position.x) * 2;

            m_pLabelShouldRetain.SetWidth(labelWidth);
        }

        private void snapArrowsToEdge()
        {
            m_pArrowsShouldRetain.Position =
                new CCPoint(m_pLabelShouldRetain.Position.x + m_pLabelShouldRetain.ContentSize.Width / 2,
                            m_pLabelShouldRetain.Position.y);
        }

        public override string title()
        {
            return "";
        }
    }
}