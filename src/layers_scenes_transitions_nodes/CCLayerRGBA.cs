using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCLayerRGBA : CCLayer, ICCColor
    {
        #region Properties

        public virtual bool IsColorCascaded { get; set; }
        public virtual bool IsOpacityCascaded { get; set; }

        public virtual byte DisplayedOpacity { get; protected set; }
        protected byte RealOpacity { get; set; }

        public virtual CCColor3B DisplayedColor { get; protected set; }
        protected CCColor3B RealColor { get; set; }

        public virtual bool IsColorModifiedByOpacity
        {
            get { return false; }
            set { }
        }

        public virtual CCColor3B Color
        {
            get { return RealColor; }
            set
            {
                DisplayedColor = RealColor = value;

                if (IsColorCascaded)
                {
                    var parentColor = CCColor3B.White;
                    var parent = Parent as ICCColor;
                    if (parent != null && parent.IsColorCascaded)
                    {
                        parentColor = parent.DisplayedColor;
                    }

                    UpdateDisplayedColor(parentColor);
                }
            }
        }

        public virtual byte Opacity
        {
            get { return RealOpacity; }
            set
            {
                DisplayedOpacity = RealOpacity = value;

                if (IsOpacityCascaded)
                {
                    byte parentOpacity = 255;
                    var pParent = Parent as ICCColor;
                    if (pParent != null && pParent.IsOpacityCascaded)
                    {
                        parentOpacity = pParent.DisplayedOpacity;
                    }
                    UpdateDisplayedOpacity(parentOpacity);
                }
            }
        }

        #endregion Properties


        #region Constructors

        public CCLayerRGBA() : base()
        {
            DisplayedOpacity = 255;
            RealOpacity = 255;
            DisplayedColor = CCColor3B.White;
            RealColor = CCColor3B.White;
        }

        #endregion Constructors


        public virtual void UpdateDisplayedColor(CCColor3B parentColor)
        {
            DisplayedColor = RealColor * (parentColor / 255.0f);

            if (IsColorCascaded)
            {
                if (IsOpacityCascaded && Children != null)
                {
                    foreach(CCNode child in Children.Elements)
                    {
                        ICCColor colorChild = child as ICCColor;
                        if (colorChild != null)
                        {
                            colorChild.UpdateDisplayedColor(DisplayedColor);
                        }
                    }
                }
            }
        }

        public virtual void UpdateDisplayedOpacity(byte parentOpacity)
        {
            DisplayedOpacity = (byte) (RealOpacity * (parentOpacity / 255.0f));

            if (IsOpacityCascaded && Children != null)
            {
                foreach(CCNode child in Children.Elements)
                {
                    ICCColor colorChild = child as ICCColor;
                    if (colorChild != null)
                    {
                        colorChild.UpdateDisplayedOpacity(DisplayedOpacity);
                    }
                }
            }
        }

    }
}

