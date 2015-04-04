using System;
using System.Collections.Generic;
using CocosSharp;

namespace tests
{
    public class LabelFNTMultiLineAlignment : AtlasDemoNew
    {
        public const string LongSentencesExample =
            "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

        public const string LineBreaksExample = "Lorem ipsum dolor\nsit amet\nconsectetur adipisicing elit\nblah\nblah";
        public const string MixedExample = "ABC\nLorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt\nDEF";

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

		private CCSprite arrowsBar;
		private CCSprite arrows;
		private CCLabel label;
		private bool drag;
        private CCMenuItemFont lastAlignmentItem;
		private CCMenuItemFont lastSentenceItem;

        public LabelFNTMultiLineAlignment()
        {
			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();

			touchListener.OnTouchesBegan = onTouchesBegan;
			touchListener.OnTouchesMoved = onTouchesMoved;
			touchListener.OnTouchesEnded = onTouchesEnded;

			AddEventListener(touchListener);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            // ask director the the window size
            var size = VisibleBoundsWorldspace.Size;

            // create and initialize a Label
            label = new CCLabel(LongSentencesExample, "fonts/markerFelt.fnt", new CCSize(size.Width / 3f, 0),
				CCTextAlignment.Center);
            //label.LineBreakWithoutSpace = true;
            arrowsBar = new CCSprite("Images/arrowsBar");
			arrows = new CCSprite("Images/arrows");

			CCMenuItemFont.FontSize = 20;
			CCMenuItemFont.FontName = "arial";
			var longSentences = new CCMenuItemFont("Long Flowing Sentences", stringChanged);
			var lineBreaks = new CCMenuItemFont("Short Sentences With Intentional Line Breaks", stringChanged);
			var mixed = new CCMenuItemFont("Long Sentences Mixed With Intentional Line Breaks", stringChanged);
			var stringMenu = new CCMenu(longSentences, lineBreaks, mixed);
			stringMenu.AlignItemsVertically();

			longSentences.Color = CCColor3B.Red;
			lastSentenceItem = longSentences;
			longSentences.Tag = LongSentences;
			lineBreaks.Tag = LineBreaks;
			mixed.Tag = Mixed;

			CCMenuItemFont.FontSize = 30;

			var left = new CCMenuItemFont("Left", alignmentChanged);
			var center = new CCMenuItemFont("Center", alignmentChanged);
			var right = new CCMenuItemFont("Right", alignmentChanged);

			var alignmentMenu = new CCMenu(left, center, right);
			alignmentMenu.AlignItemsHorizontally(alignmentItemPadding);

			center.Color = CCColor3B.Red;
            lastAlignmentItem = center;
			left.Tag = LeftAlign;
			center.Tag = CenterAlign;
			right.Tag = RightAlign;

			// position the label on the center of the screen
			label.Position = size.Center;

			arrowsBar.Visible = false;

			float arrowsWidth = (ArrowsMax - ArrowsMin) * size.Width;
			arrowsBar.ScaleX = (arrowsWidth / arrowsBar.ContentSize.Width);
			arrowsBar.Position = new CCPoint(((ArrowsMax + ArrowsMin) / 2) * size.Width, label.Position.Y);

			snapArrowsToEdge();

			stringMenu.Position = new CCPoint(size.Width / 2, size.Height - menuItemPaddingCenter);
			alignmentMenu.Position = new CCPoint(size.Width / 2, menuItemPaddingCenter + 15);

			AddChild(label);
			AddChild(arrowsBar);
			AddChild(arrows);
			AddChild(stringMenu);
			AddChild(alignmentMenu);
		}


        private void stringChanged(object sender)
        {
            var item = (CCMenuItemFont) sender;
            item.Color = CCColor3B.Red;
            lastAlignmentItem.Color = CCColor3B.White;
            lastAlignmentItem = item;

            switch (item.Tag)
            {
                case LongSentences:
                    label.Text = LongSentencesExample;
                    break;
                case LineBreaks:
                    label.Text = LineBreaksExample;
                    break;
                case Mixed:
                    label.Text = MixedExample;
                    break;

                default:
                    break;
            }

            snapArrowsToEdge();
        }

        private void alignmentChanged(object sender)
        {
            var item = (CCMenuItemFont) sender;
            item.Color = CCColor3B.Red;
            lastAlignmentItem.Color = CCColor3B.White;
            lastAlignmentItem = item;

            switch (item.Tag)
            {
                case LeftAlign:
                    label.HorizontalAlignment = CCTextAlignment.Left;
                    break;
                case CenterAlign:
                    label.HorizontalAlignment = CCTextAlignment.Center;
                    break;
                case RightAlign:
                    label.HorizontalAlignment = CCTextAlignment.Right;
                    break;

                default:
                    break;
            }

            snapArrowsToEdge();
        }

		void onTouchesBegan(List<CCTouch> pTouches, CCEvent touchEvent)
        {
            CCTouch touch = pTouches[0];
            CCPoint location = touch.Location;

            if (arrows.BoundingBox.ContainsPoint(location))
            {
                drag = true;
                arrowsBar.Visible = true;
            }
        }

		void onTouchesEnded(List<CCTouch> pTouches, CCEvent touchEvent)
        {
            drag = false;
            snapArrowsToEdge();

            arrowsBar.Visible = false;
        }

		void onTouchesMoved(List<CCTouch> pTouches, CCEvent touchEvent)
        {
            if (!drag)
            {
                return;
            }

            CCTouch touch = pTouches[0];
            CCPoint location = touch.Location;

            CCSize winSize = VisibleBoundsWorldspace.Size;

            arrows.Position = new CCPoint(Math.Max(Math.Min(location.X, ArrowsMax * winSize.Width), ArrowsMin * winSize.Width),
                                                         arrows.Position.Y);

            float labelWidth = Math.Abs(arrows.Position.X - label.Position.X);

            label.Dimensions = new CCSize(labelWidth, 0);
        }

        private void snapArrowsToEdge()
        {
            arrows.PositionX = label.Position.X + label.ContentSize.Width;
            arrows.PositionY = label.Position.Y;
        }

        public override string Title
        {
            get { return string.Empty; }
        }
    }
}