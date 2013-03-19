using cocos2d;

namespace tests.Extensions
{
    internal class MenuTestLayer : BaseLayer
    {
        public CCLabelBMFont mMenuItemStatusLabelBMFont;

        public void onMenuItemAClicked(CCObject pSender)
        {
            mMenuItemStatusLabelBMFont.Label = ("Menu Item A clicked.");
        }

        public void onMenuItemBClicked(CCObject pSender)
        {
            mMenuItemStatusLabelBMFont.Label = ("Menu Item B clicked.");
        }

        public void onMenuItemCClicked(CCObject pSender)
        {
            mMenuItemStatusLabelBMFont.Label = ("Menu Item C clicked.");
        }
    }
}