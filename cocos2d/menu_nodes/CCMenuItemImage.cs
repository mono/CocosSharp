using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCMenuItemImage : CCMenuItem
    {
        // To avoid continuously checking for null when selecting/enabling, we set the CCSprites to default values
        CCSprite disabledImage = new CCSprite();
        CCSprite normalImage = new CCSprite();
        CCSprite selectedImage = new CCSprite();


        #region Properties

        protected float OriginalScale { get; private set; }

        public CCSprite NormalImage
        {
            get { return normalImage; }
            set
            {
                if (value == null) 
                {
                    value = new CCSprite();
                }

                AddChild(value);
                value.AnchorPoint = new CCPoint(0, 0);
                ContentSize = value.ContentSize;

                if (normalImage != null)
                {
                    RemoveChild(normalImage, true);
                }

                normalImage = value;
                UpdateImagesVisibility();
            }
        }

        public CCSpriteFrame NormalImageSpriteFrame
        {
            set { NormalImage = (value == null) ? new CCSprite() : new CCSprite(value); }
        }

        public CCSprite SelectedImage
        {
            get { return selectedImage; }
            set
            {
                if (value == null) 
                {
                    value = new CCSprite(NormalImage.Texture);
                }

                AddChild(value);
                value.AnchorPoint = new CCPoint(0, 0);

                if (selectedImage != null)
                {
                    RemoveChild(selectedImage, true);
                }

                selectedImage = value;
                UpdateImagesVisibility();
            }
        }

        public CCSpriteFrame SelectedImageSpriteFrame
        {
            set { SelectedImage = (value == null) ? new CCSprite() : new CCSprite(value); }
        }

        public CCSprite DisabledImage
        {
            get { return disabledImage; }
            set
            {
                if (value == null) 
                {
                    value = new CCSprite(NormalImage.Texture);
                }

                AddChild(value);
                value.AnchorPoint = new CCPoint(0, 0);

                if (disabledImage != null)
                {
                    RemoveChild(disabledImage, true);
                }

                disabledImage = value;
                UpdateImagesVisibility();
            }
        }

        public CCSpriteFrame DisabledImageSpriteFrame
        {
            set { DisabledImage = (value == null) ? new CCSprite() : new CCSprite(value); }
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

        public override bool Selected 
        {
            set 
            {
                base.Selected = value;
                UpdateImagesVisibility();

                if (Selected && (ZoomActionState == null || ZoomActionState.IsDone)) 
                {
                    OriginalScale = Scale;
                }

                if (ZoomBehaviorOnTouch) 
                {
                    float zoomScale = (Selected) ? OriginalScale * 1.2f : OriginalScale;
                    CCAction zoomAction = new CCScaleTo(0.1f, zoomScale); 

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

        // Used for menu item image loader
        public CCMenuItemImage() : this(new CCSprite())
        {
        }

        public CCMenuItemImage(CCSprite normalSprite, CCSprite selectedSprite, CCSprite disabledSprite, Action<object> target = null) 
            : base(target)
        {
            Debug.Assert(normalSprite != null, "NormalImage cannot be null");

            NormalImage = normalSprite;
            SelectedImage = selectedSprite;
            DisabledImage = disabledSprite;

            OriginalScale = Scale;

            ContentSize = NormalImage.ContentSize;

            CascadeColorEnabled = true;
            CascadeOpacityEnabled = true;
            ZoomBehaviorOnTouch = true;
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


        public override void Activate()
        {
            if (Enabled)
            {
                if (ZoomBehaviorOnTouch)
                {
                    StopAllActions();
                    Scale = OriginalScale;
                }
                base.Activate();
            }
        }

        void UpdateImagesVisibility()
        {
            if (Enabled)
            {
                disabledImage.Visible = false;

                if (Selected) 
                {
                    normalImage.Visible = false; 
                    selectedImage.Visible = true;
                }
                else
                {
                    normalImage.Visible = true;
                    selectedImage.Visible = false;
                }
            }
            else
            {
                disabledImage.Visible = true;
                normalImage.Visible = false;
                selectedImage.Visible = false;
            }
        }
    }
}