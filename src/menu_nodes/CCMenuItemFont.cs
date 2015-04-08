using System;

namespace CocosSharp
{
    public class CCMenuItemFont : CCMenuItemLabel
    {

        #region Properties

        public static uint FontSize { get; set; }
        public static string FontName { get; set; }

        #endregion Properties


        #region Constructors

        public CCMenuItemFont (string labelString, Action<object> selector = null) 
            : base(new CCLabel(labelString, FontName, FontSize), selector)
        {
        }

        #endregion Constructors

    }
}