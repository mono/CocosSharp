using System;
using System.Collections.Generic;
using CocosSharp;

namespace tests
{
    public class LabelFNTLineHeightTest : AtlasDemoNew
    {

        private const float ArrowsMax = 0.95f;
        private const float ArrowsMin = 0.7f;

		private CCSprite arrowsBar;
		private CCSprite arrows;
		private CCLabel label;
		private bool drag;

        private float arrowsWidth;
        private float arrowsWidthCenter;

        public LabelFNTLineHeightTest()
        {

            Color = new CCColor3B(200, 191, 231);
            Opacity = 255;
			// Register Touch Event
			var touchListener = new CCEventListenerTouchAllAtOnce();

			touchListener.OnTouchesBegan = onTouchesBegan;
			touchListener.OnTouchesMoved = onTouchesMoved;
			touchListener.OnTouchesEnded = onTouchesEnded;

			AddEventListener(touchListener);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            // ask director the the window size
            var size = VisibleBoundsWorldspace.Size;

            arrowsWidth = (ArrowsMax - ArrowsMin) * size.Width;
            arrowsWidthCenter = arrowsWidth * 0.5f;

            // create and initialize a Label
            label = new CCLabel("Test\nLine\nHeight", "fonts/arial-unicode-26.fnt");
            label.Color = CCColor3B.Red;

            arrowsBar = new CCSprite("Images/arrowsBar");
            arrows = new CCSprite("Images/arrows");
            arrows.Color = CCColor3B.Blue;

            // position the label on the center of the screen
            label.Position = size.Center;

            arrowsBar.Visible = false;

            arrowsBar.ScaleX = (arrowsWidth / arrowsBar.ContentSize.Width);

            arrowsBar.Position = new CCPoint(size.Width / 2.0f, size.Height * 0.15f + arrowsBar.ContentSize.Height * 2.0f);

            snapArrowsToEdge();

            AddChild(label);
            AddChild(arrowsBar);
            AddChild(arrows);

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
            UpdateLineHeight();
        }

		void onTouchesMoved(List<CCTouch> pTouches, CCEvent touchEvent)
        {
            if (!drag)
            {
                return;
            }

            var touch = pTouches[0];
            var location = touch.Location;

            var left = arrowsBar.PositionX - arrowsWidthCenter;
            var right = arrowsBar.PositionX + arrowsWidthCenter;

            arrows.PositionX = Math.Max(Math.Min(location.X, right), left);
            arrows.PositionY = arrowsBar.Position.Y;

            UpdateLineHeight();

        }

        private void SetPercentage (float percent)
        {
            var left = arrowsBar.PositionX - arrowsWidthCenter;
            percent /= 100f;
            left += arrowsWidth * percent;
            arrows.PositionX = Math.Max(Math.Min(left, arrowsBar.PositionX + arrowsWidth), arrowsBar.PositionX - arrowsWidth);
        }

        private void UpdateLineHeight ()
        {
            var left = arrowsBar.PositionX - arrowsWidthCenter;
            var newLineHeight = (arrows.PositionX - left) / arrowsWidth * 100;

            label.LineHeight = newLineHeight;
        }

        private void snapArrowsToEdge()
        {

            if (arrows.Position == CCPoint.Zero)
            {
                SetPercentage(label.LineHeight);
            }
            else
            {
                arrows.PositionX = Math.Max(Math.Min(arrows.PositionX, arrowsBar.PositionX + arrowsWidthCenter), arrowsBar.PositionX - arrowsWidthCenter);
            }
            arrows.PositionY = arrowsBar.PositionY;
        }

        public override string Title
        {
            get { return "New Label - .FNT file"; }
        }

        public override string Subtitle
        {
            get
            {
                return "Testing line height of label";
            }
        }
    }
}