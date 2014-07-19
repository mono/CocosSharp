using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ActionManagerTest : CCLayer
    {
        string s_pPathB1 = "Images/b1";
        string s_pPathB2 = "Images/b2";
        string s_pPathR1 = "Images/r1";
        string s_pPathR2 = "Images/r2";
        string s_pPathF1 = "Images/f1";
        string s_pPathF2 = "Images/f2";

        protected CCTextureAtlas m_atlas;

        protected string m_strTitle;

        public ActionManagerTest() { }

        public virtual string title()
        {
            return "No title";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            CCLabelTtf label = new CCLabelTtf(title(), "arial", 40);
            label.AnchorPoint = new CCPoint (0.5f, 0.5f);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.Width / 2, s.Height - 50));

            CCMenuItemImage item1 = new CCMenuItemImage(s_pPathB1, s_pPathB2, backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage(s_pPathR1, s_pPathR2, restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage(s_pPathF1, s_pPathF2, nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            float padding = 10.0f;
            float halfRestartWidth = item2.ContentSize.Width / 2.0f;

            menu.Position = (new CCPoint(0, 0));

            // Anchor point of menu items is 0.5, 0.5 by default
            item1.Position = (new CCPoint(s.Width / 2 - item1.ContentSize.Width / 2.0f - halfRestartWidth - padding, item2.ContentSize.Height + padding));
            item2.Position = (new CCPoint(s.Width / 2, item2.ContentSize.Height + padding));
            item3.Position = (new CCPoint(s.Width / 2 + item3.ContentSize.Width / 2.0f + halfRestartWidth + padding, item2.ContentSize.Height + padding));

            AddChild(menu, TestScene.MENU_LEVEL);
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new ActionManagerTestScene();
            s.AddChild(restartActionManagerAction());

            Director.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new ActionManagerTestScene();

            s.AddChild(nextActionManagerAction());
            Director.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new ActionManagerTestScene();
            s.AddChild(backActionManagerAction());
            Director.ReplaceScene(s);
        }

        public static int sceneIdx = -1;
        public static int MAX_LAYER = 5;

        public static CCLayer backActionManagerAction()
        {
            sceneIdx--;
            int total = MAX_LAYER;
            if (sceneIdx < 0)
                sceneIdx += total;

            CCLayer pLayer = createActionManagerLayer(sceneIdx);

            return pLayer;
        }

        public static CCLayer createActionManagerLayer(int nIndex)
        {
            switch (nIndex)
            {
                case 0: return new CrashTest();
                case 1: return new LogicTest();
                case 2: return new PauseTest();
                case 3: return new RemoveTest();
                case 4: return new ResumeTest();
            }

            return null;
        }

        public static CCLayer nextActionManagerAction()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            CCLayer pLayer = createActionManagerLayer(sceneIdx);
            pLayer.Camera = AppDelegate.SharedCamera;

            return pLayer;
        }

        public static CCLayer restartActionManagerAction()
        {
            CCLayer pLayer = createActionManagerLayer(sceneIdx);
            pLayer.Camera = AppDelegate.SharedCamera;

            return pLayer;
        }
    }
}
