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
    public class MenuLayerPriorityTest : CCLayer
    {
        private CCMenu menu1;
        private CCMenu menu2;
        private bool priority;

        public MenuLayerPriorityTest()
        {

        }

        public override void OnEnter()
        {
            base.OnEnter();

            // Testing empty menu
            menu1 = new CCMenu();
            menu2 = new CCMenu();


            // Menu 1
            var item1 = new CCMenuItemFont("Return to Main Menu", menuCallback);
            var item2 = new CCMenuItemFont("Disable menu for 5 seconds", disableMenuCallback);


            menu1.AddChild(item1);
            menu1.AddChild(item2);

            menu1.AlignItemsVertically(2);

            AddChild(menu1);

            // Menu 2
            priority = true;
            //CCMenuItemFont.setFontSize(48);
            item1 = new CCMenuItemFont("Toggle priority", togglePriorityCallback);
            item1.Scale = 1.5f;
            item1.Color = new CCColor3B(0, 0, 255);
            menu2.AddChild(item1);
            AddChild(menu2);
        }

        public void menuCallback(object pSender)
        {
            ((CCLayerMultiplex) Parent).SwitchTo(0);
        }

        public void disableMenuCallback(object pSender)
        {
            menu1.Enabled = false;
            var wait = new CCDelayTime (5);
            var enable = new CCCallFunc(enableMenuCallback);

            var seq = new CCSequence(wait, enable);
            menu1.RunAction(seq);
        }

        private void enableMenuCallback()
        {
            menu1.Enabled = true;
        }

        private void togglePriorityCallback(object pSender)
        {
//            if (m_bPriority)
//            {
//                m_pMenu2.HandlerPriority = (CCMenu.DefaultMenuHandlerPriority + 20);
//                m_bPriority = false;
//            }
//            else
//            {
//                m_pMenu2.HandlerPriority = (CCMenu.DefaultMenuHandlerPriority - 20);
//                m_bPriority = true;
//            }
        }
    }
}

