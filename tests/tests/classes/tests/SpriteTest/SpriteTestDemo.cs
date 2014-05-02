using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

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

            CCLabelTtf label = new CCLabelTtf(title(), "arial", 28);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            string strSubtitle = subtitle();
            if (!string.IsNullOrEmpty(strSubtitle))
            {
                CCLabelTtf l = new CCLabelTtf(strSubtitle, "arial", 16);
                //CCLabelTTF l = CCLabelTTF.labelWithString(strSubtitle, "Thonburi", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            CCMenuItemImage item1 = new CCMenuItemImage("Images/b1", "Images/b2", backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage("Images/r1", "Images/r2", restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage("Images/f1", "Images/f2", nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint();
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1); 
        }

        public void restartCallback(object pSender)
        {
            ClearCaches();

            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.restartSpriteTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            ClearCaches();

            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.nextSpriteTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            ClearCaches();
            
            CCScene s = new SpriteTestScene();
            s.AddChild(SpriteTestScene.backSpriteTestAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        private void ClearCaches()
        {
            CCSpriteFrameCache.PurgeSharedSpriteFrameCache();
            CCTextureCache.PurgeInstance();
        }
    }
}
