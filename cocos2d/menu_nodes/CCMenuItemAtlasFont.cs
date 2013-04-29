
namespace cocos2d.menu_nodes
{
    ///<summary>
    /// A CCMenuItemAtlasFont
    /// Helper class that creates a MenuItemLabel class with a LabelAtlas
    ///</summary>
    public class CCMenuItemAtlasFont : CCMenuItemLabel
    {
        /// <summary>
        /// creates a menu item from a string and atlas with a target/selector
        /// </summary>
        public static CCMenuItemAtlasFont ItemFromString(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap)
        {
            return ItemFromString(value, charMapFile, itemWidth, itemHeight, startCharMap, null, null);
        }

        /// <summary>
        ///  creates a menu item from a string and atlas. Use it with MenuItemToggle
        /// </summary>
        public static CCMenuItemAtlasFont ItemFromString(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap,
                                                         SelectorProtocol target, SEL_MenuHandler selector)
        {
            var pRet = new CCMenuItemAtlasFont();
            pRet.InitFromString(value, charMapFile, itemWidth, itemHeight, startCharMap, target, selector);
            return pRet;
        }

        /// <summary>
        /// initializes a menu item from a string and atlas with a target/selector
        /// </summary>
        public bool InitFromString(string value, string charMapFile, int itemWidth, int itemHeight, char startCharMap, SelectorProtocol target,
                                   SEL_MenuHandler selector)
        {
            // CCAssert( value != NULL && strlen(value) != 0, "value length must be greater than 0");
            var label = new CCLabelAtlas(value, charMapFile, itemWidth, itemHeight, startCharMap);
            base.InitWithLabel(label, selector);
            return true;
        }
    }
}