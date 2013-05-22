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

namespace Cocos2D
{
    public class CCControlSlider : CCControl
    {
        //maunally put in the setters
        private CCSprite m_backgroundSprite;
        private float m_maximumAllowedValue;
        private float m_maximumValue;
        private float m_minimumAllowedValue;
        private float m_minimumValue;
        private CCSprite m_progressSprite;
        private CCSprite m_thumbSprite;
        private float m_value;

        public float Value
        {
            get { return m_value; }
            set
            {
                // set new value with sentinel
                if (value < m_minimumValue)
                {
                    value = m_minimumValue;
                }

                if (value > m_maximumValue)
                {
                    value = m_maximumValue;
                }

                m_value = value;

                NeedsLayout();

                SendActionsForControlEvents(CCControlEvent.ValueChanged);
            }
        }

        public float MinimumAllowedValue
        {
            get { return m_minimumAllowedValue; }
            set { m_minimumAllowedValue = value; }
        }

        public float MinimumValue
        {
            get { return m_minimumValue; }
            set
            {
                m_minimumValue = value;
                m_minimumAllowedValue = value;
                if (m_minimumValue >= m_maximumValue)
                {
                    m_maximumValue = m_minimumValue + 1.0f;
                }

                Value = m_value;
            }
        }

        public float MaximumAllowedValue
        {
            get { return m_maximumAllowedValue; }
            set { m_maximumAllowedValue = value; }
        }

        public float MaximumValue
        {
            get { return m_maximumValue; }
            set
            {
                m_maximumValue = value;
                m_maximumAllowedValue = value;
                if (m_maximumValue <= m_minimumValue)
                {
                    m_minimumValue = m_maximumValue - 1.0f;
                }
                Value = m_value;
            }
        }

        //interval to snap to
        public float SnappingInterval { get; set; }

        // maybe this should be read-only

        public CCSprite ThumbSprite
        {
            get { return m_thumbSprite; }
            set { m_thumbSprite = value; }
        }

        public CCSprite ProgressSprite
        {
            get { return m_progressSprite; }
            set { m_progressSprite = value; }
        }

        public CCSprite BackgroundSprite
        {
            get { return m_backgroundSprite; }
            set { m_backgroundSprite = value; }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (m_thumbSprite != null)
                {
                    m_thumbSprite.Opacity = (byte) (value ? 255 : 128);
                }
            }
        }

        /** 
		* Initializes a slider with a background sprite, a progress bar and a thumb
		* item.
		*
		* @param backgroundSprite  CCSprite, that is used as a background.
		* @param progressSprite    CCSprite, that is used as a progress bar.
		* @param thumbItem         CCMenuItem, that is used as a thumb.
		*/

        public virtual bool InitWithSprites(CCSprite backgroundSprite, CCSprite progressSprite, CCSprite thumbSprite)
        {
            if (base.Init())
            {
                Debug.Assert(backgroundSprite != null, "Background sprite must be not nil");
                Debug.Assert(progressSprite != null, "Progress sprite must be not nil");
                Debug.Assert(thumbSprite != null, "Thumb sprite must be not nil");

                IgnoreAnchorPointForPosition = false;
                TouchEnabled = true;

                BackgroundSprite = backgroundSprite;
                ProgressSprite = progressSprite;
                ThumbSprite = thumbSprite;

                // Defines the content size
                CCRect maxRect = CCControlUtils.CCRectUnion(backgroundSprite.BoundingBox, thumbSprite.BoundingBox);
                ContentSize = new CCSize(maxRect.Size.Width, maxRect.Size.Height);

                //setContentSize(CCSizeMake(backgroundSprite->getContentSize().width, thumbItem->getContentSize().height));
                // Add the slider background
                m_backgroundSprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
                m_backgroundSprite.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
                AddChild(m_backgroundSprite);

                // Add the progress bar
                m_progressSprite.AnchorPoint = new CCPoint(0.0f, 0.5f);
                m_progressSprite.Position = new CCPoint(0.0f, ContentSize.Height / 2);
                AddChild(m_progressSprite);

                // Add the slider thumb  
                m_thumbSprite.Position = new CCPoint(0, ContentSize.Height / 2);
                AddChild(m_thumbSprite);

                // Init default values
                m_minimumValue = 0.0f;
                m_maximumValue = 1.0f;

                Value = m_minimumValue;
                return true;
            }
            return false;
        }

        /** 
		* Creates slider with a background filename, a progress filename and a 
		* thumb image filename.
		*/

        public CCControlSlider(string bgFile, string progressFile, string thumbFile)
        {
            // Prepare background for slider
            CCSprite backgroundSprite = new CCSprite(bgFile);

            // Prepare progress for slider
            CCSprite progressSprite = new CCSprite(progressFile);

            // Prepare thumb (menuItem) for slider
            CCSprite thumbSprite = new CCSprite(thumbFile);

            InitWithSprites(backgroundSprite, progressSprite, thumbSprite);
        }

        /** 
		* Creates a slider with a given background sprite and a progress bar and a
		* thumb item.
		*
		* @see initWithBackgroundSprite:progressSprite:thumbMenuItem:
		*/

        public CCControlSlider(CCSprite backgroundSprite, CCSprite progressSprite, CCSprite thumbSprite)
        {
            InitWithSprites(backgroundSprite, progressSprite, thumbSprite);
        }

        protected void SliderBegan(CCPoint location)
        {
            Selected = true;
            ThumbSprite.Color = CCTypes.CCGray;
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
                Value = ValueForLocation(m_thumbSprite.Position);
            }
            m_thumbSprite.Color = CCTypes.CCWhite;
            Selected = false;
        }

        protected virtual CCPoint LocationFromTouch(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location; // Get the touch position
            touchLocation = ConvertToNodeSpace(touchLocation); // Convert to the node space of this class

            if (touchLocation.X < 0)
            {
                touchLocation.X = 0;
            }
            else if (touchLocation.X > m_backgroundSprite.ContentSize.Width)
            {
                touchLocation.X = m_backgroundSprite.ContentSize.Width;
            }
            return touchLocation;
        }

        public override bool IsTouchInside(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;
            touchLocation = Parent.ConvertToNodeSpace(touchLocation);

            CCRect rect = BoundingBox;
            rect.Size.Width += m_thumbSprite.ContentSize.Width;
            rect.Origin.X -= m_thumbSprite.ContentSize.Width / 2;

            return rect.ContainsPoint(touchLocation);
        }

        public override bool TouchBegan(CCTouch touch, CCEvent pEvent)
        {
            if (!IsTouchInside(touch) || !Enabled)
                return false;

            CCPoint location = LocationFromTouch(touch);
            SliderBegan(location);
            return true;
        }

        public override void TouchMoved(CCTouch pTouch, CCEvent pEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);
            SliderMoved(location);
        }

        public override void TouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
            CCPoint location = LocationFromTouch(pTouch);
            SliderEnded(location);
        }

        public override void NeedsLayout()
        {
            if (null == m_thumbSprite || null == m_backgroundSprite || null == m_progressSprite)
            {
                return;
            }
            // Update thumb position for new value
            float percent = (m_value - m_minimumValue) / (m_maximumValue - m_minimumValue);

            CCPoint pos = m_thumbSprite.Position;
            pos.X = percent * m_backgroundSprite.ContentSize.Width;
            m_thumbSprite.Position = pos;

            // Stretches content proportional to newLevel
            CCRect textureRect = m_progressSprite.TextureRect;
            textureRect = new CCRect(textureRect.Origin.X, textureRect.Origin.Y, pos.X, textureRect.Size.Height);
            m_progressSprite.SetTextureRect(textureRect, m_progressSprite.IsTextureRectRotated, textureRect.Size);
        }

        /** Returns the value for the given location. */

        protected float ValueForLocation(CCPoint location)
        {
            float percent = location.X / m_backgroundSprite.ContentSize.Width;
            return Math.Max(Math.Min(m_minimumValue + percent * (m_maximumValue - m_minimumValue), m_maximumAllowedValue), m_minimumAllowedValue);
        }
    };
}