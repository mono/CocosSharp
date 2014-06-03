using System;

namespace CocosSharp
{
    public class CCControlSaturationBrightnessPicker : CCControl
    {
        /** Contains the receiver's current saturation value. */

        //not sure if these need to be there actually. I suppose someone might want to access the sprite?

        protected int boxPos;
        protected int boxSize;


		#region Properties

		public float Saturation { get; set; }
		public float Brightness { get; set; }
		public CCPoint StartPos { get; set; }

		public CCSprite Background { get; set; }
		public CCSprite Overlay { get; set; }
		public CCSprite Shadow { get; set; }
		public CCSprite Slider { get; set; }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (Slider != null)
                {
                    Slider.Opacity = value ? (byte) 255 : (byte) 128;
                }
            }
        }

		#endregion Properties


        #region Constructors

        public CCControlSaturationBrightnessPicker(CCNode target, CCPoint pos)
        {
			Background = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPickerBackground.png", target, pos, CCPoint.Zero);
			Overlay = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPickerOverlay.png", target, pos, CCPoint.Zero);
			Shadow = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPickerShadow.png", target, pos, CCPoint.Zero);
			Slider = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPicker.png", target, pos, new CCPoint(0.5f, 0.5f));

			StartPos = pos;        
			boxPos = 35; 										// starting position of the virtual box area for picking a colour
			boxSize = (int) Background.ContentSize.Width / 2; 	// the size (width and height) of the virtual box for picking a colour from

			// Register Touch Event
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = OnTouchBegan;
			touchListener.OnTouchMoved = OnTouchMoved;

			AddEventListener(touchListener);
        }

        #endregion Constructors


        public virtual void UpdateWithHSV(HSV hsv)
        {
            HSV hsvTemp;
			hsvTemp.S = 1;
			hsvTemp.H = hsv.H;
			hsvTemp.V = 1;

            RGBA rgb = CCControlUtils.RGBfromHSV(hsvTemp);
			Background.Color = new CCColor3B((byte) (rgb.R * 255.0f), (byte) (rgb.G * 255.0f),  (byte) (rgb.B * 255.0f));
        }

        public virtual void UpdateDraggerWithHSV(HSV hsv)
        {
            // Set the position of the slider to the correct saturation and brightness
			var pos = new CCPoint(StartPos.X + boxPos + (boxSize * (1f - hsv.S)), StartPos.Y + boxPos + (boxSize * hsv.V));

            UpdateSliderPosition(pos);
        }

        protected void UpdateSliderPosition(CCPoint sliderPosition)
        {
            // Clamp the position of the icon within the circle

            float centerX = StartPos.X + Background.BoundingBox.Size.Width * 0.5f;
            float centerY = StartPos.Y + Background.BoundingBox.Size.Height * 0.5f;

            // Work out the distance difference between the location and center
            float dx = sliderPosition.X - centerX;
            float dy = sliderPosition.Y - centerY;
            var dist = (float) Math.Sqrt(dx * dx + dy * dy);

            // Update angle by using the direction of the location
            var angle = (float) Math.Atan2(dy, dx);

            // Set the limit to the slider movement within the colour picker
            float limit = Background.BoundingBox.Size.Width * 0.5f;

            if (dist > limit)
            {
                sliderPosition.X = centerX + limit * (float) Math.Cos(angle);
                sliderPosition.Y = centerY + limit * (float) Math.Sin(angle);
            }

            Slider.Position = sliderPosition;

            // Clamp the position within the virtual box for colour selection
            if (sliderPosition.X < StartPos.X + boxPos) sliderPosition.X = StartPos.X + boxPos;
            else if (sliderPosition.X > StartPos.X + boxPos + boxSize - 1)
                sliderPosition.X = StartPos.X + boxPos + boxSize - 1;
            if (sliderPosition.Y < StartPos.Y + boxPos) sliderPosition.Y = StartPos.Y + boxPos;
            else if (sliderPosition.Y > StartPos.Y + boxPos + boxSize)
                sliderPosition.Y = StartPos.Y + boxPos + boxSize;

            // Use the position / slider width to determin the percentage the dragger is at
            Saturation = 1.0f - Math.Abs((StartPos.X + boxPos - sliderPosition.X) / boxSize);
            Brightness = Math.Abs((StartPos.Y + boxPos - sliderPosition.Y) / boxSize);
        }

        protected bool CheckSliderPosition(CCPoint location)
        {
            // Clamp the position of the icon within the circle

            float centerX = StartPos.X + Background.BoundingBox.Size.Width * 0.5f;
            float centerY = StartPos.Y + Background.BoundingBox.Size.Height * 0.5f;

            float dx = location.X - centerX;
            float dy = location.Y - centerY;
            var dist = (float) Math.Sqrt(dx * dx + dy * dy);

            // check that the touch location is within the bounding rectangle before sending updates
            if (dist <= Background.BoundingBox.Size.Width * 0.5f)
            {
                UpdateSliderPosition(location);
                SendActionsForControlEvents(CCControlEvent.ValueChanged);
                return true;
            }
            return false;
        }

		#region Event handling

		bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            if (!Enabled || !Visible)
            {
                return false;
            }

            // Get the touch location
            CCPoint touchLocation = GetTouchLocation(touch);

            // Check the touch position on the slider
            return CheckSliderPosition(touchLocation);
        }

		void OnTouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            // Get the touch location
            CCPoint touchLocation = GetTouchLocation(touch);

            //small modification: this allows changing of the colour, even if the touch leaves the bounding area
            //     updateSliderPosition(touchLocation);
            //     sendActionsForControlEvents(ControlEventValueChanged);
            // Check the touch position on the slider
            CheckSliderPosition(touchLocation);
        }

		#endregion Event handling
    }
}