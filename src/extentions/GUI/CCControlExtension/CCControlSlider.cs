/*
 * CCControlSlider
 *
 * Copyright 2011 Yannick Loriot. All rights reserved.
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
 * Converted to c++ / cocos2d-x by Angus C
 */

using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCControlSlider : CCControl
    {
        float maximumValue;
        float minimumValue;
        float value;


		#region Properties

		public float SnappingInterval { get; set; }
		public float MinimumAllowedValue { get; set; }
		public float MaximumAllowedValue { get; set; }
		public CCSprite ThumbSprite { get; set; }
		public CCSprite ProgressSprite { get; set; }
		public CCSprite BackgroundSprite { get; set; }

        public float Value
        {
			get { return this.value; }
            set
            {
				this.value = CCMathHelper.Clamp(value, MinimumValue, MaximumValue);

                NeedsLayout();

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
					
				MinimumAllowedValue = minimumValue;
                Value = value;
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

				MaximumAllowedValue = maximumValue;
                Value = value;
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (ThumbSprite != null)
                {
                    ThumbSprite.Opacity = (byte) (value ? 255 : 128);
                }
            }
        }

		#endregion Properties


        #region Constructors

        public CCControlSlider(string bgFile, string progressFile, string thumbFile) 
            : this(new CCSprite(bgFile), new CCSprite(progressFile), new CCSprite(thumbFile))
        {
        }

		public CCControlSlider(CCSprite backgroundSprite, CCSprite progressSprite, CCSprite thumbSprite)
		{
            Debug.Assert(backgroundSprite != null, "Background sprite must be not nil");
            Debug.Assert(progressSprite != null, "Progress sprite must be not nil");
            Debug.Assert(thumbSprite != null, "Thumb sprite must be not nil");

            BackgroundSprite = backgroundSprite;
            ProgressSprite = progressSprite;
            ThumbSprite = thumbSprite;
			minimumValue = 0.0f;
			maximumValue = 1.0f;
			Value = minimumValue;
			IgnoreAnchorPointForPosition = false;

            // Defines the content size
            CCRect maxRect = CCControlUtils.CCRectUnion(backgroundSprite.BoundingBox, thumbSprite.BoundingBox);
            ContentSize = new CCSize(maxRect.Size.Width, maxRect.Size.Height);

            // Add the slider background
            BackgroundSprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
            BackgroundSprite.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
            AddChild(BackgroundSprite);

            // Add the progress bar
            ProgressSprite.AnchorPoint = new CCPoint(0.0f, 0.5f);
            ProgressSprite.Position = new CCPoint(0.0f, ContentSize.Height / 2);
            AddChild(ProgressSprite);

            // Add the slider thumb  
            ThumbSprite.Position = new CCPoint(0, ContentSize.Height / 2);
            AddChild(ThumbSprite);

			// Register Touch Event
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = OnTouchBegan;
			touchListener.OnTouchMoved = OnTouchMoved;
			touchListener.OnTouchEnded = OnTouchEnded;

			AddEventListener(touchListener);
        }

        #endregion Constructors


		protected float ValueForLocation(CCPoint location)
		{
			float percent = location.X / BackgroundSprite.ContentSize.Width;
			return Math.Max(Math.Min(minimumValue + percent * (MaximumValue - MinimumValue), MaximumAllowedValue), MinimumAllowedValue);
		}

		protected virtual CCPoint LocationFromTouch(CCTouch touch)
		{
			CCPoint touchLocation = touch.Location; // Get the touch position
			touchLocation = ConvertToNodeSpace(touchLocation); // Convert to the node space of this class

			if (touchLocation.X < 0)
			{
				touchLocation.X = 0;
			}
			else if (touchLocation.X > BackgroundSprite.ContentSize.Width)
			{
				touchLocation.X = BackgroundSprite.ContentSize.Width;
			}
			return touchLocation;
		}

		public override bool IsTouchInside(CCTouch touch)
		{
			CCPoint touchLocation = touch.Location;
			touchLocation = Parent.ConvertToNodeSpace(touchLocation);

			CCRect rect = BoundingBox;
			rect.Size.Width += ThumbSprite.ContentSize.Width;
			rect.Origin.X -= ThumbSprite.ContentSize.Width / 2;

			return rect.ContainsPoint(touchLocation);
		}


		#region Slider event handling

        protected void SliderBegan(CCPoint location)
        {
            Selected = true;
            ThumbSprite.Color = CCColor3B.Gray;
            Value = ValueForLocation(location);
        }

        protected void SliderMoved(CCPoint location)
        {
            Value = ValueForLocation(location);
        }

        protected void SliderEnded(CCPoint location)
        {
            if (Selected)
            {
                Value = ValueForLocation(ThumbSprite.Position);
            }
            ThumbSprite.Color = CCColor3B.White;
            Selected = false;
        }

		bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            if (!IsTouchInside(touch) || !Enabled || !Visible)
                return false;

            CCPoint location = LocationFromTouch(touch);
            SliderBegan(location);
            return true;
        }

		void OnTouchMoved(CCTouch pTouch, CCEvent touchEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);
            SliderMoved(location);
        }

		void OnTouchEnded(CCTouch pTouch, CCEvent touchEvent)
        {
            SliderEnded(CCPoint.Zero);
        }

		#endregion Slider event handling


        public override void NeedsLayout()
        {
            if (null == ThumbSprite || null == BackgroundSprite || null == ProgressSprite)
            {
                return;
            }
            // Update thumb position for new value
            float percent = (value - minimumValue) / (maximumValue - minimumValue);

            CCPoint pos = ThumbSprite.Position;
            pos.X = percent * BackgroundSprite.ContentSize.Width;
            ThumbSprite.Position = pos;

            // Stretches content proportional to newLevel
            CCRect textureRect = ProgressSprite.TextureRect;
            textureRect = new CCRect(textureRect.Origin.X, textureRect.Origin.Y, pos.X, textureRect.Size.Height);
            ProgressSprite.SetTextureRect(textureRect, ProgressSprite.IsTextureRectRotated, textureRect.Size);
        }
    };
}