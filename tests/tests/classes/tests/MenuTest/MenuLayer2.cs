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
using cocos2d;

namespace tests
{
    public class MenuLayer2 : CCLayer
    {
        string s_PlayNormal = "Images/btn-play-normal";
        string s_PlaySelect = "Images/btn-play-selected";
        string s_HighNormal = "Images/btn-highscores-normal";
        string s_HighSelect = "Images/btn-highscores-selected";
        string s_AboutNormal = "Images/btn-about-normal";
        string s_AboutSelect = "Images/btn-about-selected";


        protected CCPoint m_centeredMenu;
        protected bool m_alignedH;

        protected void alignMenusH()
        {
            for (int i = 0; i < 2; i++)
            {
                CCMenu menu = (CCMenu)GetChildByTag(100 + i);
                menu.Position = m_centeredMenu;
                if (i == 0)
                {
                    // TIP: if no padding, padding = 5
                    menu.AlignItemsHorizontally();
                    CCPoint p = menu.Position;
                    menu.Position = new CCPoint(p.X + 0, p.Y + 30);

                }
                else
                {
                    // TIP: but padding is configurable
                    menu.AlignItemsHorizontallyWithPadding(40);
                    CCPoint p = menu.Position;
                    menu.Position = new CCPoint(p.X - 0, p.Y - 30);
                }
            }
        }
        protected void alignMenusV()
        {
            for (int i = 0; i < 2; i++)
            {
                CCMenu menu = (CCMenu)GetChildByTag(100 + i);
                menu.Position = m_centeredMenu;
                if (i == 0)
                {
                    // TIP: if no padding, padding = 5
                    menu.AlignItemsVertically();
                    CCPoint p = menu.Position;
                    menu.Position = new CCPoint(p.X + 100, p.Y);
                }
                else
                {
                    // TIP: but padding is configurable
                    menu.AlignItemsVerticallyWithPadding(40);
                    CCPoint p = menu.Position;
                    menu.Position = new CCPoint(p.X - 100, p.Y);
                }
            }
        }

        public MenuLayer2()
        {
            for (int i = 0; i < 2; i++)
            {
                CCMenuItemImage item1 = new CCMenuItemImage(s_PlayNormal, s_PlaySelect, menuCallback);
                CCMenuItemImage item2 = new CCMenuItemImage(s_HighNormal, s_HighSelect, menuCallbackOpacity);
                CCMenuItemImage item3 = new CCMenuItemImage(s_AboutNormal, s_AboutSelect, menuCallbackAlign);

                item1.ScaleX = 1.5f;
                item2.ScaleX = 0.5f;
                item3.ScaleX = 0.5f;

                CCMenu menu = new CCMenu(item1, item2, item3);

                menu.Tag = (int)kTag.kTagMenu;

                AddChild(menu, 0, 100 + i);

                m_centeredMenu = menu.Position;
            }

            m_alignedH = true;
            alignMenusH();
        }
        public void menuCallback(object pSender)
        {
            CCLayerMultiplex m = m_pParent as CCLayerMultiplex;
            m.SwitchTo(0);
        }
        public void menuCallbackOpacity(object pSender)
        {
            CCMenu menu = (CCMenu)(((CCNode)(pSender)).Parent);
            byte opacity = menu.Opacity;
            if (opacity == 128)
                menu.Opacity = 255;
            else
                menu.Opacity = 128;
        }
        public void menuCallbackAlign(object pSender)
        {
            m_alignedH = !m_alignedH;

            if (m_alignedH)
                alignMenusH();
            else
                alignMenusV();
        }
    }
}
