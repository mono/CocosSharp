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
        const string ControlStepperLabelFont = "Arial";
        const float AutorepeatDeltaTime = 0.15f;
        const int AutorepeatIncreaseTimeIncrement = 12;

        static readonly CCColor3B ControlStepperLabelColorDisabled = new CCColor3B(147, 147, 147);
        static readonly CCColor3B ControlStepperLabelColorEnabled = new CCColor3B(55, 55, 55);

		bool wraps;
        bool autorepeat;
        int autorepeatCount;

		float value;
		float maximumValue;
		float minimumValue;
        float stepValue;

        bool touchInsideFlag;
        CCControlStepperPart touchedPart;

  
		#region Properties

		public CCSprite MinusSprite { get; set; }
		public CCSprite PlusSprite { get; set; }
		public CCLabelTtf MinusLabel { get; set; }
		public CCLabelTtf PlusLabel { get; set; }
		public virtual bool IsContinuous { get; private set; }


        public virtual bool Wraps
        {
            get { return wraps; }
            set
            {
                wraps = value;

                if (wraps)
                {
                    MinusLabel.Color = ControlStepperLabelColorEnabled;
                    PlusLabel.Color = ControlStepperLabelColorEnabled;
                }

                Value = this.value;
            }
        }

        public virtual float MinimumValue
        {
            get { return minimumValue; }
            set
            {
                if (value >= maximumValue)
                {
                    Debug.Assert(value < maximumValue, "Must be numerically less than maximumValue.");
                }

                minimumValue = value;
                Value = this.value;
            }
        }

        public virtual float MaximumValue
        {
            get { return maximumValue; }
            set
            {
                if (value <= minimumValue)
                {
                    Debug.Assert(value > minimumValue, "Must be numerically greater than minimumValue.");
                }

                maximumValue = value;
                Value = this.value;
            }
        }

        public virtual float Value
        {
            get { return this.value; }
            set { SetValueWithSendingEvent(value, true); }
        }

        public virtual float StepValue
        {
            get { return stepValue; }
            set
            {
                if (value <= 0)
                {
                    Debug.Assert(value > 0, "Must be numerically greater than 0.");
                }

                stepValue = value;
            }
        }

		#endregion Properties


        #region Constructors

        public CCControlStepper(CCSprite minusSprite, CCSprite plusSprite)
        {
            Debug.Assert(minusSprite != null, "Minus sprite must be not nil");
            Debug.Assert(plusSprite != null, "Plus sprite must be not nil");

			IsContinuous = true;
			IgnoreAnchorPointForPosition = false;
            autorepeat = true;
            minimumValue = 0;
            maximumValue = 100;
            value = 0;
            stepValue = 1;
            wraps = false;

            MinusSprite = minusSprite;
			MinusSprite.Position = new CCPoint(minusSprite.ContentSize.Width / 2, minusSprite.ContentSize.Height / 2);
            AddChild(MinusSprite);

            MinusLabel = new CCLabelTtf("-", ControlStepperLabelFont, 38);
            MinusLabel.Color = ControlStepperLabelColorDisabled;
			MinusLabel.Position = new CCPoint(MinusSprite.ContentSize.Width / 2, MinusSprite.ContentSize.Height / 2);
            MinusSprite.AddChild(MinusLabel);

            PlusSprite = plusSprite;
			PlusSprite.Position = new CCPoint(minusSprite.ContentSize.Width + plusSprite.ContentSize.Width / 2, minusSprite.ContentSize.Height / 2);
            AddChild(PlusSprite);

            PlusLabel = new CCLabelTtf("+", ControlStepperLabelFont, 38);
            PlusLabel.Color = ControlStepperLabelColorEnabled;
            PlusLabel.Position = PlusSprite.ContentSize.Center;
            PlusSprite.AddChild(PlusLabel);

            // Defines the content size
            CCRect maxRect = CCControlUtils.CCRectUnion(MinusSprite.BoundingBox, PlusSprite.BoundingBox);
            ContentSize = new CCSize(MinusSprite.ContentSize.Width + PlusSprite.ContentSize.Height, maxRect.Size.Height);

			// Register Touch Event
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = OnTouchBegan;
			touchListener.OnTouchMoved = OnTouchMoved;
			touchListener.OnTouchEnded = OnTouchEnded;

            AddEventListener(touchListener);
        }

        #endregion Constructors


        public virtual void SetValueWithSendingEvent(float value, bool send)
        {
            if (value < minimumValue)
            {
                value = wraps ? maximumValue : minimumValue;
            }
            else if (value > maximumValue)
            {
                value = wraps ? minimumValue : maximumValue;
            }

            this.value = value;

            if (!wraps)
            {
                MinusLabel.Color = (value == minimumValue)
                                        ? ControlStepperLabelColorDisabled
                                        : ControlStepperLabelColorEnabled;
                PlusLabel.Color = (value == maximumValue)
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
            autorepeatCount++;

            if ((autorepeatCount < AutorepeatIncreaseTimeIncrement) && (autorepeatCount % 3) != 0)
                return;

            if (touchedPart == CCControlStepperPart.PartMinus)
            {
                SetValueWithSendingEvent(this.value - stepValue, IsContinuous);
            }
            else if (touchedPart == CCControlStepperPart.PartPlus)
            {
                SetValueWithSendingEvent(this.value + stepValue, IsContinuous);
            }
        }

		/** Update the layout of the stepper with the given touch location. */

		protected void UpdateLayoutUsingTouchLocation(CCPoint location)
		{
			if (location.X < MinusSprite.ContentSize.Width
				&& this.value > minimumValue)
			{
				touchedPart = CCControlStepperPart.PartMinus;

				MinusSprite.Color = CCColor3B.Gray;
				PlusSprite.Color = CCColor3B.White;
			}
			else if (location.X >= MinusSprite.ContentSize.Width
				&& this.value < maximumValue)
			{
				touchedPart = CCControlStepperPart.PartPlus;

				MinusSprite.Color = CCColor3B.White;
				PlusSprite.Color = CCColor3B.Gray;
			}
			else
			{
				touchedPart = CCControlStepperPart.PartNone;

				MinusSprite.Color = CCColor3B.White;
				PlusSprite.Color = CCColor3B.White;
			}
		}

		protected void StartAutorepeat()
		{
			autorepeatCount = -1;

			Schedule(Update, AutorepeatDeltaTime, CCSchedulePriority.RepeatForever, AutorepeatDeltaTime * 3);
		}

		protected void StopAutorepeat()
		{
			Unschedule(Update);
		}


		#region Event handling

		bool OnTouchBegan(CCTouch pTouch, CCEvent touchEvent)
        {
            if (!IsTouchInside(pTouch) || !Enabled || !Visible)
            {
                return false;
            }

            CCPoint location = GetTouchLocation(pTouch);
            UpdateLayoutUsingTouchLocation(location);

            touchInsideFlag = true;

            if (autorepeat)
            {
                StartAutorepeat();
            }

            return true;
        }

		void OnTouchMoved(CCTouch pTouch, CCEvent touchEvent)
        {
            if (IsTouchInside(pTouch))
            {
                CCPoint location = GetTouchLocation(pTouch);
                UpdateLayoutUsingTouchLocation(location);

                if (!touchInsideFlag)
                {
                    touchInsideFlag = true;

                    if (autorepeat)
                    {
                        StartAutorepeat();
                    }
                }
            }
            else
            {
                touchInsideFlag = false;

                touchedPart = CCControlStepperPart.PartNone;

                MinusSprite.Color = CCColor3B.White;
                PlusSprite.Color = CCColor3B.White;

                if (autorepeat)
                {
                    StopAutorepeat();
                }
            }
        }

		void OnTouchEnded(CCTouch pTouch, CCEvent touchEvent)
        {
            MinusSprite.Color = CCColor3B.White;
            PlusSprite.Color = CCColor3B.White;

            if (autorepeat)
            {
                StopAutorepeat();
            }

            if (IsTouchInside(pTouch))
            {
                CCPoint location = GetTouchLocation(pTouch);

				Value = this.value + ((location.X < MinusSprite.ContentSize.Width) ? (0.0f - stepValue) : stepValue);
            }
        }

		#endregion Event handling

    }
}