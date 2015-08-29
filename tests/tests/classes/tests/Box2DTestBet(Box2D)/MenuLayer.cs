using CocosSharp;
using tests;

namespace Box2D.TestBed
{
    internal class MenuLayer : CCLayer
    {
        private const int kTagBox2DNode = 5;

        public static int g_totalEntries = TestEntries.TestList.Length - 1;

        private int m_entryID;

        public bool initWithEntryID(int entryId)
        {
            m_entryID = entryId;

			// Register Touch Event
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = false;

			touchListener.OnTouchBegan = onTouchBegan;
			touchListener.OnTouchMoved = onTouchMoved;

			AddEventListener(touchListener);

            return true;
        }

        public override void OnEnter ()
        {
            base.OnEnter ();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            Box2DView view = Box2DView.viewWithEntryID(m_entryID);
            AddChild(view, 0, kTagBox2DNode);
            view.Scale = 8;
            view.AnchorPoint = new CCPoint(0, 0);
            view.Position = new CCPoint(s.Width / 2, s.Height / 4);

            //#if (CC_TARGET_PLATFORM == CC_PLATFORM_MARMALADE)
            //    CCLabelBMFont* label = new CCLabelBMFont(view.title().c_str(),  "fonts/arial16.fnt");
            //#else    
            var label = new CCLabel(view.title(), "arial", 18, CCLabelFormat.SpriteFont);
            //#endif
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 30);

            CCMenuItemImage item1 = new CCMenuItemImage("Images/b1", "Images/b2", backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage("Images/r1", "Images/r2", restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage("Images/f1", "Images/f2", nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public void restartCallback(object sender)
        {
            CCScene s = new Box2dTestBedScene();
            MenuLayer box = menuWithEntryID(m_entryID);
            s.AddChild(box);
            Scene.Director.ReplaceScene(s);
        }

        public void nextCallback(object sender)
        {
            CCScene s = new Box2dTestBedScene();
            int next = m_entryID + 1;
            if (next >= g_totalEntries)
                next = 0;
            MenuLayer box = menuWithEntryID(next);
            s.AddChild(box);
            Scene.Director.ReplaceScene(s);
        }

        public void backCallback(object sender)
        {
            CCScene s = new Box2dTestBedScene();
            int next = m_entryID - 1;
            if (next < 0)
            {
                next = g_totalEntries - 1;
            }

            MenuLayer box = menuWithEntryID(next);

            s.AddChild(box);
            Scene.Director.ReplaceScene(s);
        }

		bool onTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            return true;
        }

		void onTouchMoved(CCTouch touch, CCEvent touchEvent)
        {
            CCPoint diff = touch.Delta;
            CCNode node = GetChildByTag(kTagBox2DNode);
            node.Position = node.Position + diff;
        }

        public static MenuLayer menuWithEntryID(int entryId)
        {
            var pLayer = new MenuLayer();
            pLayer.initWithEntryID(entryId);
            return pLayer;
        }
    }
}