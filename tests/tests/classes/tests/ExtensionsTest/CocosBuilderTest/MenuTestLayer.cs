using Cocos2D;

namespace tests.Extensions
{
    internal class MenuTestLayer : BaseLayer
    {
        public CCLabelBMFont mMenuItemStatusLabelBMFont;

        public void onMenuItemAClicked(object pSender)
        {
            mMenuItemStatusLabelBMFont.Label = ("Menu Item A clicked.");
        }

        public void onMenuItemBClicked(object pSender)
        {
            mMenuItemStatusLabelBMFont.Label = ("Menu Item B clicked.");
        }

        public void onMenuItemCClicked(object pSender)
        {
            mMenuItemStatusLabelBMFont.Label = ("Menu Item C clicked.");
        }
    }
}