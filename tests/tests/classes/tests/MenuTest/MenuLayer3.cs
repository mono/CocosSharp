/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2009 Jason Booth
Copyright (c) 2011-2012 openxlive.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class MenuLayer3 : CCLayer
    {
        protected CCMenuItem m_disabledItem;

        string s_MenuItem = "Images/menuitemsprite";
        public MenuLayer3()
        {
			var label = new CCLabelBMFont("Enable AtlasItem", "fonts/bitmapFontTest3.fnt");
            var item1 = new CCMenuItemLabelBMFont(label, this.menuCallback2);

			CCMenuItemFont.FontSize = 28;
			CCMenuItemFont.FontName = "arial";

            var item2 = new CCMenuItemFont("--- Go Back ---", this.menuCallback);

			// We do not have an HD version of the menuitemsprite so internally CocosSharp tries to convert our
			// rectangle coordinates passed to work with HD images so the coordinates are off.  We will just 
			// modify this here to make sure we have the correct sizes when they are passed.
			var spriteNormal = new CCSprite(s_MenuItem, new CCRect(0, 23 * 2, 115, 23).PixelsToPoints());
			var spriteSelected = new CCSprite(s_MenuItem, new CCRect(0, 23 * 1, 115, 23).PixelsToPoints());
			var spriteDisabled = new CCSprite(s_MenuItem, new CCRect(0, 23 * 0, 115, 23).PixelsToPoints());


            var item3 = new CCMenuItemImage(spriteNormal, spriteSelected, spriteDisabled, this.menuCallback3);
            m_disabledItem = item3;
            m_disabledItem.Enabled = false;

			var menu = new CCMenu(item1, item2, item3);
            menu.Position = new CCPoint(0, 0);

			var s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

            item1.Position = new CCPoint(s.Width / 2 - 150, s.Height / 2);
            item2.Position = new CCPoint(s.Width / 2 - 200, s.Height / 2);
            item3.Position = new CCPoint(s.Width / 2, s.Height / 2 - 100);

			var jump = new CCJumpBy (3, new CCPoint(400, 0), 50, 4);
			item2.RepeatForever(jump, jump.Reverse());

			var spin1 = new CCRotateBy (3, 360);

			item1.RepeatForever(spin1);
			item2.RepeatForever(spin1);
			item3.RepeatForever(spin1);

            AddChild(menu);
        }
        public void menuCallback(object pSender)
        {
            ((CCLayerMultiplex)Parent).SwitchTo(0);
        }
        public void menuCallback2(object pSender)
        {
            //UXLOG("Label clicked. Toogling AtlasSprite");
            m_disabledItem.Enabled = !m_disabledItem.Enabled;
            m_disabledItem.StopAllActions();
        }
        public void menuCallback3(object pSender)
        {
            //UXLOG("MenuItemSprite clicked");
        }
    }
}
