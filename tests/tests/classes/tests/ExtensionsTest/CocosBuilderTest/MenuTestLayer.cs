using CocosSharp;

namespace tests.Extensions
{
    internal class MenuTestLayer : BaseLayer
    {
        public CCLabelBMFont mMenuItemStatusLabelBMFont;

        public void onMenuItemAClicked(object pSender)
        {
            mMenuItemStatusLabelBMFont.Text = ("Menu Item A clicked.");
        }

        public void onMenuItemBClicked(object pSender)
        {
            mMenuItemStatusLabelBMFont.Text = ("Menu Item B clicked.");
        }

        public void onMenuItemCClicked(object pSender)
        {
            mMenuItemStatusLabelBMFont.Text = ("Menu Item C clicked.");
        }
    }
}