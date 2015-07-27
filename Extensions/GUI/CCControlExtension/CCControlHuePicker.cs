using System;

namespace CocosSharp
{
    public class CCControlHuePicker : CCControl
    {
        float hue;
        float huePercentage;


		#region Properties

		public CCPoint StartPos { get; set; }
		public CCSprite Background { get; set; }
		public CCSprite Slider { get; set; }

        public float Hue
        {
            get { return hue; }
            set
            {
                hue = value;

                float huePercentage = value / 360.0f;
                HuePercentage = huePercentage;
            }
        }

        public float HuePercentage
        {
            get { return huePercentage; }
            set
            {
                huePercentage = value;

                hue = huePercentage * 360.0f;

                // Clamp the position of the icon within the circle
                CCRect BackgroundBox = Background.BoundingBox;

                // Get the center point of the Background image
                float centerX = StartPos.X + BackgroundBox.Size.Width * 0.5f;
                float centerY = StartPos.Y + BackgroundBox.Size.Height * 0.5f;

                // Work out the limit to the distance of the picker when moving around the hue bar
                float limit = BackgroundBox.Size.Width * 0.5f - 15.0f;

                // Update angle
                float angleDeg = huePercentage * 360.0f - 180.0f;
                float angle = CCMacros.CCDegreesToRadians(angleDeg);

                // Set new position of the Slider
                float x = centerX + limit * (float) Math.Cos(angle);
                float y = centerY + limit * (float) Math.Sin(angle);
                Slider.Position = new CCPoint(x, y);
            }
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                if (Slider != null)
                {
                    Slider.Opacity = value ? (byte)255 : (byte)128;
                }
            }
        }

		#endregion Properties


        #region Constructors

        public CCControlHuePicker(CCNode target, CCPoint pos)
        {
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = OnTouchBegan;
			touchListener.OnTouchMoved = OnTouchMoved;

            AddEventListener(touchListener);

			Background = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("huePickerBackground.png", target, pos, CCPoint.Zero);
			Slider = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPicker.png", target, pos, new CCPoint(0.5f, 0.5f));

            Slider.Position = new CCPoint(pos.X, pos.Y + Background.BoundingBox.Size.Height * 0.5f);
            StartPos = pos;
        }

        #endregion Constructors


		#region Event handling

		bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
		{
			if (!Enabled || !Visible)
			{
				return false;
			}

			// Get the touch location
			CCPoint touchLocation = GetTouchLocation(touch);

			// Check the touch position on the Slider
			return CheckSliderPosition(touchLocation);
		}

		void OnTouchMoved(CCTouch pTouch, CCEvent touchEvent)
		{
			// Get the touch location
			CCPoint touchLocation = GetTouchLocation(pTouch);

			//small modification: this allows changing of the colour, even if the touch leaves the bounding area
			//     UpdateSliderPosition(touchLocation);
			//     sendActionsForControlEvents(ControlEventValueChanged);
			// Check the touch position on the Slider
			CheckSliderPosition(touchLocation);
		}

		#endregion Event handling


        protected void UpdateSliderPosition(CCPoint location)
        {
            // Clamp the position of the icon within the circle
            CCRect BackgroundBox = Background.BoundingBox;

            float centerX = StartPos.X + BackgroundBox.Size.Width * 0.5f;
            float centerY = StartPos.Y + BackgroundBox.Size.Height * 0.5f;

            float dx = location.X - centerX;
            float dy = location.Y - centerY;

            // Update angle by using the direction of the location
            var angle = (float) Math.Atan2(dy, dx);
            float angleDeg = CCMacros.CCRadiansToDegrees(angle) + 180.0f;

            // use the position / Slider width to determin the percentage the dragger is at
            Hue = angleDeg;

            OnValueChanged();
        }

        protected bool CheckSliderPosition(CCPoint location)
        {
            double distance = Math.Sqrt(Math.Pow(location.X + 10, 2) + Math.Pow(location.Y, 2));

            // check that the touch location is within the circle
            if (80 > distance && distance > 59)
            {
                UpdateSliderPosition(location);
                return true;
            }
            return false;
        }
    }
}