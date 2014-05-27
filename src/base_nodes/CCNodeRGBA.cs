using System;
using System.Collections.Generic;

namespace CocosSharp
{
    /** CCNodeRGBA is a subclass of CCNode that implements the CCRGBAProtocol protocol.
 
     All features from CCNode are valid, plus the following new features:
     - opacity
     - RGB colors
 
     Opacity/Color propagates into children that conform to the CCRGBAProtocol if cascadeOpacity/cascadeColor is enabled.
     @since v2.1
     */

    public class CCNodeRGBA : CCNode, ICCColor
    {
        // ivars
        byte displayedOpacity;
        CCColor3B displayedColor;

        #region Properties

        public virtual bool IsColorCascaded { get; set; }
        public virtual bool IsOpacityCascaded { get; set; }

        protected CCColor3B RealColor { get; set; }
        protected byte RealOpacity { get; set; }

        public CCColor3B DisplayedColor 
        { 
            get { return displayedColor; } 
        }

        public byte DisplayedOpacity 
        { 
            get { return displayedOpacity; } 
        }

        public virtual CCColor3B Color
        {
            get { return RealColor; }
            set
            {
                displayedColor = RealColor = value;

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
                displayedOpacity = RealOpacity = value;

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

        public virtual bool IsColorModifiedByOpacity
        {
            get { return false; }
            set { }
        }

        #endregion Properties


        #region Constructors

        public CCNodeRGBA()
        {
            displayedOpacity = 255;
            RealOpacity = 255;
            displayedColor = CCColor3B.White;
            RealColor = CCColor3B.White;
            IsColorCascaded = false;
            IsOpacityCascaded = false;
        }

        #endregion Constructors


        public virtual void UpdateDisplayedColor(CCColor3B parentColor)
        {
            displayedColor = RealColor * (parentColor / 255.0f);

            if (IsColorCascaded)
            {
                if (IsOpacityCascaded && Children != null)
                {
                    foreach(CCNode node in Children.Elements)
                    {
                        var item = node as ICCColor;
                        if (item != null)
                        {
                            item.UpdateDisplayedColor(DisplayedColor);
                        }
                    }
                }
            }
        }

        public virtual void UpdateDisplayedOpacity(byte parentOpacity)
        {
            displayedOpacity = (byte) (RealOpacity * parentOpacity / 255.0f);

            if (IsOpacityCascaded && Children != null)
            {
                foreach(CCNode node in Children.Elements)
                {
                    var item = node as ICCColor;
                    if (item != null)
                    {
                        item.UpdateDisplayedOpacity(DisplayedOpacity);
                    }
                }
            }
        }
    }
}
