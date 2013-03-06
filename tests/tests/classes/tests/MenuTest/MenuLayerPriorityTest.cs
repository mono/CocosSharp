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
    public class MenuLayerPriorityTest : CCLayer
    {
        private CCMenu m_pMenu1;
        private CCMenu m_pMenu2;
        private bool m_bPriority;

        public MenuLayerPriorityTest()
        {
            // Testing empty menu
            m_pMenu1 = CCMenu.Create();
            m_pMenu2 = CCMenu.Create();


            // Menu 1
            CCMenuItemFont item1 = CCMenuItemFont.Create("Return to Main Menu", menuCallback);
            CCMenuItemFont item2 = CCMenuItemFont.Create("Disable menu for 5 seconds", disableMenuCallback);


            m_pMenu1.AddChild(item1);
            m_pMenu1.AddChild(item2);

            m_pMenu1.AlignItemsVerticallyWithPadding(2);

            AddChild(m_pMenu1);

            // Menu 2
            m_bPriority = true;
            //CCMenuItemFont.setFontSize(48);
            item1 = CCMenuItemFont.Create("Toggle priority", togglePriorityCallback);
            item1.Scale = 1.5f;
            item1.Color = new CCColor3B(0, 0, 255);
            m_pMenu2.AddChild(item1);
            AddChild(m_pMenu2);
        }

        public void menuCallback(CCObject pSender)
        {
            ((CCLayerMultiplex) m_pParent).SwitchTo(0);
        }

        public void disableMenuCallback(CCObject pSender)
        {
            m_pMenu1.Enabled = false;
            CCDelayTime wait = CCDelayTime.Create(5);
            CCCallFunc enable = CCCallFunc.Create(enableMenuCallback);

            CCFiniteTimeAction seq = CCSequence.Create(wait, enable);
            m_pMenu1.RunAction(seq);
        }

        private void enableMenuCallback()
        {
            m_pMenu1.Enabled = true;
        }

        private void togglePriorityCallback(CCObject pSender)
        {
            if (m_bPriority)
            {
                m_pMenu2.SetHandlerPriority(CCMenu.kCCMenuHandlerPriority + 20);
                m_bPriority = false;
            }
            else
            {
                m_pMenu2.SetHandlerPriority(CCMenu.kCCMenuHandlerPriority - 20);
                m_bPriority = true;
            }
        }
    }
}

