using System;

namespace CocosSharp
{
    /** @class ControlPotentiometer Potentiometer control for Cocos2D. */

    public class CCControlPotentiometer : CCControl
    {
        float maximumValue;
		float minimumValue;
		float value;
        CCPoint previousLocation;


		#region Properties

		protected CCPoint PreviousLocation { get; set; }
		protected CCSprite ThumbSprite { get; set; }
		protected CCProgressTimer ProgressTimer { get; set; }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
				if (ThumbSprite != null)
                {
					ThumbSprite.Opacity = value ? (byte) 255 : (byte) 128;
                }
            }
        }

        public float Value
        {
            get { return this.value; }
            set
            {
                if (value < minimumValue)
                {
                    value = minimumValue;
                }
                if (value > maximumValue)
                {
                    value = maximumValue;
                }
                this.value = value;

                // Update thumb and progress position for new value
                float percent = (value - minimumValue) / (maximumValue - minimumValue);
                ProgressTimer.Percentage = percent * 100.0f;
                ThumbSprite.Rotation = percent * 360.0f;

                SendActionsForControlEvents(CCControlEvent.ValueChanged);
            }
        }

        public float MinimumValue
        {
            get { return minimumValue; }
            set
            {
                minimumValue = value;

                if (minimumValue >= maximumValue)
                {
                    maximumValue = minimumValue + 1.0f;
                }

                Value = maximumValue;
            }
        }

        public float MaximumValue
        {
            get { return maximumValue; }
            set
            {
                maximumValue = value;

                if (maximumValue <= minimumValue)
                {
                    minimumValue = maximumValue - 1.0f;
                }

                Value = minimumValue;
            }
        }

		#endregion Properties


        #region Constructors

        public CCControlPotentiometer(string backgroundFile, string progressFile, string thumbFile)
        {
			var trackSprite = new CCSprite(backgroundFile);
            var thumbSprite = new CCSprite(thumbFile);
            var progressTimer = new CCProgressTimer(new CCSprite(progressFile));

			minimumValue = 0.0f;
			maximumValue = 1.0f;
			value = minimumValue;

			ProgressTimer = progressTimer;
			ThumbSprite = thumbSprite;
			thumbSprite.Position = progressTimer.Position;

			AddChild(thumbSprite, 2);
			AddChild(progressTimer, 1);
			AddChild(trackSprite);

			ContentSize = trackSprite.ContentSize;

			// Register Touch Event
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = OnTouchBegan;
			touchListener.OnTouchMoved = OnTouchMoved;
			touchListener.OnTouchEnded = OnTouchEnded;

            EventDispatcher.AddEventListener(touchListener, this);
        }

        #endregion Constructors


		#region Event handling

        public override bool IsTouchInside(CCTouch touch)
        {
            CCPoint touchLocation = GetTouchLocation(touch);

            float distance = DistanceBetweenPointAndPoint(ProgressTimer.Position, touchLocation);

            return distance < Math.Min(ContentSize.Width / 2, ContentSize.Height / 2);
        }

		bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            if (!IsTouchInside(touch) || !Enabled || !Visible)
            {
                return false;
            }

            previousLocation = GetTouchLocation(touch);

            PotentiometerBegan(previousLocation);

            return true;
        }

		void OnTouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            CCPoint location = GetTouchLocation(touch);

            PotentiometerMoved(location);
        }

		void OnTouchEnded(CCTouch touch, CCEvent touchEvent)
        {
            PotentiometerEnded(CCPoint.Zero);
        }

        protected void PotentiometerBegan(CCPoint location)
        {
            Selected = true;
			ThumbSprite.Color = CCColor3B.DarkGray;
        }

        protected void PotentiometerMoved(CCPoint location)
        {
            float angle = AngleInDegreesBetweenLineFromPoint_toPoint_toLineFromPoint_toPoint(
                ProgressTimer.Position,
                location,
                ProgressTimer.Position,
                previousLocation);

            // fix value, if the 12 o'clock position is between location and previousLocation
            if (angle > 180)
            {
                angle -= 360;
            }
            else if (angle < -180)
            {
                angle += 360;
            }

            Value = this.value + angle / 360.0f * (maximumValue - minimumValue);

            previousLocation = location;
        }

        protected void PotentiometerEnded(CCPoint location)
        {
			ThumbSprite.Color = CCColor3B.White;
            Selected = false;
        }

		#endregion Event handling


        protected float DistanceBetweenPointAndPoint(CCPoint point1, CCPoint point2)
        {
            float dx = point1.X - point2.X;
            float dy = point1.Y - point2.Y;
            return (float) Math.Sqrt(dx * dx + dy * dy);
        }

        protected float AngleInDegreesBetweenLineFromPoint_toPoint_toLineFromPoint_toPoint(
            CCPoint beginLineA,
            CCPoint endLineA,
            CCPoint beginLineB,
            CCPoint endLineB)
        {
            float a = endLineA.X - beginLineA.X;
            float b = endLineA.Y - beginLineA.Y;
            float c = endLineB.X - beginLineB.X;
            float d = endLineB.Y - beginLineB.Y;

            var atanA = (float) Math.Atan2(a, b);
            var atanB = (float) Math.Atan2(c, d);

            // convert radiants to degrees
            return (atanA - atanB) * 180f / (float) Math.PI;
        }
    }
}