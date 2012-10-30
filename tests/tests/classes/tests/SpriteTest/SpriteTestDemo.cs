using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SpriteTestDemo : CCLayer
    {
        protected string m_strTitle;

        public SpriteTestDemo()
        { }

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

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 28);
            AddChild(label, 1);
            label.Position = new CCPoint(s.width / 2, s.height - 50);

            string strSubtitle = subtitle();
            if (!string.IsNullOrEmpty(strSubtitle))
            {
                CCLabelTTF l = CCLabelTTF.Create(strSubtitle, "arial", 16);
                //CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Thonburi", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create("Images/b1", "Images/b2", backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create("Images/r1", "Images/r2", restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create("Images/f1", "Images/f2", nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = new CCPoint();
            item1.Position = new CCPoint(s.width / 2 - 100, 30);
            item2.Position = new CCPoint(s.width / 2, 30);
            item3.Position = new CCPoint(s.width / 2 + 100, 30);

            AddChild(menu, 1); 
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.restartSpriteTestAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.nextSpriteTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.backSpriteTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}
