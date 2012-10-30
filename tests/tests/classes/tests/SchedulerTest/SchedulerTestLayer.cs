using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SchedulerTestLayer : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 32);
            AddChild(label);
            label.Position = (new CCPoint(s.width / 2, s.height - 50));

            string subTitle = subtitle();
            if (!string.IsNullOrEmpty(subTitle))
            {
                CCLabelTTF l = CCLabelTTF.Create(subTitle, "arial", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create("Images/b1", "Images/b2", backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create("Images/r1", "Images/r2", restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create("Images/f1", "Images/f2", nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);
            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.width / 2 - 100, 30);
            item2.Position = new CCPoint(s.width / 2, 30);
            item3.Position = new CCPoint(s.width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public void backCallback(CCObject pSender)
        {
            CCScene pScene = new SchedulerTestScene();
            CCLayer pLayer = SchedulerTestScene.backSchedulerTest();

            pScene.AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(pScene);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene pScene = new SchedulerTestScene();
            CCLayer pLayer = SchedulerTestScene.nextSchedulerTest();

            pScene.AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(pScene);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene pScene = new SchedulerTestScene();
            CCLayer pLayer = SchedulerTestScene.restartSchedulerTest();

            pScene.AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(pScene);
        }
    }
}
