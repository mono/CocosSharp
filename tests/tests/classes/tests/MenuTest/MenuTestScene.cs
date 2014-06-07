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
    public class MenuTestScene : TestScene
    {
        public override void runThisTest()
        {
            CCLayer pLayer1 = new MenuLayer1();
            CCLayer pLayer2 = new MenuLayer2();
            CCLayer pLayer3 = new MenuLayer3();
            CCLayer pLayer4 = new MenuLayer4();
            CCLayer pLayer5 = new MenuLayerPriorityTest();


            CCLayerMultiplex layer = new CCLayerMultiplex(pLayer1, pLayer2, pLayer3, pLayer4, pLayer5);
            AddChild(layer, 0);

            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(this);
        }
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
    }
    enum kTag
    {
        kTagMenu = 1,
        kTagMenu0 = 0,
        kTagMenu1 = 1,
    }
}
