using System;

namespace CocosSharp
{
    ///<summary>
    /// A CCMenuItemAtlasFont
    /// Helper class that creates a MenuItemLabel class with a LabelAtlas
    ///</summary>
    public class CCMenuItemAtlasFont : CCMenuItemLabel
    {
        #region Constructors

        /// <summary>
        /// creates a menu item from a string and atlas with a target/selector
        /// </summary>
        public CCMenuItemAtlasFont(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap) 
            : this(value, charMapFile, itemWidth, itemHeight, startCharMap, null, null)
        {
        }

        /// <summary>
        ///  creates a menu item from a string and atlas. Use it with MenuItemToggle
        /// </summary>
        public CCMenuItemAtlasFont(string value, string charMapFile, int itemWidth, int itemHeight, 
            char startCharMap, ICCUpdatable target, Action<object> selector)
        {
            InitCCMenuItemAtlasFont(value, charMapFile, itemWidth, itemHeight, startCharMap, target, selector);
        }

        private void InitCCMenuItemAtlasFont(string value, string charMapFile, int itemWidth, int itemHeight, 
            char startCharMap, ICCUpdatable target, Action<object> selector)
        {
            // CCAssert( value != NULL && strlen(value) != 0, "value length must be greater than 0");
            var label = new CCLabelAtlas(value, charMapFile, itemWidth, itemHeight, startCharMap);
			base.Label = label;
			base.Target = selector;
        }

        #endregion Constructors

    }
}