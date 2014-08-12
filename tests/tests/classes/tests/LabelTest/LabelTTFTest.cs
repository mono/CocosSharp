using System;
using CocosSharp;

namespace tests
{

    public class AlignmentPanel : CCDrawNode
    {

        public AlignmentPanel (CCColor4B color)
        {
            Color = new CCColor3B(color);
            Opacity = color.A;
            //AnchorPoint = CCPoint.AnchorLowerLeft;
        }

        public AlignmentPanel(CCSize size, CCColor4B color)
        {
            Color = new CCColor3B(color);
            Opacity = color.A;
            //AnchorPoint = CCPoint.AnchorMiddle;
            ContentSize = size;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var fillColor = new CCColor4B(Color.R, Color.G, Color.B, Opacity);
            DrawRect(new CCRect(0,0,ContentSize.Width,ContentSize.Height), fillColor, 1f, CCColor4B.White);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            if (ContentSize == CCSize.Zero)
                ContentSize = VisibleBoundsWorldspace.Size;
        }
    }

    public class LabelTTFTest : AtlasDemo
    {
        private CCTextAlignment horizontalAlign;
        private CCVerticalTextAlignment verticalAlign;
        private CCLabelTtf alignmentLabel;
        private Background colorLayer;
        private CCSize blockSize = new CCSize(200, 160);
        private CCMenu menuLeft;
        private CCMenu menuRight;

        public LabelTTFTest()
        {

            CCMenuItemFont.FontSize = 32;
            CCMenuItemFont.FontName = "MarkerFelt";

            menuLeft = new CCMenu(
                new CCMenuItemFont("Left", setAlignmentLeft),
                new CCMenuItemFont("Center", setAlignmentCenter),
                new CCMenuItemFont("Right", setAlignmentRight)
            );
            menuLeft.AlignItemsVertically(4);

            AddChild(menuLeft);

            menuRight = new CCMenu(
                new CCMenuItemFont("Top", setAlignmentTop),
                new CCMenuItemFont("Middle", setAlignmentMiddle),
                new CCMenuItemFont("Bottom", setAlignmentBottom)
            );
            menuRight.AlignItemsVertically(4);

            AddChild(menuRight);

        }

        public override void OnEnter()
        {
            base.OnEnter();

            var s = VisibleBoundsWorldspace.Size;

            menuLeft.Position = new CCPoint(50, s.Height / 2 - 20);
            menuRight.Position = new CCPoint(s.Width - 50, s.Height / 2 - 20);


            alignmentLabel = null;
            horizontalAlign = CCTextAlignment.Left;
            verticalAlign = CCVerticalTextAlignment.Top;


            blockSize = new CCSize(s.Width / 3, s.Height / 2);
            //blockSize = new CCSize(50, 50);
            var leftPanel = new AlignmentPanel(blockSize, new CCColor4B(100, 100, 100, 255));
            var centerPanel = new AlignmentPanel(blockSize, new CCColor4B(200, 100, 100, 255));
            var rightPanel = new AlignmentPanel(blockSize, new CCColor4B(100, 100, 200, 255));

            leftPanel.IgnoreAnchorPointForPosition = false;
            centerPanel.IgnoreAnchorPointForPosition = false;
            rightPanel.IgnoreAnchorPointForPosition = false;

            leftPanel.AnchorPoint = new CCPoint(0, 0.5f);
            centerPanel.AnchorPoint = new CCPoint(0, 0.5f);
            rightPanel.AnchorPoint = new CCPoint(0, 0.5f);

            leftPanel.Position = new CCPoint(0, s.Height / 2);
            centerPanel.Position = new CCPoint(blockSize.Width, s.Height / 2);
            rightPanel.Position = new CCPoint(blockSize.Width * 2, s.Height / 2);

            AddChild(leftPanel, -1);
            AddChild(rightPanel, -1);
            AddChild(centerPanel, -1);

            updateAlignment();
        }

        private void updateAlignment()
        {
			var s = VisibleBoundsWorldspace.Size;

            if (alignmentLabel != null)
            {
                alignmentLabel.RemoveFromParent(true);
            }

            alignmentLabel = new CCLabelTtf(getCurrentAlignment(), "MarkerFelt", 32,
                                         blockSize, horizontalAlign, verticalAlign);

            alignmentLabel.AnchorPoint = new CCPoint(0, 0);
            alignmentLabel.Position = new CCPoint((s.Width - blockSize.Width) / 2, (s.Height - blockSize.Height) / 2);

            AddChild(alignmentLabel);
        }

        private void setAlignmentLeft(object pSender)
        {
            horizontalAlign = CCTextAlignment.Left;
            updateAlignment();
        }

        private void setAlignmentCenter(object pSender)
        {
            horizontalAlign = CCTextAlignment.Center;
            updateAlignment();
        }

        private void setAlignmentRight(object pSender)
        {
            horizontalAlign = CCTextAlignment.Right;
            updateAlignment();
        }

        private void setAlignmentTop(object pSender)
        {
            verticalAlign = CCVerticalTextAlignment.Top;
            updateAlignment();
        }

        private void setAlignmentMiddle(object pSender)
        {
            verticalAlign = CCVerticalTextAlignment.Center;
            updateAlignment();
        }

        private void setAlignmentBottom(object pSender)
        {
            verticalAlign = CCVerticalTextAlignment.Bottom;
            updateAlignment();
        }

        private string getCurrentAlignment()
        {
			var vertical = "";
			var horizontal = "";
            switch (verticalAlign)
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
            switch (horizontalAlign)
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
            return "Select the buttons on the sides to change the alignment.";
        }
    }
}