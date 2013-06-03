using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cocos2D
{
    /** CCNodeRGBA is a subclass of CCNode that implements the CCRGBAProtocol protocol.
 
     All features from CCNode are valid, plus the following new features:
     - opacity
     - RGB colors
 
     Opacity/Color propagates into children that conform to the CCRGBAProtocol if cascadeOpacity/cascadeColor is enabled.
     @since v2.1
     */
    public class CCNodeRGBA : CCNode, ICCRGBAProtocol
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
                    var parent = m_pParent as ICCRGBAProtocol;
                    if (parent != null && parent.CascadeColorEnabled)
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
                    var pParent = m_pParent as ICCRGBAProtocol;
                    if (pParent != null && pParent.CascadeOpacityEnabled)
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

        public virtual bool IsOpacityModifyRGB
        {
            get { return false; }
            set { }
        }

        public virtual bool CascadeColorEnabled
        {
            get { return _cascadeColorEnabled; }
            set { _cascadeColorEnabled = value; }
        }

        public virtual bool CascadeOpacityEnabled
        {
            get { return _cascadeOpacityEnabled; }
            set { _cascadeOpacityEnabled = value; }
        }

        public CCNodeRGBA()
        {
            _displayedOpacity = 255;
            _realOpacity = 255;
            _displayedColor = CCTypes.CCWhite;
            _realColor = CCTypes.CCWhite;
            _cascadeColorEnabled = false;
            _cascadeOpacityEnabled = false;
        }

        public virtual bool Init()
        {
            //base.Init();

            _displayedOpacity = _realOpacity = 255;
            _displayedColor = _realColor = CCTypes.CCWhite;
            _cascadeOpacityEnabled = _cascadeColorEnabled = false;
            
            return true;
        }

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
                        var item = m_pChildren.Elements[i] as ICCRGBAProtocol;
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
                    var item = m_pChildren.Elements[i] as ICCRGBAProtocol;
                    if (item != null)
                    {
                        item.UpdateDisplayedOpacity(_displayedOpacity);
                    }
                }
            }
        }
    }
}
