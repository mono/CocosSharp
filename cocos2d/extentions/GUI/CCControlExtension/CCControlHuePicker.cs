using System;

namespace Cocos2D
{
    public class CCControlHuePicker : CCControl
    {
        //maunally put in the setters
        private CCSprite _background;
        private float _hue;
        private float _huePercentage;
        private CCSprite _slider;
        private CCPoint _startPos;

        public CCControlHuePicker()
        {
        }

        public CCControlHuePicker(CCNode target, CCPoint pos)
        {
            InitWithTargetAndPos(target, pos);
        }

        public float Hue
        {
            get { return _hue; }
            set
            {
                _hue = value;

                float huePercentage = value / 360.0f;
                HuePercentage = huePercentage;
            }
        }

        public float HuePercentage
        {
            get { return _huePercentage; }
            set
            {
                _huePercentage = value;

                _hue = _huePercentage * 360.0f;

                // Clamp the position of the icon within the circle
                CCRect backgroundBox = _background.BoundingBox;

                // Get the center point of the background image
                float centerX = _startPos.X + backgroundBox.Size.Width * 0.5f;
                float centerY = _startPos.Y + backgroundBox.Size.Height * 0.5f;

                // Work out the limit to the distance of the picker when moving around the hue bar
                float limit = backgroundBox.Size.Width * 0.5f - 15.0f;

                // Update angle
                float angleDeg = _huePercentage * 360.0f - 180.0f;
                float angle = CCMacros.CCDegreesToRadians(angleDeg);

                // Set new position of the slider
                float x = centerX + limit * (float) Math.Cos(angle);
                float y = centerY + limit * (float) Math.Sin(angle);
                _slider.Position = new CCPoint(x, y);
            }
        }

        public CCSprite Background
        {
            get { return _background; }
            set { _background = value; }
        }

        public CCSprite Slider
        {
            get { return _slider; }
            set { _slider = value; }
        }

        public CCPoint StartPos
        {
            get { return _startPos; }
            set { _startPos = value; }
        }

        public virtual bool InitWithTargetAndPos(CCNode target, CCPoint pos)
        {
            if (base.Init())
            {
                TouchEnabled = true;
                // Add background and slider sprites
                Background = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("huePickerBackground.png", target,
                                                                              pos, CCPoint.Zero);
                Slider = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPicker.png", target, pos,
                                                                          new CCPoint(0.5f, 0.5f));

                _slider.Position = new CCPoint(pos.X, pos.Y + _background.BoundingBox.Size.Height * 0.5f);
                _startPos = pos;

                // Sets the default value
                _hue = 0.0f;
                _huePercentage = 0.0f;
                return true;
            }
            return false;
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
                if (_slider != null)
                {
                    _slider.Opacity = value ? (byte)255 : (byte)128;
                }
            }
        }

        protected void UpdateSliderPosition(CCPoint location)
        {
            // Clamp the position of the icon within the circle
            CCRect backgroundBox = _background.BoundingBox;

            // Get the center point of the background image
            float centerX = _startPos.X + backgroundBox.Size.Width * 0.5f;
            float centerY = _startPos.Y + backgroundBox.Size.Height * 0.5f;

            // Work out the distance difference between the location and center
            float dx = location.X - centerX;
            float dy = location.Y - centerY;

            // Update angle by using the direction of the location
            var angle = (float) Math.Atan2(dy, dx);
            float angleDeg = CCMacros.CCRadiansToDegrees(angle) + 180.0f;

            // use the position / slider width to determin the percentage the dragger is at
            Hue = angleDeg;

            // send Control callback
            SendActionsForControlEvents(CCControlEvent.ValueChanged);
        }

        protected bool CheckSliderPosition(CCPoint location)
        {
            // compute the distance between the current location and the center
            double distance = Math.Sqrt(Math.Pow(location.X + 10, 2) + Math.Pow(location.Y, 2));

            // check that the touch location is within the circle
            if (80 > distance && distance > 59)
            {
                UpdateSliderPosition(location);
                return true;
            }
            return false;
        }

        public override bool TouchBegan(CCTouch touch)
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

        public override void TouchMoved(CCTouch pTouch)
        {
            // Get the touch location
            CCPoint touchLocation = GetTouchLocation(pTouch);

            //small modification: this allows changing of the colour, even if the touch leaves the bounding area
            //     UpdateSliderPosition(touchLocation);
            //     sendActionsForControlEvents(ControlEventValueChanged);
            // Check the touch position on the slider
            CheckSliderPosition(touchLocation);
        }
    }
}