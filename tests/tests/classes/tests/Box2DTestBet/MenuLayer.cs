using FarseerPhysics.TestBed.Tests;
using cocos2d;

namespace tests.classes.tests.Box2DTestBet
{
    internal class MenuLayer : CCLayer
    {
        private const int kTagBox2DNode = 5;

        public static int g_totalEntries = TestEntries.TestList.Length - 1;

        private int m_entryID;

        public bool initWithEntryID(int entryId)
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            m_entryID = entryId;

            TouchEnabled = true;

            Box2DView view = Box2DView.viewWithEntryID(entryId);
            AddChild(view, 0, kTagBox2DNode);
            view.Scale = 10;
            view.AnchorPoint = new CCPoint(0, 0);
            view.Position = new CCPoint(s.Width / 2, s.Height / 4);
            //#if (CC_TARGET_PLATFORM == CC_PLATFORM_MARMALADE)
            //    CCLabelBMFont* label = CCLabelBMFont.Create(view.title().c_str(),  "fonts/arial16.fnt");
            //#else    
            CCLabelTTF label = CCLabelTTF.Create(view.title(), "arial", 28);
            //#endif
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            CCMenuItemImage item1 = CCMenuItemImage.Create("Images/b1", "Images/b2", backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create("Images/r1", "Images/r2", restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create("Images/f1", "Images/f2", nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);

            return true;
        }

        public void restartCallback(CCObject sender)
        {
            CCScene s = new Box2dTestBedScene();
            MenuLayer box = menuWithEntryID(m_entryID);
            s.AddChild(box);
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject sender)
        {
            CCScene s = new Box2dTestBedScene();
            int next = m_entryID + 1;
            if (next >= g_totalEntries)
                next = 0;
            MenuLayer box = menuWithEntryID(next);
            s.AddChild(box);
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(CCObject sender)
        {
            CCScene s = new Box2dTestBedScene();
            int next = m_entryID - 1;
            if (next < 0)
            {
                next = g_totalEntries - 1;
            }

            MenuLayer box = menuWithEntryID(next);

            s.AddChild(box);
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public override void RegisterWithTouchDispatcher()
        {
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.TouchDispatcher.AddTargetedDelegate(this, 0, true);
        }

        public override bool TouchBegan(CCTouch touch, CCEvent e)
        {
            return true;
        }

        public override void TouchMoved(CCTouch touch, CCEvent e)
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