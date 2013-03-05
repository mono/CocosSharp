using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LayerTest : CCLayer
    {
        string s_pPathB1 = "Images/b1";
        string s_pPathB2 = "Images/b2";
        string s_pPathR1 = "Images/r1";
        string s_pPathR2 = "Images/r2";
        string s_pPathF1 = "Images/f1";
        string s_pPathF2 = "Images/f2";

        protected string m_strTitle;

        public LayerTest()
        {

        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 32);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.Width / 2, s.Height - 50));

            string subtitle_ = subtitle();
            if (subtitle_.Length > 0)
            {
                CCLabelTTF l = CCLabelTTF.Create(subtitle_, "arial", 16);
                AddChild(l, 1);
                l.Position = (new CCPoint(s.Width / 2, s.Height - 80));
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create(s_pPathB1, s_pPathB2, (backCallback));
            CCMenuItemImage item2 = CCMenuItemImage.Create(s_pPathR1, s_pPathR2, (restartCallback));
            CCMenuItemImage item3 = CCMenuItemImage.Create(s_pPathF1, s_pPathF2, (nextCallback));

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.restartTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.nextTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new LayerTestScene();
            s.AddChild(LayerTestScene.backTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}
