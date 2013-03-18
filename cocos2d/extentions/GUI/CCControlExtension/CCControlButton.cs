using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace cocos2d
{
    public class CCControlButton : CCControl
    {
        /* Define the button margin for Left/Right edge */
        public const int CCControlButtonMarginLR = 8; // px
        /* Define the button margin for Top/Bottom edge */
        public const int CCControlButtonMarginTB = 2; // px

        public const int kZoomActionTag = (0x7CCB0001);
        protected bool m_bParentInited;

        protected CCScale9Sprite m_backgroundSprite;
        protected Dictionary<CCControlState, CCScale9Sprite> m_backgroundSpriteDispatchTable;
        protected string m_currentTitle;

        /** The current color used to display the title. */
        protected CCColor3B m_currentTitleColor;
        protected bool m_doesAdjustBackgroundImage;
        protected bool m_isPushed;
        protected CCPoint m_labelAnchorPoint;
        protected int m_marginH;
        protected int m_marginV;
        protected CCSize m_preferredSize;
        protected Dictionary<CCControlState, CCColor3B> m_titleColorDispatchTable;
        protected Dictionary<CCControlState, string> m_titleDispatchTable;
        protected CCNode m_titleLabel;
        protected Dictionary<CCControlState, CCNode> m_titleLabelDispatchTable;
        protected bool m_zoomOnTouchDown;


        public CCScale9Sprite BackgroundSprite
        {
            get { return m_backgroundSprite; }
            set { m_backgroundSprite = value; }
        }

        public CCNode TitleLabel
        {
            get { return m_titleLabel; }
            set { m_titleLabel = value; }
        }

        public override byte Opacity
        {
            get { return base.Opacity; }
            set
            {
                base.Opacity = value;
                if (m_pChildren != null)
                {
                    for (int i = 0; i < m_pChildren.count; i++)
                    {
                        var node = m_pChildren.Elements[i] as ICCRGBAProtocol;
                        if (node != null)
                        {
                            node.Opacity = value;
                        }
                    }
                }

                foreach (CCScale9Sprite item in m_backgroundSpriteDispatchTable.Values)
                {
                    item.Opacity = value;
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

                if (m_zoomOnTouchDown)
                {
                    float scaleValue = (Highlighted && Enabled && !Selected) ? 1.1f : 1.0f;
                    CCAction zoomAction = CCScaleTo.Create(0.05f, scaleValue);
                    zoomAction.Tag = kZoomActionTag;
                    RunAction(zoomAction);
                }
            }
        }

        public bool IsPushed
        {
            get { return m_isPushed; }
        }

        public override void NeedsLayout()
        {
            if (!m_bParentInited)
            {
                return;
            }
            // Hide the background and the label
            if (m_titleLabel != null)
            {
                m_titleLabel.Visible = false;
            }
            if (m_backgroundSprite != null)
            {
                m_backgroundSprite.Visible = false;
            }
            // Update anchor of all labels
            LabelAnchorPoint = m_labelAnchorPoint;

            // Update the label to match with the current state

            m_currentTitle = GetTitleForState(m_eState);
            m_currentTitleColor = GetTitleColorForState(m_eState);

            TitleLabel = GetTitleLabelForState(m_eState);

            var label = (ICCLabelProtocol) m_titleLabel;
            if (label != null && !String.IsNullOrEmpty(m_currentTitle))
            {
                label.String = (m_currentTitle);
            }

            var rgbaLabel = (ICCRGBAProtocol) m_titleLabel;
            if (rgbaLabel != null)
            {
                rgbaLabel.Color = m_currentTitleColor;
            }
            if (m_titleLabel != null)
            {
                m_titleLabel.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
            }

            // Update the background sprite
            BackgroundSprite = GetBackgroundSpriteForState(m_eState);
            if (m_backgroundSprite != null)
            {
                m_backgroundSprite.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
            }

            // Get the title label size
            CCSize titleLabelSize = CCSize.Zero;
            if (m_titleLabel != null)
            {
                titleLabelSize = m_titleLabel.BoundingBox.Size;
            }

            // Adjust the background image if necessary
            if (m_doesAdjustBackgroundImage)
            {
                // Add the margins
                if (m_backgroundSprite != null)
                {
                    m_backgroundSprite.ContentSize = new CCSize(titleLabelSize.Width + m_marginH * 2, titleLabelSize.Height + m_marginV * 2);
                }
            }
            else
            {
                //TODO: should this also have margins if one of the preferred sizes is relaxed?
                if (m_backgroundSprite != null)
                {
                    CCSize preferredSize = m_backgroundSprite.PreferredSize;
                    if (preferredSize.Width <= 0)
                    {
                        preferredSize.Width = titleLabelSize.Width;
                    }
                    if (preferredSize.Height <= 0)
                    {
                        preferredSize.Height = titleLabelSize.Height;
                    }

                    m_backgroundSprite.ContentSize = preferredSize;
                }
            }

            // Set the content size
            CCRect rectTitle = CCRect.Zero;
            if (m_titleLabel != null)
            {
                rectTitle = m_titleLabel.BoundingBox;
            }
            CCRect rectBackground = CCRect.Zero;
            if (m_backgroundSprite != null)
            {
                rectBackground = m_backgroundSprite.BoundingBox;
            }

            CCRect maxRect = CCControlUtils.CCRectUnion(rectTitle, rectBackground);
            ContentSize = new CCSize(maxRect.Size.Width, maxRect.Size.Height);

            if (m_titleLabel != null)
            {
                m_titleLabel.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
                // Make visible label
                m_titleLabel.Visible = true;
            }

            if (m_backgroundSprite != null)
            {
                m_backgroundSprite.Position = new CCPoint(ContentSize.Width / 2, ContentSize.Height / 2);
                // Make visible the background
                m_backgroundSprite.Visible = true;
            }
        }

        /** Adjust the background image. YES by default. If the property is set to NO, the 
        background will use the prefered size of the background image. */

        public void SetAdjustBackgroundImage(bool adjustBackgroundImage)
        {
            m_doesAdjustBackgroundImage = adjustBackgroundImage;
            NeedsLayout();
        }

        public bool DoesAdjustBackgroundImage()
        {
            return m_doesAdjustBackgroundImage;
        }


        /** Adjust the button zooming on touchdown. Default value is YES. */

        public bool ZoomOnTouchDown
        {
            set { m_zoomOnTouchDown = value; }
            get { return m_zoomOnTouchDown; }
        }

        /** The prefered size of the button, if label is larger it will be expanded. */

        public CCSize PreferredSize
        {
            get { return m_preferredSize; }
            set
            {
                if (value.Width == 0 && value.Height == 0)
                {
                    m_doesAdjustBackgroundImage = true;
                }
                else
                {
                    m_doesAdjustBackgroundImage = false;
                    foreach (var item in m_backgroundSpriteDispatchTable)
                    {
                        CCScale9Sprite sprite = item.Value;
                        sprite.PreferredSize = value;
                    }
                }

                m_preferredSize = value;

                NeedsLayout();
            }
        }


        public CCPoint LabelAnchorPoint
        {
            get { return m_labelAnchorPoint; }
            set
            {
                m_labelAnchorPoint = value;
                if (m_titleLabel != null)
                {
                    m_titleLabel.AnchorPoint = value;
                }
            }
        }

        /** The current title that is displayed on the button. */

        //set the margins at once (so we only have to do one call of needsLayout)
        protected virtual void SetMargins(int marginH, int marginV)
        {
            m_marginV = marginV;
            m_marginH = marginH;
            NeedsLayout();
        }

        public override bool Init()
        {
            return InitWithLabelAndBackgroundSprite(CCLabelTTF.Create("", "Helvetica", 12), CCScale9Sprite.Create());
        }

        public virtual bool InitWithLabelAndBackgroundSprite(CCNode node, CCScale9Sprite backgroundSprite)
        {
            if (base.Init())
            {
                Debug.Assert(node != null, "Label must not be nil.");
                var label = node as ICCLabelProtocol;
                var rgbaLabel = node as ICCRGBAProtocol;
                Debug.Assert(backgroundSprite != null, "Background sprite must not be nil.");
                Debug.Assert(label != null || rgbaLabel != null || backgroundSprite != null);

                m_bParentInited = true;

                // Initialize the button state tables
                m_titleDispatchTable = new Dictionary<CCControlState, string>();
                m_titleColorDispatchTable = new Dictionary<CCControlState, CCColor3B>();
                m_titleLabelDispatchTable = new Dictionary<CCControlState, CCNode>();
                m_backgroundSpriteDispatchTable = new Dictionary<CCControlState, CCScale9Sprite>();

                TouchEnabled = true;
                m_isPushed = false;
                m_zoomOnTouchDown = true;

                m_currentTitle = null;

                // Adjust the background image by default
                SetAdjustBackgroundImage(true);
                PreferredSize = CCSize.Zero;
                // Zooming button by default
                m_zoomOnTouchDown = true;

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

                string tempString = label.String;
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

        public static CCControlButton Create(CCNode label, CCScale9Sprite backgroundSprite)
        {
            var pRet = new CCControlButton();
            pRet.InitWithLabelAndBackgroundSprite(label, backgroundSprite);
            return pRet;
        }

        public virtual bool InitWithTitleAndFontNameAndFontSize(string title, string fontName, float fontSize)
        {
            CCLabelTTF label = CCLabelTTF.Create(title, fontName, fontSize);
            return InitWithLabelAndBackgroundSprite(label, CCScale9Sprite.Create());
        }

        public static CCControlButton Create(string title, string fontName, float fontSize)
        {
            var pRet = new CCControlButton();
            pRet.InitWithTitleAndFontNameAndFontSize(title, fontName, fontSize);
            return pRet;
        }

        public virtual bool InitWithBackgroundSprite(CCScale9Sprite sprite)
        {
            CCLabelTTF label = CCLabelTTF.Create("", "arial", 30);
            return InitWithLabelAndBackgroundSprite(label, sprite);
        }

        public static CCControlButton Create(CCScale9Sprite sprite)
        {
            var pRet = new CCControlButton();
            pRet.InitWithBackgroundSprite(sprite);
            return pRet;
        }

        //events
        public override bool TouchBegan(CCTouch pTouch, CCEvent pEvent)
        {
            if (!IsTouchInside(pTouch) || !Enabled)
            {
                return false;
            }

            m_eState = CCControlState.Highlighted;
            m_isPushed = true;
            Highlighted = true;
            SendActionsForControlEvents(CCControlEvent.TouchDown);
            return true;
        }

        public override void TouchMoved(CCTouch pTouch, CCEvent pEvent)
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
            if (isTouchMoveInside && !m_bHighlighted)
            {
                m_eState = CCControlState.Highlighted;
                Highlighted = true;
                SendActionsForControlEvents(CCControlEvent.TouchDragEnter);
            }
            else if (isTouchMoveInside && Highlighted)
            {
                SendActionsForControlEvents(CCControlEvent.TouchDragInside);
            }
            else if (!isTouchMoveInside && Highlighted)
            {
                m_eState = CCControlState.Normal;
                Highlighted = false;

                SendActionsForControlEvents(CCControlEvent.TouchDragExit);
            }
            else if (!isTouchMoveInside && !Highlighted)
            {
                SendActionsForControlEvents(CCControlEvent.TouchDragOutside);
            }
        }

        public override void TouchEnded(CCTouch pTouch, CCEvent pEvent)
        {
            m_eState = CCControlState.Normal;
            m_isPushed = false;
            Highlighted = false;


            if (IsTouchInside(pTouch))
            {
                SendActionsForControlEvents(CCControlEvent.TouchUpInside);
            }
            else
            {
                SendActionsForControlEvents(CCControlEvent.TouchUpOutside);
            }
        }

        public override void TouchCancelled(CCTouch pTouch, CCEvent pEvent)
        {
            m_eState = CCControlState.Normal;
            m_isPushed = false;
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
            if (m_titleDispatchTable != null)
            {
                string title;
                if (m_titleDispatchTable.TryGetValue(state, out title))
                {
                    return title;
                }
                if (m_titleDispatchTable.TryGetValue(CCControlState.Normal, out title))
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
            if (m_titleDispatchTable.ContainsKey(state))
            {
                m_titleDispatchTable.Remove(state);
            }

            if (!String.IsNullOrEmpty(title))
            {
                m_titleDispatchTable.Add(state, title);
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
            if (m_titleColorDispatchTable != null)
            {
                CCColor3B color;

                if (m_titleColorDispatchTable.TryGetValue(state, out color))
                {
                    return color;
                }

                if (m_titleColorDispatchTable.TryGetValue(CCControlState.Normal, out color))
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
            if (m_titleColorDispatchTable.ContainsKey(state))
            {
                m_titleColorDispatchTable.Remove(state);
            }

            m_titleColorDispatchTable.Add(state, color);

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
            if (m_titleLabelDispatchTable.TryGetValue(state, out titleLabel))
            {
                return titleLabel;
            }
            if (m_titleLabelDispatchTable.TryGetValue(CCControlState.Normal, out titleLabel))
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
            if (m_titleLabelDispatchTable.TryGetValue(state, out previousLabel))
            {
                RemoveChild(previousLabel, true);
                m_titleLabelDispatchTable.Remove(state);
            }

            m_titleLabelDispatchTable.Add(state, titleLabel);
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
            SetTitleLabelForState(CCLabelTTF.Create(title, fntFile, 12), state);
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
            SetTitleLabelForState(CCLabelBMFont.Create(title, fntFile), state);
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

        public virtual CCScale9Sprite GetBackgroundSpriteForState(CCControlState state)
        {
            CCScale9Sprite backgroundSprite;
            if (m_backgroundSpriteDispatchTable.TryGetValue(state, out backgroundSprite))
            {
                return backgroundSprite;
            }
            if (m_backgroundSpriteDispatchTable.TryGetValue(CCControlState.Normal, out backgroundSprite))
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

        public virtual void SetBackgroundSpriteForState(CCScale9Sprite sprite, CCControlState state)
        {
            CCSize oldPreferredSize = m_preferredSize;

            CCScale9Sprite previousBackgroundSprite;
            if (m_backgroundSpriteDispatchTable.TryGetValue(state, out previousBackgroundSprite))
            {
                RemoveChild(previousBackgroundSprite, true);
                m_backgroundSpriteDispatchTable.Remove(state);
            }

            m_backgroundSpriteDispatchTable.Add(state, sprite);
            sprite.Visible = false;
            sprite.AnchorPoint = new CCPoint(0.5f, 0.5f);
            AddChild(sprite);

            if (m_preferredSize.Width != 0 || m_preferredSize.Height != 0)
            {
                if (oldPreferredSize.Equals(m_preferredSize))
                {
                    // Force update of preferred size
                    sprite.PreferredSize = new CCSize(oldPreferredSize.Width + 1, oldPreferredSize.Height + 1);
                }

                sprite.PreferredSize = m_preferredSize;
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
            CCScale9Sprite sprite = CCScale9Sprite.CreateWithSpriteFrame(spriteFrame);
            SetBackgroundSpriteForState(sprite, state);
        }

        /// <summary>
        /// Default ctor. Does nothing.
        /// </summary>
        public CCControlButton()
        {
        }

        [Obsolete("Use the default ctor")]
        public new static CCControlButton Create()
        {
            var pControlButton = new CCControlButton();
            return pControlButton;
        }
    }
}