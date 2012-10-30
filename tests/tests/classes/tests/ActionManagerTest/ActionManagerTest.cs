using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

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

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 32);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.width / 2, s.height - 50));

            CCMenuItemImage item1 = CCMenuItemImage.Create(s_pPathB1, s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(s_pPathR1, s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(s_pPathF1, s_pPathF2, nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = (new CCPoint(0, 0));
            item1.Position = (new CCPoint(s.width / 2 - 100, 30));
            item2.Position = (new CCPoint(s.width / 2, 30));
            item3.Position = (new CCPoint(s.width / 2 + 100, 30));

            AddChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new ActionManagerTestScene();
            s.AddChild(restartActionManagerAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new ActionManagerTestScene();
            s.AddChild(nextActionManagerAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new ActionManagerTestScene();
            s.AddChild(backActionManagerAction());
            CCDirector.SharedDirector.ReplaceScene(s);
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

            return pLayer;
        }

        public static CCLayer restartActionManagerAction()
        {
            CCLayer pLayer = createActionManagerLayer(sceneIdx);

            return pLayer;
        }
    }
}
