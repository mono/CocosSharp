/*
 * CCControlSwitch.h
 *
 * Copyright 2012 Yannick Loriot. All rights reserved.
 * http://yannickloriot.com
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 *
 */


using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public delegate void CCSwitchValueChangedDelegate(object sender, bool bState);

    /** @class CCControlSwitch Switch control for Cocos2D. */

    public class CCControlSwitch : CCControl
    {
        bool on;
        float initialTouchXPosition;
        CCControlSwitchSprite switchSprite;

		public event CCSwitchValueChangedDelegate OnValueChanged;


		#region Properties

		public bool HasMoved { get; private set; }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (switchSprite != null)
                {
                    switchSprite.Opacity = (byte) (value ? 255 : 128);
                }
            }
        }

		public bool On
		{
			get { return on; }
			set
			{
				bool notify = false;
				if(on != value) 
				{
					on = value;
					notify = true;
				}

				switchSprite.RunAction (
					new CCActionTween (
						0.2f,
						"sliderXPosition",
						switchSprite.SliderXPosition,
						(on) ? switchSprite.OnPosition : switchSprite.OffPosition
					)
				);

				if(notify) 
				{
					SendActionsForControlEvents(CCControlEvent.ValueChanged);
					if (OnValueChanged != null) 
					{
						OnValueChanged(this, on);
					}
				}
			}
		}

		#endregion Properties


        #region Constructors

		public CCControlSwitch(CCSprite maskSprite, CCSprite onSprite, CCSprite offSprite, CCSprite thumbSprite) 
			: this(maskSprite, onSprite, offSprite, thumbSprite, null, null)
		{
		}

        public CCControlSwitch(CCSprite maskSprite, CCSprite onSprite, CCSprite offSprite, CCSprite thumbSprite, CCLabel onLabel,
            CCLabel offLabel)
        {
            Debug.Assert(maskSprite != null, "Mask must not be nil.");
            Debug.Assert(onSprite != null, "onSprite must not be nil.");
            Debug.Assert(offSprite != null, "offSprite must not be nil.");
            Debug.Assert(thumbSprite != null, "thumbSprite must not be nil.");

            on = true;

            switchSprite = new CCControlSwitchSprite(maskSprite, onSprite, offSprite, thumbSprite, onLabel, offLabel);
            switchSprite.Position = new CCPoint(switchSprite.ContentSize.Width / 2, switchSprite.ContentSize.Height / 2);
            AddChild(switchSprite);

            IgnoreAnchorPointForPosition = false;
            AnchorPoint = new CCPoint(0.5f, 0.5f);
            ContentSize = switchSprite.ContentSize;

			// Register Touch Event
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = OnTouchBegan;
			touchListener.OnTouchMoved = OnTouchMoved;
			touchListener.OnTouchEnded = OnTouchEnded;
			touchListener.OnTouchCancelled = OnTouchCancelled;

            AddEventListener(touchListener);
        }

        #endregion Constructors


        public CCPoint LocationFromTouch(CCTouch touch)
        {
            CCPoint touchLocation = touch.LocationOnScreen;
            touchLocation = WorldToParentspace(Layer.ScreenToWorldspace(touchLocation));	

            return touchLocation;
        }


		#region Event handling

		bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
			if (!IsTouchInside(touch) || !Enabled)
            {
                return false;
            }

            HasMoved = false;

			CCPoint location = LocationFromTouch(touch);

            initialTouchXPosition = location.X - switchSprite.SliderXPosition;

            switchSprite.ThumbSprite.Color = new CCColor3B(166, 166, 166);
            switchSprite.NeedsLayout();

            return true;
        }

		void OnTouchMoved(CCTouch pTouch, CCEvent touchEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);
            location = new CCPoint(location.X - initialTouchXPosition, 0);

            HasMoved = true;

            switchSprite.SliderXPosition = location.X;
        }

		void OnTouchEnded(CCTouch pTouch, CCEvent touchEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);

            switchSprite.ThumbSprite.Color = new CCColor3B(255, 255, 255);

            if (HasMoved)
            {
				On = !(location.X < switchSprite.ContentSize.Width / 2);
            }
            else
            {
				On = !on;
            }
        }

		void OnTouchCancelled(CCTouch pTouch, CCEvent touchEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);

            switchSprite.ThumbSprite.Color = new CCColor3B(255, 255, 255);

            if (HasMoved)
            {
				On = !(location.X < switchSprite.ContentSize.Width / 2);
            }
            else
            {
				On = !on;
            }
        }

		#endregion Event handling
    }

    public class CCControlSwitchSprite : CCSprite, ICCActionTweenDelegate
    {
        float sliderXPosition;


		#region Properties

		public float OnPosition { get; set; }
		public float OffPosition { get; set; }

		public CCSprite MaskSprite { get; set; }        
		public CCSprite OnSprite { get; set; }
		public CCSprite OffSprite { get; set; }
		public CCSprite ThumbSprite { get; set; }

		public CCLabel OnLabel { get; set; }
		public CCLabel OffLabel { get; set; }


		public float OnSideWidth
		{
			get { return OnSprite.ContentSize.Width; }
		}

		public float OffSideWidth
		{
			get { return OffSprite.ContentSize.Height; }
		}

        public float SliderXPosition
        {
            get { return sliderXPosition; }
            set
            {
                if (value <= OffPosition)
                {
                    // Off
                    value = OffPosition;
                }
                else if (value >= OnPosition)
                {
                    // On
                    value = OnPosition;
                }

                sliderXPosition = value;

                NeedsLayout();
            }
        }

		#endregion Properties


        #region Constructors

		public CCControlSwitchSprite(CCSprite maskSprite, CCSprite onSprite, CCSprite offSprite, CCSprite thumbSprite, 
			CCLabel onLabel, CCLabel offLabel) 
			: base((CCTexture2D)null, new CCRect(0.0f, 0.0f, maskSprite.TextureRectInPixels.Size.Width, maskSprite.TextureRectInPixels.Size.Height))
        {
            OnPosition = 0;
            OffPosition = -onSprite.ContentSize.Width + thumbSprite.ContentSize.Width / 2;
            sliderXPosition = OnPosition;

            OnSprite = onSprite;
            OffSprite = offSprite;
            ThumbSprite = thumbSprite;
            OnLabel = onLabel;
            OffLabel = offLabel;
            MaskSprite = maskSprite;

            AddChild(ThumbSprite);

            NeedsLayout();
        }

        #endregion Constructors


        public virtual void UpdateTweenAction(float value, string key)
        {
            //CCLog.Log("key = {0}, value = {1}", key, value);
            SliderXPosition = value;
        }

        public void NeedsLayout()
        {
			OnSprite.Position = new CCPoint(OnSprite.ContentSize.Width / 2 + sliderXPosition, OnSprite.ContentSize.Height / 2);
			OffSprite.Position = new CCPoint(OnSprite.ContentSize.Width + OffSprite.ContentSize.Width / 2 + sliderXPosition, OffSprite.ContentSize.Height / 2);
			ThumbSprite.Position = new CCPoint(OnSprite.ContentSize.Width + sliderXPosition, MaskSprite.ContentSize.Height / 2);

            if (OnLabel != null)
            {
				OnLabel.Position = new CCPoint(OnSprite.Position.X - ThumbSprite.ContentSize.Width / 6, OnSprite.ContentSize.Height / 2);
            }
            if (OffLabel != null)
            {
				OffLabel.Position = new CCPoint(OffSprite.Position.X + ThumbSprite.ContentSize.Width / 6, OffSprite.ContentSize.Height / 2);
            }

            var rt = new CCRenderTexture(
                MaskSprite.TextureRectInPixels.Size,
                MaskSprite.TextureRectInPixels.Size,
				CCSurfaceFormat.Color, CCDepthFormat.None, CCRenderTargetUsage.DiscardContents
                );

            rt.BeginWithClear(0, 0, 0, 0);

            OnSprite.Visit();
            OffSprite.Visit();

            if (OnLabel != null)
            {
                OnLabel.Visit();
            }
            if (OffLabel != null)
            {
                OffLabel.Visit();
            }

            MaskSprite.AnchorPoint = new CCPoint(0, 0);
            MaskSprite.BlendFunc = new CCBlendFunc(CCOGLES.GL_ZERO, CCOGLES.GL_SRC_ALPHA);

            MaskSprite.Visit();

            rt.End();

			Texture = rt.Sprite.Texture;
            
            ContentSize = MaskSprite.ContentSize;
        }
    }
}
