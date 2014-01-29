using System;

namespace CocosSharp
{
    public class CCMenuItemFont : CCMenuItemLabel
    {
        protected string m_strFontName;
        protected uint m_uFontSize;

        public static uint FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        public static string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }

        public uint FontSizeObj
        {
            set
            {
                m_uFontSize = value;
                RecreateLabel();
            }
            get { return m_uFontSize; }
        }

        public string FontNameObj
        {
            set
            {
                m_strFontName = value;
                RecreateLabel();
            }
            get { return m_strFontName; }
        }


        #region Constructors

        public CCMenuItemFont (string value) : this(value, null)
        { }

        public CCMenuItemFont (string value, Action<object> selector) 
            : base (new CCLabelTTF(value, _fontName, _fontSize), selector)
        {
            m_strFontName = _fontName;
            m_uFontSize = _fontSize;
        }

        #endregion Constructors


        protected void RecreateLabel()
        {
            CCLabelTTF label = new CCLabelTTF((m_pLabel as ICCTextContainer).Text, m_strFontName, m_uFontSize);
            Label = label;
        }
    }
}