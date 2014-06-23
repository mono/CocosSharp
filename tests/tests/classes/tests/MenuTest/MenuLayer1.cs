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

using CocosSharp;

namespace tests
{
    public class MenuLayer1 : CCLayer
    {
        protected CCMenuItemLabelAtlas disabledItem;
        private string s_SendScore = "Images/SendScoreButton";
        private string s_MenuItem = "Images/menuitemsprite";
        private string s_PressSendScore = "Images/SendScoreButtonPressed";
		private CCTintBy color_action = new CCTintBy(0.5f, 0, -255, -255); 

		CCEventListenerTouchOneByOne touchListener;
		CCMenu menu;

        public MenuLayer1()
        {

			// We do not have an HD version of the menuitemsprite so internally CocosSharp tries to convert our
			// rectangle coordinates passed to work with HD images so the coordinates are off.  We will just 
			// modify this here to make sure we have the correct sizes when they are passed.
			var spriteNormal = new CCSprite(s_MenuItem, new CCRect(0, 23 * 2, 115, 23).PixelsToPoints(Director.ContentScaleFactor));
			var spriteSelected = new CCSprite(s_MenuItem, new CCRect(0, 23 * 1, 115, 23).PixelsToPoints(Director.ContentScaleFactor));
			var spriteDisabled = new CCSprite(s_MenuItem, new CCRect(0, 23 * 0, 115, 23).PixelsToPoints(Director.ContentScaleFactor));

            var item1 = new CCMenuItemImage(spriteNormal, spriteSelected, spriteDisabled, this.menuCallback);

            // Image Item
            var item2 = new CCMenuItemImage(s_SendScore, s_PressSendScore, this.menuCallback2);

            // Label Item (LabelAtlas)
            var labelAtlas = new CCLabelAtlas("0123456789", "Images/fps_Images.png", 12, 32, '.');
            var item3 = new CCMenuItemLabelAtlas(labelAtlas, this.menuCallbackDisabled);
            item3.DisabledColor = new CCColor3B(32, 32, 64);
            item3.Color = new CCColor3B(200, 200, 255);

            // Font Item
			CCMenuItemFont item4 = new CCMenuItemFont("I toggle enable items", (sender) => 
				{
					disabledItem.Enabled = !disabledItem.Enabled;

				});

            // Label Item (CCLabelBMFont)
            CCLabelBMFont label = new CCLabelBMFont("configuration", "fonts/bitmapFontTest3.fnt");
            CCMenuItemLabelBMFont item5 = new CCMenuItemLabelBMFont(label, this.menuCallbackConfig);


            // Testing issue #500
            item5.Scale = 0.8f;

			CCMenuItemFont.FontSize = 30;
            // Events
            CCMenuItemFont item6 = new CCMenuItemFont("Priority Test", menuCallbackPriorityTest);

            // Font Item
			CCMenuItemFont item7 = new CCMenuItemFont("Quit", this.onQuit);
			item7.RepeatForever(color_action, color_action.Reverse());

			menu = new CCMenu(item1, item2, item3, item4, item5, item6, item7);
			menu.AlignItemsVertically();

            disabledItem = item3;
            disabledItem.Enabled = false;

            AddChild(menu);
			menu.Scale = 0;
			menu.RunAction(new CCScaleTo(1, 1));
        }

		protected override void RunningOnNewWindow(CCSize windowSize)
		{
			base.RunningOnNewWindow(windowSize);

			// Register Touch Event
			touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;

			touchListener.OnTouchBegan = onTouchBegan;
			touchListener.OnTouchMoved = onTouchMoved;
			touchListener.OnTouchEnded = onTouchEnded;
			touchListener.OnTouchCancelled = onTouchCancelled;

			EventDispatcher.AddEventListener(touchListener, 1);

			// elastic effect
			CCSize s = windowSize;

			int i = 0;
			CCNode child;
			var pArray = menu.Children;
			object pObject = null;
			if (pArray.Count > 0)
			{
				for (int j = 0; j < pArray.Count; j++)
				{
					pObject = pArray[j];
					if (pObject == null)
						break;

					child = (CCNode) pObject;
					CCPoint dstPoint = child.Position;
					int offset = (int) (s.Width / 2 + 50);
					if (i % 2 == 0)
						offset = -offset;

					child.Position = new CCPoint(dstPoint.X + offset, dstPoint.Y);
					child.RunAction(new CCEaseElasticOut(new CCMoveBy(2, new CCPoint(dstPoint.X - offset, 0)), 0.35f));
					i++;

				}
			}
		}

        private void menuCallbackPriorityTest(object pSender)
        {
            ((CCLayerMultiplex) Parent).SwitchTo(4);
        }

		bool onTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            return true;
        }

		void onTouchEnded(CCTouch touch, CCEvent touchEvent)
        {
        }

		void onTouchCancelled(CCTouch touch, CCEvent touchEvent)
        {
        }

		void onTouchMoved(CCTouch touch, CCEvent touchEvent)
        {
        }

        public void allowTouches(float dt)
        {
			EventDispatcher.SetPriority(touchListener,1);
            base.UnscheduleAll();
            CCLog.Log("TOUCHES ALLOWED AGAIN");
        }

        public void menuCallback(object pSender)
        {
            ((CCLayerMultiplex) Parent).SwitchTo(1);
        }

        public void menuCallbackConfig(object pSender)
        {
            ((CCLayerMultiplex) Parent).SwitchTo(3);
        }

        public void menuCallbackDisabled(object pSender)
        {
            // hijack all touch events for 5 seconds
			EventDispatcher.SetPriority(touchListener,-1);
            base.Schedule(this.allowTouches, 5.0f);
            CCLog.Log("TOUCHES DISABLED FOR 5 SECONDS");
        }

        public void menuCallback2(object pSender)
        {
            (Parent as CCLayerMultiplex).SwitchTo(2);
        }

        public void onQuit(object pSender)
        {
            //[[Director sharedDirector] end];
            //getCocosApp()->exit();
        }
    }
}
