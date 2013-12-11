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
    public class MenuLayer4 : CCLayer
    {
        public MenuLayer4()
        {
            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 18;

            CCMenuItemFont title1 = new CCMenuItemFont("Sound");
            title1.Enabled = false;
            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 34;
            CCMenuItemToggle item1 = new CCMenuItemToggle(this.menuCallback,
                                                                        new CCMenuItemFont("On"),
                                                                        new CCMenuItemFont("Off"));

            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 18;
            CCMenuItemFont title2 = new CCMenuItemFont("Music");
            title2.Enabled = false;
            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 34;
            CCMenuItemToggle item2 = new CCMenuItemToggle(this.menuCallback,
                                                                        new CCMenuItemFont("On"),
                                                                        new CCMenuItemFont("Off"));

            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 18;
            CCMenuItemFont title3 = new CCMenuItemFont("Quality");
            title3.Enabled = false;
            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 34;
            CCMenuItemToggle item3 = new CCMenuItemToggle(this.menuCallback,
                                                                        new CCMenuItemFont("High"),
                                                                        new CCMenuItemFont("Low"));

            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 18;
            CCMenuItemFont title4 = new CCMenuItemFont("Orientation");
            title4.Enabled = false;
            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 34;
            CCMenuItemToggle item4 = new CCMenuItemToggle(this.menuCallback,
                                                                     new CCMenuItemFont("Off"));

            item4.SubItems.Add(new CCMenuItemFont("33%"));
            item4.SubItems.Add(new CCMenuItemFont("66%"));
            item4.SubItems.Add(new CCMenuItemFont("100%"));

            // you can change the one of the items by doing this
            item4.SelectedIndex = 2;

            CCMenuItemFont.FontName = "arial";
            CCMenuItemFont.FontSize = 34;

            CCLabelBMFont label = new CCLabelBMFont("go back", "fonts/bitmapFontTest3.fnt");
            CCMenuItemLabel back = new CCMenuItemLabel(label, this.backCallback);

            CCMenu menu = new CCMenu(
                          title1, title2,
                          item1, item2,
                          title3, title4,
                          item3, item4,
                          back); // 9 items.

            menu.AlignItemsInColumns(2, 2, 2, 2, 1);

            AddChild(menu);
        }

        public void menuCallback(object pSender)
        {
            //UXLOG("selected item: %x index:%d", dynamic_cast<CCMenuItemToggle*>(sender)->selectedItem(), dynamic_cast<CCMenuItemToggle*>(sender)->selectedIndex() ); 
        }

        public void backCallback(object pSender)
        {
            ((CCLayerMultiplex)m_pParent).SwitchTo(0);
        }
    }
}
