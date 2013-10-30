using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cocos2D
{
    public delegate void CCButtonTapDelegate(object sender);

    public class CCControlButton : CCControl
    {
        /* Define the button margin for Left/Right edge */
        public const int CCControlButtonMarginLR = 8; // px
        /* Define the button margin for Top/Bottom edge */
        public const int CCControlButtonMarginTB = 2; // px

        public const int kZoomActionTag = (0x7CCB0001);
        protected bool _parentInited;

        protected CCNode _backgroundSprite;
        protected Dictionary<CCControlState, CCNode> _backgroundSpriteDispatchTable;
        protected string _currentTitle;

        /** The current color used to display the title. */
        protected CCColor3B _currentTitleColor;
        protected bool _doesAdjustBackgroundImage;
        protected bool _isPushed;
        protected CCPoint _labelAnchorPoint;
        protected int _marginH = CCControlButtonMarginLR;
        protected int _marginV = CCControlButtonMarginTB;
        protected CCSize _preferredSize;
        protected Dictionary<CCControlState, CCColor3B> _titleColorDispatchTable;
        protected Dictionary<CCControlState, string> _titleDispatchTable;
        protected CCNode _titleLabel;
        protected Dictionary<CCControlState, CCNode> _titleLabelDispatchTable;
        protected bool _zoomOnTouchDown;

        public event CCButtonTapDelegate OnButtonTap;

        /// <summary>
        /// Default ctor. Does nothing.
        /// </summary>
        public CCControlButton()
        {
        }

        public CCNode BackgroundSprite
        {
            get { return _backgroundSprite; }
            set { _backgroundSprite = value; }
        }

        public CCNode TitleLabel
        {
            get { return _titleLabel; }
            set { _titleLabel = value; }
        }

        public override byte Opacity
        {
            get { return _realOpacity; }
            set
            {
                base.Opacity = value;
                foreach (ICCRGBAProtocol item in _backgroundSpriteDispatchTable.Values)
                {
                    if (item != null)
                    {
                        item.Opacity = value;
                    }
                }
            }
        }

        public override CCColor3B Color
        {
            get { return base._realColor; }
            set
            {
                base.Color = value;
                foreach (ICCRGBAProtocol item in _backgroundSpriteDispatchTable.Values)
                {
                    if (item != null)
                    {
                        item.Color = value;
                    }
                }
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                NeedsLayout();
            }
        }

        public override bool Selected
        {
            set
            {
                base.Selected = value;
                NeedsLayout();
            }
        }

        public override bool Highlighted
        {
            set
            {
                base.Highlighted = value;

                CCAction action = GetActionByTag(kZoomActionTag);
                if (action != null)
                {
                    StopAction(action);
                }

                NeedsLayout();

                if (_zoomOnTouchDown)
                {
                    float scaleValue = (Highlighted && Enabled && !Selected) ? 1.1f : 1.0f;
                    CCAction zoomAction = new CCScaleTo(0.05f, scaleValue);
                    zoomAction.Tag = kZoomActionTag;
                    RunAction(zoomAction);
                }
            }
        }

        public bool IsPushed
        {
            get { return _isPushed; }
        }

        public override void NeedsLayout()
        {
            if (!_parentInited)
            {
                return;
            }
            // Hide the background and the label
            if (_titleLabel != null)
            {
                _titleLabel.Visible = false;
            }
            if (_backgroundSprite != null)
            {
                _backgroundSprite.Visible = false;
            }
            // Update anchor of all labels
            LabelAnchorPoint = _labelAnchorPoint;

            // Update the label to match with the current state

            _currentTitle = GetTitleForState(_state);
            _currentTitleColor = GetTitleColorForState(_state);

            TitleLabel = GetTitleLabelForState(_state);

            var label = (ICCLabelProtocol) _titleLabel;
            if (label != null && !String.IsNullOrEmpty(_currentTitle))
            {
                label.Text = (_currentTitle);
            }

            var rgbaLabel = (ICCRGBAProtocol) _titleLabel;
            if (rgbaLabel != null)
            {
                rgbaLabel.Color = _currentTitleColor;
            }
            if (_titleLabel != null)
            {
                _titleLabel.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
            }

            // Update the background sprite
            BackgroundSprite = GetBackgroundSpriteForState(_state);
            if (_backgroundSprite != null)
            {
                _backgroundSprite.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
            }

            // Get the title label size
            CCSize titleLabelSize = CCSize.Zero;
            if (_titleLabel != null)
            {
                titleLabelSize = _titleLabel.BoundingBox.Size;
            }

            // Adjust the background image if necessary
            if (_doesAdjustBackgroundImage)
            {
                // Add the margins
                if (_backgroundSprite != null)
                {
                    _backgroundSprite.ContentSize = new CCSize(titleLabelSize.Width + _marginH * 2, titleLabelSize.Height + _marginV * 2);
                }
            }
            else
            {
                //TODO: should this also have margins if one of the preferred sizes is relaxed?
                if (_backgroundSprite != null && _backgroundSprite is CCScale9Sprite)
                {
                    CCSize preferredSize = ((CCScale9Sprite)_backgroundSprite).PreferredSize;
                    if (preferredSize.Width <= 0)
                    {
                        preferredSize.Width = titleLabelSize.Width;
                    }
                    if (preferredSize.Height <= 0)
                    {
                        preferredSize.Height = titleLabelSize.Height;
                    }

                    _backgroundSprite.ContentSize = preferredSize;
                }
            }

            // Set the content size
            CCRect rectTitle = CCRect.Zero;
            if (_titleLabel != null)
            {
                rectTitle = _titleLabel.BoundingBox;
            }
            CCRect rectBackground = CCRect.Zero;
            if (_backgroundSprite != null)
            {
                rectBackground = _backgroundSprite.BoundingBox;
            }

            CCRect maxRect = CCControlUtils.CCRectUnion(rectTitle, rectBackground);
            ContentSize = new CCSize(maxRect.Size.Width, maxRect.Size.Height);

            if (_titleLabel != null)
            {
                _titleLabel.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
                // Make visible label
                _titleLabel.Visible = true;
            }

            if (_backgroundSprite != null)
            {
                _backgroundSprite.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
                // Make visible the background
                _backgroundSprite.Visible = true;
            }
        }

        /** Adjust the background image. YES by default. If the property is set to NO, the 
        background will use the prefered size of the background image. */

        public void SetAdjustBackgroundImage(bool adjustBackgroundImage)
        {
            _doesAdjustBackgroundImage = adjustBackgroundImage;
            NeedsLayout();
        }

        public bool DoesAdjustBackgroundImage()
        {
            return _doesAdjustBackgroundImage;
        }


        /** Adjust the button zooming on touchdown. Default value is YES. */

        public bool ZoomOnTouchDown
        {
            set { _zoomOnTouchDown = value; }
            get { return _zoomOnTouchDown; }
        }

        /** The prefered size of the button, if label is larger it will be expanded. */

        public CCSize PreferredSize
        {
            get { return _preferredSize; }
            set
            {
                if (value.Width == 0 && value.Height == 0)
                {
                    _doesAdjustBackgroundImage = true;
                }
                else
                {
                    _doesAdjustBackgroundImage = false;
                    foreach (var item in _backgroundSpriteDispatchTable)
                    {
                        var sprite = item.Value as CCScale9Sprite;
                        if (sprite != null)
                        {
                            sprite.PreferredSize = value;
                        }
                    }
                }

                _preferredSize = value;

                NeedsLayout();
            }
        }


        public CCPoint LabelAnchorPoint
        {
            get { return _labelAnchorPoint; }
            set
            {
                _labelAnchorPoint = value;
                if (_titleLabel != null)
                {
                    _titleLabel.AnchorPoint = value;
                }
            }
        }

        /** The current title that is displayed on the button. */

        //set the margins at once (so we only have to do one call of needsLayout)
        protected virtual void SetMargins(int marginH, int marginV)
        {
            _marginV = marginV;
            _marginH = marginH;
            NeedsLayout();
        }

        public override bool Init()
        {
            return InitWithLabelAndBackgroundSprite(new CCLabelTTF("", "Arial", 12), new CCSprite());
        }

        public virtual bool InitWithLabelAndBackgroundSprite(CCNode node, CCNode backgroundSprite)
        {
            if (base.Init())
            {
                Debug.Assert(node != null, "Label must not be nil.");
                var label = node as ICCLabelProtocol;
                var rgbaLabel = node as ICCRGBAProtocol;
                Debug.Assert(backgroundSprite != null, "Background sprite must not be nil.");
                Debug.Assert(label != null || rgbaLabel != null || backgroundSprite != null);

                _parentInited = true;

                // Initialize the button state tables
                _titleDispatchTable = new Dictionary<CCControlState, string>();
                _titleColorDispatchTable = new Dictionary<CCControlState, CCColor3B>();
                _titleLabelDispatchTable = new Dictionary<CCControlState, CCNode>();
                _backgroundSpriteDispatchTable = new Dictionary<CCControlState, CCNode>();

                TouchEnabled = true;
                _isPushed = false;
                _zoomOnTouchDown = true;

                _currentTitle = null;

                // Adjust the background image by default
                SetAdjustBackgroundImage(true);
                PreferredSize = CCSize.Zero;
                // Zooming button by default
                _zoomOnTouchDown = true;

                // Set the default anchor point
                IgnoreAnchorPointForPosition = false;
                AnchorPoint = new CCPoint(0.5f, 0.5f);

                // Set the nodes
                TitleLabel = node;
                BackgroundSprite = backgroundSprite;

                // Set the default color and opacity
                Color = new CCColor3B(255, 255, 255);
                Opacity = 255;
                IsOpacityModifyRGB = true;

                // Initialize the dispatch table

                string tempString = label.Text;
                //tempString->autorelease();
                SetTitleForState(tempString, CCControlState.Normal);
                SetTitleColorForState(rgbaLabel.Color, CCControlState.Normal);
                SetTitleLabelForState(node, CCControlState.Normal);
                SetBackgroundSpriteForState(backgroundSprite, CCControlState.Normal);

                LabelAnchorPoint = new CCPoint(0.5f, 0.5f);

                NeedsLayout();

                return true;
            }
            //couldn't init the CCControl
            return false;
        }

        public CCControlButton(CCNode label, CCNode backgroundSprite)
        {
            InitWithLabelAndBackgroundSprite(label, backgroundSprite);
        }

        protected virtual bool InitWithTitleAndFontNameAndFontSize(string title, string fontName, float fontSize)
        {
            CCLabelTTF label = new CCLabelTTF(title, fontName, fontSize);
            return InitWithLabelAndBackgroundSprite(label, new CCNode());
        }

        public CCControlButton(string title, string fontName, float fontSize)
        {
            InitWithTitleAndFontNameAndFontSize(title, fontName, fontSize);
        }

        protected virtual bool InitWithBackgroundSprite(CCNode sprite)
        {
            CCLabelTTF label = new CCLabelTTF("", "Arial", 30);
            return InitWithLabelAndBackgroundSprite(label, sprite);
        }

        public CCControlButton(CCNode sprite)
        {
            InitWithBackgroundSprite(sprite);
        }

        //events
        public override bool TouchBegan(CCTouch pTouch)
        {
            if (!IsTouchInside(pTouch) || !Enabled)
            {
                return false;
            }

            _state = CCControlState.Highlighted;
            _isPushed = true;
            Highlighted = true;
            SendActionsForControlEvents(CCControlEvent.TouchDown);
            return true;
        }

        public override void TouchMoved(CCTouch pTouch)
        {
            if (!Enabled || !IsPushed || Selected)
            {
                if (Highlighted)
                {
                    Highlighted = false;
                }
                return;
            }

            bool isTouchMoveInside = IsTouchInside(pTouch);
            if (isTouchMoveInside && !_highlighted)
            {
                _state = CCControlState.Highlighted;
                Highlighted = true;
                SendActionsForControlEvents(CCControlEvent.TouchDragEnter);
            }
            else if (isTouchMoveInside && Highlighted)
            {
                SendActionsForControlEvents(CCControlEvent.TouchDragInside);
            }
            else if (!isTouchMoveInside && Highlighted)
            {
                _state = CCControlState.Normal;
                Highlighted = false;

                SendActionsForControlEvents(CCControlEvent.TouchDragExit);
            }
            else if (!isTouchMoveInside && !Highlighted)
            {
                SendActionsForControlEvents(CCControlEvent.TouchDragOutside);
            }
        }

        public override void TouchEnded(CCTouch pTouch)
        {
            _state = CCControlState.Normal;
            _isPushed = false;
            Highlighted = false;


            if (IsTouchInside(pTouch))
            {
                if (OnButtonTap != null)
                {
                    OnButtonTap(this);
                }
                SendActionsForControlEvents(CCControlEvent.TouchUpInside);
            }
            else
            {
                SendActionsForControlEvents(CCControlEvent.TouchUpOutside);
            }
        }

        public override void TouchCancelled(CCTouch pTouch)
        {
            _state = CCControlState.Normal;
            _isPushed = false;
            Highlighted = false;
            SendActionsForControlEvents(CCControlEvent.TouchCancel);
        }

        /**
        * Returns the title used for a state.
        *
        * @param state The state that uses the title. Possible values are described in
        * "CCControlState".
        *
        * @return The title for the specified state.
        */

        public virtual string GetTitleForState(CCControlState state)
        {
            if (_titleDispatchTable != null)
            {
                string title;
                if (_titleDispatchTable.TryGetValue(state, out title))
                {
                    return title;
                }
                if (_titleDispatchTable.TryGetValue(CCControlState.Normal, out title))
                {
                    return title;
                }
            }
            return String.Empty;
        }

        /**
    * Sets the title string to use for the specified state.
    * If a property is not specified for a state, the default is to use
    * the CCButtonStateNormal value.
    *
    * @param title The title string to use for the specified state.
    * @param state The state that uses the specified title. The values are described
    * in "CCControlState".
    */

        public virtual void SetTitleForState(string title, CCControlState state)
        {
            if (_titleDispatchTable.ContainsKey(state))
            {
                _titleDispatchTable.Remove(state);
            }

            if (!String.IsNullOrEmpty(title))
            {
                _titleDispatchTable.Add(state, title);
            }

            // If the current state if equal to the given state we update the layout
            if (State == state)
            {
                NeedsLayout();
            }
        }

        /**
    * Returns the title color used for a state.
    *
    * @param state The state that uses the specified color. The values are described
    * in "CCControlState".
    *
    * @return The color of the title for the specified state.
    */

        public virtual CCColor3B GetTitleColorForState(CCControlState state)
        {
            if (_titleColorDispatchTable != null)
            {
                CCColor3B color;

                if (_titleColorDispatchTable.TryGetValue(state, out color))
                {
                    return color;
                }

                if (_titleColorDispatchTable.TryGetValue(CCControlState.Normal, out color))
                {
                    return color;
                }
            }
            return CCTypes.CCWhite;
        }

        /**
    * Sets the color of the title to use for the specified state.
    *
    * @param color The color of the title to use for the specified state.
    * @param state The state that uses the specified color. The values are described
    * in "CCControlState".
    */

        public virtual void SetTitleColorForState(CCColor3B color, CCControlState state)
        {
            if (_titleColorDispatchTable.ContainsKey(state))
            {
                _titleColorDispatchTable.Remove(state);
            }

            _titleColorDispatchTable.Add(state, color);

            // If the current state if equal to the given state we update the layout
            if (State == state)
            {
                NeedsLayout();
            }
        }

        /**
    * Returns the title label used for a state.
    *
    * @param state The state that uses the title label. Possible values are described
    * in "CCControlState".
    */

        public virtual CCNode GetTitleLabelForState(CCControlState state)
        {
            CCNode titleLabel;
            if (_titleLabelDispatchTable.TryGetValue(state, out titleLabel))
            {
                return titleLabel;
            }
            if (_titleLabelDispatchTable.TryGetValue(CCControlState.Normal, out titleLabel))
            {
                return titleLabel;
            }
            return null;
        }

        /**
    * Sets the title label to use for the specified state.
    * If a property is not specified for a state, the default is to use
    * the CCButtonStateNormal value.
    *
    * @param title The title label to use for the specified state.
    * @param state The state that uses the specified title. The values are described
    * in "CCControlState".
    */

        public virtual void SetTitleLabelForState(CCNode titleLabel, CCControlState state)
        {
            CCNode previousLabel;
            if (_titleLabelDispatchTable.TryGetValue(state, out previousLabel))
            {
                RemoveChild(previousLabel, true);
                _titleLabelDispatchTable.Remove(state);
            }

            _titleLabelDispatchTable.Add(state, titleLabel);
            titleLabel.Visible = false;
            titleLabel.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(titleLabel, 1);

            // If the current state if equal to the given state we update the layout
            if (State == state)
            {
                NeedsLayout();
            }
        }

        public virtual void SetTitleTtfForState(string fntFile, CCControlState state)
        {
            string title = GetTitleForState(state);
            if (title == null)
            {
                title = String.Empty;
            }
            SetTitleLabelForState(new CCLabelTTF(title, fntFile, 12), state);
        }

        public virtual string GetTitleTtfForState(CCControlState state)
        {
            var label = (ICCLabelProtocol) GetTitleLabelForState(state);
            var labelTtf = label as CCLabelTTF;
            if (labelTtf != null)
            {
                return labelTtf.FontName;
            }
            return String.Empty;
        }

        public virtual void SetTitleTtfSizeForState(float size, CCControlState state)
        {
            var label = (ICCLabelProtocol) GetTitleLabelForState(state);
            if (label != null)
            {
                var labelTtf = label as CCLabelTTF;
                if (labelTtf != null)
                {
                    labelTtf.FontSize = size;
                }
            }
        }

        public virtual float GetTitleTtfSizeForState(CCControlState state)
        {
            var labelTtf = GetTitleLabelForState(state) as CCLabelTTF;
            if (labelTtf != null)
            {
                return labelTtf.FontSize;
            }
            return 0;
        }

        /**
     * Sets the font of the label, changes the label to a CCLabelBMFont if neccessary.
     * @param fntFile The name of the font to change to
     * @param state The state that uses the specified fntFile. The values are described
     * in "CCControlState".
     */

        public virtual void SetTitleBmFontForState(string fntFile, CCControlState state)
        {
            string title = GetTitleForState(state);
            if (title == null)
            {
                title = String.Empty;
            }
            SetTitleLabelForState(new CCLabelBMFont(title, fntFile), state);
        }

        public virtual string GetTitleBmFontForState(CCControlState state)
        {
            var label = (ICCLabelProtocol) GetTitleLabelForState(state);
            var labelBmFont = label as CCLabelBMFont;
            if (labelBmFont != null)
            {
                return labelBmFont.FntFile;
            }
            return String.Empty;
        }

        /**
    * Returns the background sprite used for a state.
    *
    * @param state The state that uses the background sprite. Possible values are
    * described in "CCControlState".
    */

        public virtual CCNode GetBackgroundSpriteForState(CCControlState state)
        {
            CCNode backgroundSprite;
            if (_backgroundSpriteDispatchTable.TryGetValue(state, out backgroundSprite))
            {
                return backgroundSprite;
            }
            if (_backgroundSpriteDispatchTable.TryGetValue(CCControlState.Normal, out backgroundSprite))
            {
                return backgroundSprite;
            }
            return null;
        }

        /**
        * Sets the background sprite to use for the specified button state.
        *
        * @param sprite The background sprite to use for the specified state.
        * @param state The state that uses the specified image. The values are described
        * in "CCControlState".
        */

        public virtual void SetBackgroundSpriteForState(CCNode sprite, CCControlState state)
        {
            CCSize oldPreferredSize = _preferredSize;

            CCNode previousBackgroundSprite;
            if (_backgroundSpriteDispatchTable.TryGetValue(state, out previousBackgroundSprite))
            {
                RemoveChild(previousBackgroundSprite, true);
                _backgroundSpriteDispatchTable.Remove(state);
            }

            _backgroundSpriteDispatchTable.Add(state, sprite);
            sprite.Visible = false;
            sprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
            
            AddChild(sprite);

            if (_preferredSize.Width != 0 || _preferredSize.Height != 0 && sprite is CCScale9Sprite)
            {
                var scale9 = ((CCScale9Sprite) sprite);

                if (oldPreferredSize.Equals(_preferredSize))
                {
                    // Force update of preferred size
                    scale9.PreferredSize = new CCSize(oldPreferredSize.Width + 1, oldPreferredSize.Height + 1);
                }

                scale9.PreferredSize = _preferredSize;
            }

            // If the current state if equal to the given state we update the layout
            if (State == state)
            {
                NeedsLayout();
            }
        }

        /**
     * Sets the background spriteFrame to use for the specified button state.
     *
     * @param spriteFrame The background spriteFrame to use for the specified state.
     * @param state The state that uses the specified image. The values are described
     * in "CCControlState".
     */

        public virtual void SetBackgroundSpriteFrameForState(CCSpriteFrame spriteFrame, CCControlState state)
        {
            CCScale9Sprite sprite = new CCScale9SpriteFrame(spriteFrame);
            SetBackgroundSpriteForState(sprite, state);
        }
    }
}