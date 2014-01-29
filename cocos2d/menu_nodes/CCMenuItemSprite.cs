using System;

namespace CocosSharp
{
    public class CCMenuItemSprite : CCMenuItem
    {
        protected float m_fOriginalScale;
        private CCNode m_pDisabledImage;
        private CCNode m_pNormalImage;

        private CCNode m_pSelectedImage;

        public CCNode NormalImage
        {
            get { return m_pNormalImage; }
            set
            {
                if (value != null)
                {
                    AddChild(value);
                    value.AnchorPoint = new CCPoint(0, 0);
                    ContentSize = value.ContentSize;
                }

                if (m_pNormalImage != null)
                {
                    RemoveChild(m_pNormalImage, true);
                }

                m_pNormalImage = value;
                UpdateImagesVisibility();
            }
        }

        public CCNode SelectedImage
        {
            get { return m_pSelectedImage; }
            set
            {
                if (value != null)
                {
                    AddChild(value);
                    value.AnchorPoint = new CCPoint(0, 0);
                }

                if (m_pSelectedImage != null)
                {
                    RemoveChild(m_pSelectedImage, true);
                }

                m_pSelectedImage = value;
                UpdateImagesVisibility();
            }
        }

        public CCNode DisabledImage
        {
            get { return m_pDisabledImage; }
            set
            {
                if (value != null)
                {
                    AddChild(value);
                    value.AnchorPoint = new CCPoint(0, 0);
                }

                if (m_pDisabledImage != null)
                {
                    RemoveChild(m_pDisabledImage, true);
                }

                m_pDisabledImage = value;
                UpdateImagesVisibility();
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                UpdateImagesVisibility();
            }
        }

        /// <summary>
        /// Set this to true if you want to zoom-in/out on the button image like the CCMenuItemLabel works.
        /// </summary>
        public bool ZoomBehaviorOnTouch { get; set; }


        #region Constructors

        public CCMenuItemSprite()
            : this(null, null, null, null)
        {
            ZoomBehaviorOnTouch = false;
        }

		public CCMenuItemSprite(Action<object> selector)
            : base(selector)
        {
        }

		public CCMenuItemSprite(string normalSprite, string selectedSprite, Action<object> selector)
            : this(new CCSprite(normalSprite), new CCSprite(selectedSprite), null, selector)
        {
        }

        public CCMenuItemSprite(CCNode normalSprite, CCNode selectedSprite)
            : this(normalSprite, selectedSprite, null, null)
        {
        }

		public CCMenuItemSprite(CCNode normalSprite, CCNode selectedSprite, Action<object> selector)
            : this(normalSprite, selectedSprite, null, selector)
        {
        }

		public CCMenuItemSprite(CCNode normalSprite, CCNode selectedSprite, CCNode disabledSprite, Action<object> selector) 
			: base(selector)
        {
            NormalImage = normalSprite;
            SelectedImage = selectedSprite;
            DisabledImage = disabledSprite;

            if (m_pNormalImage != null)
            {
                ContentSize = m_pNormalImage.ContentSize;
            }

            CascadeColorEnabled = true;
            CascadeOpacityEnabled = true;
        }

        #endregion Constructors


        public override void Selected()
        {
            base.Selected();

            if (m_pNormalImage != null)
            {
                if (m_pDisabledImage != null)
                {
                    m_pDisabledImage.Visible = false;
                }

                if (m_pSelectedImage != null)
                {
                    m_pNormalImage.Visible = false;
                    m_pSelectedImage.Visible = true;
                }
                else
                {
                    m_pNormalImage.Visible = true;
                    if (ZoomBehaviorOnTouch)
                    {
                        CCAction action = GetActionByTag(unchecked((int)kZoomActionTag));
                        if (action != null)
                        {
                            StopAction(action);
                        }
                        else
                        {
                            m_fOriginalScale = Scale;
                        }

                        CCAction zoomAction = new CCScaleTo(0.1f, m_fOriginalScale * 1.2f);
                        zoomAction.Tag = unchecked((int)kZoomActionTag);
                        RunAction(zoomAction);
                    }
                }
            }
        }

        public override void Unselected()
        {
            base.Unselected();
            if (m_pNormalImage != null)
            {
                m_pNormalImage.Visible = true;

                if (m_pSelectedImage != null)
                {
                    m_pSelectedImage.Visible = false;
                    if (ZoomBehaviorOnTouch)
                    {
                        StopActionByTag(unchecked((int)kZoomActionTag));
                        CCAction zoomAction = new CCScaleTo(0.1f, m_fOriginalScale);
                        zoomAction.Tag = unchecked((int)kZoomActionTag);
                        RunAction(zoomAction);
                    }
                }

                if (m_pDisabledImage != null)
                {
                    m_pDisabledImage.Visible = false;
                }
            }
        }

        public override void Activate()
        {
            if (m_bIsEnabled)
            {
                if (ZoomBehaviorOnTouch)
                {
                    StopAllActions();
                    Scale = m_fOriginalScale;
                }
                base.Activate();
            }
        }

        // Helper 
        private void UpdateImagesVisibility()
        {
            if (m_bIsEnabled)
            {
                if (m_pNormalImage != null) m_pNormalImage.Visible = true;
                if (m_pSelectedImage != null) m_pSelectedImage.Visible = false;
                if (m_pDisabledImage != null) m_pDisabledImage.Visible = false;
            }
            else
            {
                if (m_pDisabledImage != null)
                {
                    if (m_pNormalImage != null) m_pNormalImage.Visible = false;
                    if (m_pSelectedImage != null) m_pSelectedImage.Visible = false;
                    if (m_pDisabledImage != null) m_pDisabledImage.Visible = true;
                }
                else
                {
                    if (m_pNormalImage != null) m_pNormalImage.Visible = true;
                    if (m_pSelectedImage != null) m_pSelectedImage.Visible = false;
                    if (m_pDisabledImage != null) m_pDisabledImage.Visible = false;
                }
            }
        }
    }
}