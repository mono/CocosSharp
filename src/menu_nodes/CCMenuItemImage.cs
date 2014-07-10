using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCMenuItemImage : CCMenuItem
    {
        CCSprite disabledImage;
        CCSprite normalImage;
        CCSprite selectedImage;
        CCSprite visibleMenuItemSprite;

        CCPoint originalScale;


        #region Properties

        public CCSprite NormalImage
        {
            get { return normalImage; }
            set
            {
                if (value != null && normalImage != value) 
                {
                    normalImage = value;
                    UpdateVisibleMenuItemSprite();
                }
            }
        }

        public CCSpriteFrame NormalImageSpriteFrame
        {
            set { NormalImage = (value == null) ? new CCSprite() : new CCSprite(value.ContentSize, value); }
        }

        public CCSprite SelectedImage
        {
            get { return selectedImage; }
            set
            {
                if (value != null && selectedImage != value) 
                {
                    selectedImage = value;
                    UpdateVisibleMenuItemSprite();
                }
            }
        }

        public CCSpriteFrame SelectedImageSpriteFrame
        {
            set { SelectedImage = (value == null) ? new CCSprite() : new CCSprite(value.ContentSize, value); }
        }

        public CCSprite DisabledImage
        {
            get { return disabledImage; }
            set
            {
                if (value != null && disabledImage != value) 
                {
                    disabledImage = value;
                    UpdateVisibleMenuItemSprite();
                }
            }
        }

        public CCSpriteFrame DisabledImageSpriteFrame
        {
            set { DisabledImage = (value == null) ? new CCSprite() : new CCSprite(value.ContentSize, value); }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                UpdateVisibleMenuItemSprite();
            }
        }

        /// <summary>
        /// Set this to true if you want to zoom-in/out on the button image like the CCMenuItemLabel works.
        /// </summary>
        public bool ZoomBehaviorOnTouch { get; set; }

        public override bool Selected 
        {
            set 
            {
                base.Selected = value;
                UpdateVisibleMenuItemSprite();

                if (Selected && (ZoomActionState == null || ZoomActionState.IsDone)) 
                {
                    originalScale.X = ScaleX;
                    originalScale.Y = ScaleY;
                }

                if (ZoomBehaviorOnTouch) 
                {
                    CCPoint zoomScale = (Selected) ? originalScale * 1.2f : originalScale;
                    CCAction zoomAction = new CCScaleTo(0.1f, zoomScale.X, zoomScale.Y); 

                    if(ZoomActionState !=null)
                    { 
                        ZoomActionState.Stop(); 
                    }

                    ZoomActionState = RunAction(zoomAction);
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCMenuItemImage() : this(new CCSprite())
        {
        }

        public CCMenuItemImage(CCSprite normalSprite, CCSprite selectedSprite, CCSprite disabledSprite, Action<object> target = null) 
            : base(target)
        {
            Debug.Assert(normalSprite != null, "NormalImage cannot be null");

            visibleMenuItemSprite = new CCSprite();

            NormalImage = normalSprite;
            SelectedImage = selectedSprite;
            DisabledImage = disabledSprite;

            originalScale.X = ScaleX;
            originalScale.Y = ScaleY;

            IsColorCascaded = true;
            IsOpacityCascaded = true;
            ZoomBehaviorOnTouch = true;

            AddChild (visibleMenuItemSprite);
        }

        public CCMenuItemImage(CCSprite normalSprite, CCSprite selectedSprite, Action<object> target = null)
            : this(normalSprite, selectedSprite, new CCSprite(), target)
        {
        }
            
        public CCMenuItemImage(CCSprite normalSprite, Action<object> selector = null)
            : this(normalSprite, new CCSprite())
        {
        }

        public CCMenuItemImage(CCSpriteFrame normalSpFrame, CCSpriteFrame selectedSpFrame, CCSpriteFrame disabledSpFrame, Action<object> target = null)
            : this(new CCSprite(normalSpFrame), new CCSprite(selectedSpFrame), new CCSprite(disabledSpFrame), target)
        {
        }

        public CCMenuItemImage(string normalSprite, string selectedSprite, string disabledSprite, Action<object> target = null)
            : this(new CCSprite(normalSprite), new CCSprite(selectedSprite), new CCSprite(disabledSprite), target)
        {
        }

        public CCMenuItemImage(string normalSprite, string selectedSprite, Action<object> target = null)
            : this(new CCSprite(normalSprite), new CCSprite(selectedSprite), target)
        {
        }

        #endregion Constructors

        protected override void UpdateTransform ()
        {
            base.UpdateTransform ();
        }

        public override void Activate()
        {
            if (Enabled)
            {
                if (ZoomBehaviorOnTouch)
                {
                    StopAllActions();
                    ScaleX = originalScale.X;
                    ScaleY = originalScale.Y;
                }
                base.Activate();
            }
        }

        void UpdateVisibleMenuItemSprite()
        {
            if (normalImage == null)
                return;

            CCSprite menuItemSprite = null;

            if(Selected) 
            {
                menuItemSprite = selectedImage;
            }

            if(Enabled == false && menuItemSprite == null) 
            {
                menuItemSprite = disabledImage;
            }

            if (menuItemSprite == null) 
            {
                menuItemSprite = normalImage;
            }

            visibleMenuItemSprite.ContentSize = menuItemSprite.ContentSize;
            visibleMenuItemSprite.Texture = menuItemSprite.Texture;
            visibleMenuItemSprite.TextureRectInPixels = menuItemSprite.TextureRectInPixels;
            visibleMenuItemSprite.IsTextureRectRotated = menuItemSprite.IsTextureRectRotated;

            ContentSize = visibleMenuItemSprite.ContentSize;
        }
    }
}