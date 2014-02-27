using System.Diagnostics;

namespace CocosSharp
{
    public enum CCControlStepperPart
    {
        PartMinus = 0,
        PartPlus,
        PartNone
    }

    public class CCControlStepper : CCControl
    {
        private const string ControlStepperLabelFont = "Arial";
        private const float kAutorepeatDeltaTime = 0.15f;
        private const int kAutorepeatIncreaseTimeIncrement = 12;
        private static readonly CCColor3B ControlStepperLabelColorDisabled = new CCColor3B(147, 147, 147);
        private static readonly CCColor3B ControlStepperLabelColorEnabled = new CCColor3B(55, 55, 55);
        protected bool _autorepeat;
        protected int _autorepeatCount;
        protected bool _continuous;
        protected float _maximumValue;
        protected float _minimumValue;
        protected CCLabelTtf _minusLabel;
        protected CCSprite _minusSprite;
        protected CCLabelTtf _plusLabel;
        protected CCSprite _plusSprite;
        protected float _stepValue;
        protected bool _touchInsideFlag;
        protected CCControlStepperPart _touchedPart;
        protected float _value;
        protected bool _wraps;


        public CCSprite MinusSprite
        {
            get { return _minusSprite; }
            set { _minusSprite = value; }
        }

        public CCSprite PlusSprite
        {
            get { return _plusSprite; }
            set { _plusSprite = value; }
        }

        public CCLabelTtf MinusLabel
        {
            get { return _minusLabel; }
            set { _minusLabel = value; }
        }

        public CCLabelTtf PlusLabel
        {
            get { return _plusLabel; }
            set { _plusLabel = value; }
        }


        public virtual bool Wraps
        {
            get { return _wraps; }
            set
            {
                _wraps = value;

                if (_wraps)
                {
                    _minusLabel.Color = ControlStepperLabelColorEnabled;
                    _plusLabel.Color = ControlStepperLabelColorEnabled;
                }

                Value = _value;
            }
        }

        public virtual float MinimumValue
        {
            get { return _minimumValue; }
            set
            {
                if (value >= _maximumValue)
                {
                    Debug.Assert(value < _maximumValue, "Must be numerically less than maximumValue.");
                }

                _minimumValue = value;
                Value = _value;
            }
        }

        public virtual float MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                if (value <= _minimumValue)
                {
                    Debug.Assert(value > _minimumValue, "Must be numerically greater than minimumValue.");
                }

                _maximumValue = value;
                Value = _value;
            }
        }

        /** The numeric value of the stepper. */

        public virtual float Value
        {
            get { return _value; }
            set { SetValueWithSendingEvent(value, true); }
        }

        public virtual float StepValue
        {
            get { return _stepValue; }
            set
            {
                if (value <= 0)
                {
                    Debug.Assert(value > 0, "Must be numerically greater than 0.");
                }

                _stepValue = value;
            }
        }

        public virtual bool IsContinuous
        {
            get { return _continuous; }
        }


        #region Constructors

        public CCControlStepper()
        {
        }

        public CCControlStepper(CCSprite minusSprite, CCSprite plusSprite)
        {
            InitCCControlStepper(minusSprite, plusSprite);
        }

        private void InitCCControlStepper(CCSprite minusSprite, CCSprite plusSprite)
        {
            Debug.Assert(minusSprite != null, "Minus sprite must be not nil");
            Debug.Assert(plusSprite != null, "Plus sprite must be not nil");

            TouchEnabled = true;

            // Set the default values
            _autorepeat = true;
            _continuous = true;
            _minimumValue = 0;
            _maximumValue = 100;
            _value = 0;
            _stepValue = 1;
            _wraps = false;
            IgnoreAnchorPointForPosition = false;

            // Add the minus components
            MinusSprite = minusSprite;
            _minusSprite.Position = new CCPoint(minusSprite.ContentSize.Width / 2,
                                                minusSprite.ContentSize.Height / 2);
            AddChild(_minusSprite);

            MinusLabel = new CCLabelTtf("-", ControlStepperLabelFont, 38);
            _minusLabel.Color = ControlStepperLabelColorDisabled;
            _minusLabel.Position = new CCPoint(_minusSprite.ContentSize.Width / 2,
                                               _minusSprite.ContentSize.Height / 2);
            _minusSprite.AddChild(_minusLabel);

            // Add the plus components 
            PlusSprite = plusSprite;
            _plusSprite.Position =
                new CCPoint(minusSprite.ContentSize.Width + plusSprite.ContentSize.Width / 2,
                            minusSprite.ContentSize.Height / 2);
            AddChild(_plusSprite);

            PlusLabel = new CCLabelTtf("+", ControlStepperLabelFont, 38);
            _plusLabel.Color = ControlStepperLabelColorEnabled;
            _plusLabel.Position = _plusSprite.ContentSize.Center;
            _plusSprite.AddChild(_plusLabel);

            // Defines the content size
            CCRect maxRect = CCControlUtils.CCRectUnion(_minusSprite.BoundingBox, _plusSprite.BoundingBox);
            ContentSize = new CCSize(_minusSprite.ContentSize.Width + _plusSprite.ContentSize.Height, maxRect.Size.Height);
        }

        #endregion Constructors


        public virtual void SetValueWithSendingEvent(float value, bool send)
        {
            if (value < _minimumValue)
            {
                value = _wraps ? _maximumValue : _minimumValue;
            }
            else if (value > _maximumValue)
            {
                value = _wraps ? _minimumValue : _maximumValue;
            }

            _value = value;

            if (!_wraps)
            {
                _minusLabel.Color = (value == _minimumValue)
                                        ? ControlStepperLabelColorDisabled
                                        : ControlStepperLabelColorEnabled;
                _plusLabel.Color = (value == _maximumValue)
                                       ? ControlStepperLabelColorDisabled
                                       : ControlStepperLabelColorEnabled;
            }

            if (send)
            {
                SendActionsForControlEvents(CCControlEvent.ValueChanged);
            }
        }

        public override void Update(float dt)
        {
            _autorepeatCount++;

            if ((_autorepeatCount < kAutorepeatIncreaseTimeIncrement) && (_autorepeatCount % 3) != 0)
                return;

            if (_touchedPart == CCControlStepperPart.PartMinus)
            {
                SetValueWithSendingEvent(_value - _stepValue, _continuous);
            }
            else if (_touchedPart == CCControlStepperPart.PartPlus)
            {
                SetValueWithSendingEvent(_value + _stepValue, _continuous);
            }
        }

        //events
        public override bool TouchBegan(CCTouch pTouch)
        {
            if (!IsTouchInside(pTouch) || !Enabled || !Visible)
            {
                return false;
            }

            CCPoint location = GetTouchLocation(pTouch);
            UpdateLayoutUsingTouchLocation(location);

            _touchInsideFlag = true;

            if (_autorepeat)
            {
                StartAutorepeat();
            }

            return true;
        }

        public override void TouchMoved(CCTouch pTouch)
        {
            if (IsTouchInside(pTouch))
            {
                CCPoint location = GetTouchLocation(pTouch);
                UpdateLayoutUsingTouchLocation(location);

                if (!_touchInsideFlag)
                {
                    _touchInsideFlag = true;

                    if (_autorepeat)
                    {
                        StartAutorepeat();
                    }
                }
            }
            else
            {
                _touchInsideFlag = false;

                _touchedPart = CCControlStepperPart.PartNone;

                _minusSprite.Color = CCTypes.CCWhite;
                _plusSprite.Color = CCTypes.CCWhite;

                if (_autorepeat)
                {
                    StopAutorepeat();
                }
            }
        }

        public override void TouchEnded(CCTouch pTouch)
        {
            _minusSprite.Color = CCTypes.CCWhite;
            _plusSprite.Color = CCTypes.CCWhite;

            if (_autorepeat)
            {
                StopAutorepeat();
            }

            if (IsTouchInside(pTouch))
            {
                CCPoint location = GetTouchLocation(pTouch);

                Value = _value +
                        ((location.X < _minusSprite.ContentSize.Width) ? (0.0f - _stepValue) : _stepValue);
            }
        }

        // Weak links to children

        /** Update the layout of the stepper with the given touch location. */

        protected void UpdateLayoutUsingTouchLocation(CCPoint location)
        {
            if (location.X < _minusSprite.ContentSize.Width
                && _value > _minimumValue)
            {
                _touchedPart = CCControlStepperPart.PartMinus;

                _minusSprite.Color = CCTypes.CCGray;
                _plusSprite.Color = CCTypes.CCWhite;
            }
            else if (location.X >= _minusSprite.ContentSize.Width
                     && _value < _maximumValue)
            {
                _touchedPart = CCControlStepperPart.PartPlus;

                _minusSprite.Color = CCTypes.CCWhite;
                _plusSprite.Color = CCTypes.CCGray;
            }
            else
            {
                _touchedPart = CCControlStepperPart.PartNone;

                _minusSprite.Color = CCTypes.CCWhite;
                _plusSprite.Color = CCTypes.CCWhite;
            }
        }

        /** Start the autorepeat increment/decrement. */

        protected void StartAutorepeat()
        {
            _autorepeatCount = -1;

            Schedule(Update, kAutorepeatDeltaTime, CCScheduler.kCCRepeatForever, kAutorepeatDeltaTime * 3);
        }

        /** Stop the autorepeat. */

        protected void StopAutorepeat()
        {
            Unschedule(Update);
        }
    }
}