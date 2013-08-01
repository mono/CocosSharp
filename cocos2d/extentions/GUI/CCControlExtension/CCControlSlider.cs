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
        private CCSprite _backgroundSprite;
        private float _maximumAllowedValue;
        private float _maximumValue;
        private float _minimumAllowedValue;
        private float _minimumValue;
        private CCSprite _progressSprite;
        private CCSprite _thumbSprite;
        private float _value;

        public float Value
        {
            get { return _value; }
            set
            {
                // set new value with sentinel
                if (value < _minimumValue)
                {
                    value = _minimumValue;
                }

                if (value > _maximumValue)
                {
                    value = _maximumValue;
                }

                _value = value;

                NeedsLayout();

                SendActionsForControlEvents(CCControlEvent.ValueChanged);
            }
        }

        public float MinimumAllowedValue
        {
            get { return _minimumAllowedValue; }
            set { _minimumAllowedValue = value; }
        }

        public float MinimumValue
        {
            get { return _minimumValue; }
            set
            {
                _minimumValue = value;
                _minimumAllowedValue = value;
                if (_minimumValue >= _maximumValue)
                {
                    _maximumValue = _minimumValue + 1.0f;
                }

                Value = _value;
            }
        }

        public float MaximumAllowedValue
        {
            get { return _maximumAllowedValue; }
            set { _maximumAllowedValue = value; }
        }

        public float MaximumValue
        {
            get { return _maximumValue; }
            set
            {
                _maximumValue = value;
                _maximumAllowedValue = value;
                if (_maximumValue <= _minimumValue)
                {
                    _minimumValue = _maximumValue - 1.0f;
                }
                Value = _value;
            }
        }

        //interval to snap to
        public float SnappingInterval { get; set; }

        // maybe this should be read-only

        public CCSprite ThumbSprite
        {
            get { return _thumbSprite; }
            set { _thumbSprite = value; }
        }

        public CCSprite ProgressSprite
        {
            get { return _progressSprite; }
            set { _progressSprite = value; }
        }

        public CCSprite BackgroundSprite
        {
            get { return _backgroundSprite; }
            set { _backgroundSprite = value; }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                if (_thumbSprite != null)
                {
                    _thumbSprite.Opacity = (byte) (value ? 255 : 128);
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
                _backgroundSprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
                _backgroundSprite.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
                AddChild(_backgroundSprite);

                // Add the progress bar
                _progressSprite.AnchorPoint = new CCPoint(0.0f, 0.5f);
                _progressSprite.Position = new CCPoint(0.0f, ContentSize.Height / 2);
                AddChild(_progressSprite);

                // Add the slider thumb  
                _thumbSprite.Position = new CCPoint(0, ContentSize.Height / 2);
                AddChild(_thumbSprite);

                // Init default values
                _minimumValue = 0.0f;
                _maximumValue = 1.0f;

                Value = _minimumValue;
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
                Value = ValueForLocation(_thumbSprite.Position);
            }
            _thumbSprite.Color = CCTypes.CCWhite;
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
            else if (touchLocation.X > _backgroundSprite.ContentSize.Width)
            {
                touchLocation.X = _backgroundSprite.ContentSize.Width;
            }
            return touchLocation;
        }

        public override bool IsTouchInside(CCTouch touch)
        {
            CCPoint touchLocation = touch.Location;
            touchLocation = Parent.ConvertToNodeSpace(touchLocation);

            CCRect rect = BoundingBox;
            rect.Size.Width += _thumbSprite.ContentSize.Width;
            rect.Origin.X -= _thumbSprite.ContentSize.Width / 2;

            return rect.ContainsPoint(touchLocation);
        }

        public override bool TouchBegan(CCTouch touch)
        {
            if (!IsTouchInside(touch) || !Enabled || !Visible)
                return false;

            CCPoint location = LocationFromTouch(touch);
            SliderBegan(location);
            return true;
        }

        public override void TouchMoved(CCTouch pTouch)
        {
            CCPoint location = LocationFromTouch(pTouch);
            SliderMoved(location);
        }

        public override void TouchEnded(CCTouch pTouch)
        {
            SliderEnded(CCPoint.Zero);
        }

        public override void NeedsLayout()
        {
            if (null == _thumbSprite || null == _backgroundSprite || null == _progressSprite)
            {
                return;
            }
            // Update thumb position for new value
            float percent = (_value - _minimumValue) / (_maximumValue - _minimumValue);

            CCPoint pos = _thumbSprite.Position;
            pos.X = percent * _backgroundSprite.ContentSize.Width;
            _thumbSprite.Position = pos;

            // Stretches content proportional to newLevel
            CCRect textureRect = _progressSprite.TextureRect;
            textureRect = new CCRect(textureRect.Origin.X, textureRect.Origin.Y, pos.X, textureRect.Size.Height);
            _progressSprite.SetTextureRect(textureRect, _progressSprite.IsTextureRectRotated, textureRect.Size);
        }

        /** Returns the value for the given location. */

        protected float ValueForLocation(CCPoint location)
        {
            float percent = location.X / _backgroundSprite.ContentSize.Width;
            return Math.Max(Math.Min(_minimumValue + percent * (_maximumValue - _minimumValue), _maximumAllowedValue), _minimumAllowedValue);
        }
    };
}