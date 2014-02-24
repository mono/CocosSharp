using System;

namespace CocosSharp
{
    public class CCMenuItemFont : CCMenuItemLabelTTF
    {
        uint fontSize;
        string fontName;


        #region Properties

        public uint FontSize 
        { 
            get { return fontSize; }
            set 
            {
                fontSize = value;
                RecreateLabel();
            }
        }

        public string FontName 
        { 
            get { return fontName; }
            set 
            {
                fontName = value;
                RecreateLabel();
            }
        }

        #endregion Properties


        #region Constructors

        public CCMenuItemFont (string labelString, string fontNameIn = "arial", uint fontSizeIn = 32, Action<object> selector = null) 
            : base(new CCLabelTTF(labelString, fontNameIn, fontSizeIn), selector)
        {
            fontSize = fontSizeIn;
            fontName = fontNameIn;
        }

        public CCMenuItemFont (string labelString, uint fontSizeIn = 32, Action<object> selector = null) : this(labelString, "arial", fontSizeIn, selector)
        {
        }

        public CCMenuItemFont (string labelString, Action<object> selector = null) : this(labelString, "arial", 32, selector)
        {
        }

        public CCMenuItemFont (string labelString) : this(labelString, "arial", 32, null)
        {
        }

        #endregion Constructors


        protected void RecreateLabel()
        {
            CCLabelTTF label = new CCLabelTTF(LabelTTF.Text, FontName, FontSize);
            LabelTTF = label;
        }
    }
}