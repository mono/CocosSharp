using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCLayerRGBA : CCLayer, ICCColor
    {
        protected byte _displayedOpacity;
        protected byte _realOpacity;
        protected CCColor3B _displayedColor;
        protected CCColor3B _realColor;
        protected bool _cascadeColorEnabled;
        protected bool _cascadeOpacityEnabled;

        public virtual CCColor3B Color
        {
            get { return _realColor; }
            set
            {
                _displayedColor = _realColor = value;

                if (_cascadeColorEnabled)
                {
                    var parentColor = CCTypes.CCWhite;
                    var parent = m_pParent as ICCColor;
                    if (parent != null && parent.IsColorCascaded)
                    {
                        parentColor = parent.DisplayedColor;
                    }

                    UpdateDisplayedColor(parentColor);
                }
            }
        }

        public virtual CCColor3B DisplayedColor
        {
            get { return _displayedColor; }
        }

        public virtual byte Opacity
        {
            get { return _realOpacity; }
            set
            {
                _displayedOpacity = _realOpacity = value;

                if (_cascadeOpacityEnabled)
                {
                    byte parentOpacity = 255;
                    var pParent = m_pParent as ICCColor;
                    if (pParent != null && pParent.IsOpacityCascaded)
                    {
                        parentOpacity = pParent.DisplayedOpacity;
                    }
                    UpdateDisplayedOpacity(parentOpacity);
                }
            }
        }

        public virtual byte DisplayedOpacity
        {
            get { return _displayedOpacity; }
        }

        public virtual bool IsColorModifiedByOpacity
        {
            get { return false; }
            set { }
        }
                            
        public virtual bool IsColorCascaded
        {
            get { return _cascadeColorEnabled; }
            set { _cascadeColorEnabled = value; }
        }

        public virtual bool IsOpacityCascaded
        {
            get { return _cascadeOpacityEnabled; }
            set { _cascadeOpacityEnabled = value; }
        }


        #region Constructors

        public CCLayerRGBA() : base()
        {
            _displayedOpacity = 255;
            _realOpacity = 255;
            _displayedColor = CCTypes.CCWhite;
            _realColor = CCTypes.CCWhite;
            
            _cascadeColorEnabled = false;
            _cascadeOpacityEnabled = false;
        }

        #endregion Constructors


        public virtual void UpdateDisplayedColor(CCColor3B parentColor)
        {
            _displayedColor.R = (byte) (_realColor.R * parentColor.R / 255.0f);
            _displayedColor.G = (byte) (_realColor.G * parentColor.G / 255.0f);
            _displayedColor.B = (byte) (_realColor.B * parentColor.B / 255.0f);

            if (_cascadeColorEnabled)
            {
                if (_cascadeOpacityEnabled && m_pChildren != null)
                {
                    for (int i = 0, count = m_pChildren.count; i < count; i++)
                    {
                        var item = m_pChildren.Elements[i] as ICCColor;
                        if (item != null)
                        {
                            item.UpdateDisplayedColor(_displayedColor);
                        }
                    }
                }
            }
        }

        public virtual void UpdateDisplayedOpacity(byte parentOpacity)
        {
            _displayedOpacity = (byte) (_realOpacity * parentOpacity / 255.0f);

            if (_cascadeOpacityEnabled && m_pChildren != null)
            {
                for (int i = 0, count = m_pChildren.count; i < count; i++)
                {
                    var item = m_pChildren.Elements[i] as ICCColor;
                    if (item != null)
                    {
                        item.UpdateDisplayedOpacity(_displayedOpacity);
                    }
                }
            }
        }
    }
}

