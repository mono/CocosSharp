using System;

namespace CocosSharp
{
    /** @class ControlPotentiometer Potentiometer control for Cocos2D. */

    public class CCControlPotentiometer : CCControl
    {
        protected float _maximumValue;
        protected float _minimumValue;
        private CCPoint _previousLocation;

        private CCProgressTimer _progressTimer;
        private CCSprite _thumbSprite;
        protected float _value;

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (_thumbSprite != null)
                {
                    _thumbSprite.Opacity = value ? (byte) 255 : (byte) 128;
                }
            }
        }

        protected CCSprite ThumbSprite
        {
            get { return _thumbSprite; }
            set { _thumbSprite = value; }
        }

        protected CCProgressTimer ProgressTimer
        {
            get { return _progressTimer; }
            set { _progressTimer = value; }
        }

        protected CCPoint PreviousLocation
        {
            get { return _previousLocation; }
            set { _previousLocation = value; }
        }

        public float Value
        {
            get { return _value; }
            set
            {
                if (value < _minimumValue)
                {
                    value = _minimumValue;
                }
                if (value > _maximumValue)
                {
                    value = _maximumValue;
                }
                _value = value;

                // Update thumb and progress position for new value
                float percent = (value - _minimumValue) / (_maximumValue - _minimumValue);
                _progressTimer.Percentage = percent * 100.0f;
                _thumbSprite.Rotation = percent * 360.0f;

                SendActionsForControlEvents(CCControlEvent.ValueChanged);
            }
        }

        public float MinimumValue
        {
            get { return _minimumValue; }
            set
            {
                _minimumValue = value;

                if (_minimumValue >= _maximumValue)
                {
                    _maximumValue = _minimumValue + 1.0f;
                }

                Value = _maximumValue;
            }
        }

        public float MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                _maximumValue = value;

                if (_maximumValue <= _minimumValue)
                {
                    _minimumValue = _maximumValue - 1.0f;
                }

                Value = _minimumValue;
            }
        }


        #region Constructors

        public CCControlPotentiometer()
        {
            _thumbSprite = null;
            _progressTimer = null;
            _value = 0.0f;
            _minimumValue = 0.0f;
            _maximumValue = 0.0f;
        }

        /*        * 
         * Creates potentiometer with a track filename and a progress filename.
         */

        public CCControlPotentiometer(string backgroundFile, string progressFile, string thumbFile)
        {
            // Prepare track for potentiometer
            var backgroundSprite = new CCSprite(backgroundFile);

            // Prepare thumb for potentiometer
            var thumbSprite = new CCSprite(thumbFile);

            // Prepare progress for potentiometer
            var progressTimer = new CCProgressTimer(new CCSprite(progressFile));
            //progressTimer.type              = kProgressTimerTypeRadialCW;
            InitCCControlPotentiometer(backgroundSprite, progressTimer, thumbSprite);
        }

        private void InitCCControlPotentiometer(CCSprite trackSprite, CCProgressTimer progressTimer, CCSprite thumbSprite)
        {
            TouchEnabled = true;

            ProgressTimer = progressTimer;
            ThumbSprite = thumbSprite;
            thumbSprite.Position = progressTimer.Position;

            AddChild(thumbSprite, 2);
            AddChild(progressTimer, 1);
            AddChild(trackSprite);

            ContentSize = trackSprite.ContentSize;

            // Init default values
            _minimumValue = 0.0f;
            _maximumValue = 1.0f;
            Value = _minimumValue;
        }

        #endregion Constructors


        public override bool IsTouchInside(CCTouch touch)
        {
            CCPoint touchLocation = GetTouchLocation(touch);

            float distance = DistanceBetweenPointAndPoint(_progressTimer.Position, touchLocation);

            return distance < Math.Min(ContentSize.Width / 2, ContentSize.Height / 2);
        }

        public override bool TouchBegan(CCTouch touch)
        {
            if (!IsTouchInside(touch) || !Enabled || !Visible)
            {
                return false;
            }

            _previousLocation = GetTouchLocation(touch);

            PotentiometerBegan(_previousLocation);

            return true;
        }

        public override void TouchMoved(CCTouch touch)
        {
            CCPoint location = GetTouchLocation(touch);

            PotentiometerMoved(location);
        }

        public override void TouchEnded(CCTouch touch)
        {
            PotentiometerEnded(CCPoint.Zero);
        }

        protected void PotentiometerBegan(CCPoint location)
        {
            Selected = true;
            ThumbSprite.Color = new CCColor3B(128, 128, 128); //TODO: CCColor3B.GRAY
        }

        protected void PotentiometerMoved(CCPoint location)
        {
            float angle = AngleInDegreesBetweenLineFromPoint_toPoint_toLineFromPoint_toPoint(
                _progressTimer.Position,
                location,
                _progressTimer.Position,
                _previousLocation);

            // fix value, if the 12 o'clock position is between location and previousLocation
            if (angle > 180)
            {
                angle -= 360;
            }
            else if (angle < -180)
            {
                angle += 360;
            }

            Value = _value + angle / 360.0f * (_maximumValue - _minimumValue);

            _previousLocation = location;
        }

        protected void PotentiometerEnded(CCPoint location)
        {
            ThumbSprite.Color = new CCColor3B(255, 255, 255); //TODO: CCColor3B.WHITE
            Selected = false;
        }

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