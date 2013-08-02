using System;

namespace Cocos2D
{
    public class CCControlSaturationBrightnessPicker : CCControl
    {
        /** Contains the receiver's current saturation value. */

        //not sure if these need to be there actually. I suppose someone might want to access the sprite?
        private CCSprite _background;
        private float _brightness;
        private CCSprite _overlay;
        private float _saturation;
        private CCSprite _shadow;
        private CCSprite _slider;
        private CCPoint _startPos;
        protected int boxPos;
        protected int boxSize;

        public CCControlSaturationBrightnessPicker()
        {
        }

        public CCControlSaturationBrightnessPicker(CCNode target, CCPoint pos)
        {
            InitWithTargetAndPos(target, pos);
        }

        public float Saturation
        {
            get { return _saturation; }
            set { _saturation = value; }
        }

        public float Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }

        public CCSprite Background
        {
            get { return _background; }
            set { _background = value; }
        }

        public CCSprite Overlay
        {
            get { return _overlay; }
            set { _overlay = value; }
        }

        public CCSprite Shadow
        {
            get { return _shadow; }
            set { _shadow = value; }
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

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (_slider != null)
                {
                    _slider.Opacity = value ? (byte) 255 : (byte) 128;
                }
            }
        }

        public virtual bool InitWithTargetAndPos(CCNode target, CCPoint pos)
        {
            if (base.Init())
            {
                TouchEnabled = true;
                // Add background and slider sprites
                _background = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPickerBackground.png", target, pos,
                                                                               CCPoint.Zero);
                _overlay = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPickerOverlay.png", target, pos,
                                                                            CCPoint.Zero);
                _shadow = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPickerShadow.png", target, pos,
                                                                           CCPoint.Zero);
                _slider = CCControlUtils.AddSpriteToTargetWithPosAndAnchor("colourPicker.png", target, pos,
                                                                           new CCPoint(0.5f, 0.5f));

                _startPos = pos; // starting position of the colour picker        
                boxPos = 35; // starting position of the virtual box area for picking a colour
                boxSize = (int) _background.ContentSize.Width / 2;
                // the size (width and height) of the virtual box for picking a colour from
                return true;
            }

            return false;
        }

        public virtual void UpdateWithHSV(HSV hsv)
        {
            HSV hsvTemp;
            hsvTemp.s = 1;
            hsvTemp.h = hsv.h;
            hsvTemp.v = 1;

            RGBA rgb = CCControlUtils.RGBfromHSV(hsvTemp);
            _background.Color = new CCColor3B((byte) (rgb.r * 255.0f), (byte) (rgb.g * 255.0f),
                                              (byte) (rgb.b * 255.0f));
        }

        public virtual void UpdateDraggerWithHSV(HSV hsv)
        {
            // Set the position of the slider to the correct saturation and brightness
            var pos = new CCPoint(_startPos.X + boxPos + (boxSize * (1f - hsv.s)),
                                  _startPos.Y + boxPos + (boxSize * hsv.v));

            // update
            UpdateSliderPosition(pos);
        }

        protected void UpdateSliderPosition(CCPoint sliderPosition)
        {
            // Clamp the position of the icon within the circle

            // Get the center point of the bkgd image
            float centerX = _startPos.X + _background.BoundingBox.Size.Width * 0.5f;
            float centerY = _startPos.Y + _background.BoundingBox.Size.Height * 0.5f;

            // Work out the distance difference between the location and center
            float dx = sliderPosition.X - centerX;
            float dy = sliderPosition.Y - centerY;
            var dist = (float) Math.Sqrt(dx * dx + dy * dy);

            // Update angle by using the direction of the location
            var angle = (float) Math.Atan2(dy, dx);

            // Set the limit to the slider movement within the colour picker
            float limit = _background.BoundingBox.Size.Width * 0.5f;

            // Check distance doesn't exceed the bounds of the circle
            if (dist > limit)
            {
                sliderPosition.X = centerX + limit * (float) Math.Cos(angle);
                sliderPosition.Y = centerY + limit * (float) Math.Sin(angle);
            }

            // Set the position of the dragger
            _slider.Position = sliderPosition;


            // Clamp the position within the virtual box for colour selection
            if (sliderPosition.X < _startPos.X + boxPos) sliderPosition.X = _startPos.X + boxPos;
            else if (sliderPosition.X > _startPos.X + boxPos + boxSize - 1)
                sliderPosition.X = _startPos.X + boxPos + boxSize - 1;
            if (sliderPosition.Y < _startPos.Y + boxPos) sliderPosition.Y = _startPos.Y + boxPos;
            else if (sliderPosition.Y > _startPos.Y + boxPos + boxSize)
                sliderPosition.Y = _startPos.Y + boxPos + boxSize;

            // Use the position / slider width to determin the percentage the dragger is at
            _saturation = 1.0f - Math.Abs((_startPos.X + boxPos - sliderPosition.X) / boxSize);
            _brightness = Math.Abs((_startPos.Y + boxPos - sliderPosition.Y) / boxSize);
        }

        protected bool CheckSliderPosition(CCPoint location)
        {
            // Clamp the position of the icon within the circle

            // get the center point of the bkgd image
            float centerX = _startPos.X + _background.BoundingBox.Size.Width * 0.5f;
            float centerY = _startPos.Y + _background.BoundingBox.Size.Height * 0.5f;

            // work out the distance difference between the location and center
            float dx = location.X - centerX;
            float dy = location.Y - centerY;
            var dist = (float) Math.Sqrt(dx * dx + dy * dy);

            // check that the touch location is within the bounding rectangle before sending updates
            if (dist <= _background.BoundingBox.Size.Width * 0.5f)
            {
                UpdateSliderPosition(location);
                SendActionsForControlEvents(CCControlEvent.ValueChanged);
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

        public override void TouchMoved(CCTouch touch)
        {
            // Get the touch location
            CCPoint touchLocation = GetTouchLocation(touch);

            //small modification: this allows changing of the colour, even if the touch leaves the bounding area
            //     updateSliderPosition(touchLocation);
            //     sendActionsForControlEvents(ControlEventValueChanged);
            // Check the touch position on the slider
            CheckSliderPosition(touchLocation);
        }
    }
}