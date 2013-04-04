using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class EffectAdvanceTextLayer : CCLayer
    {
        protected CCTextureAtlas m_atlas;
        protected string m_strTitle;

        public EffectAdvanceTextLayer()
        {

        }

        protected CCSprite grossini;
        protected CCSprite tamara;
        public override void OnEnter()
        {
            base.OnEnter();

            float x, y;

            CCSize size = CCDirector.SharedDirector.WinSize;
            x = size.Width;
            y = size.Height;

            CCSprite bg = CCSprite.Create("Images/background3");
            AddChild(bg, 0, EffectAdvanceScene.kTagBackground);
            bg.Position = new CCPoint(x / 2, y / 2);

            grossini = CCSprite.Create("Images/grossinis_sister2");
            bg.AddChild(grossini, 1, EffectAdvanceScene.kTagSprite1);
            grossini.Position = new CCPoint(x / 3.0f, 200);
            CCActionInterval sc = new CCScaleBy(2, 5);
            CCFiniteTimeAction sc_back = sc.Reverse();
            grossini.RunAction(new CCRepeatForever ((CCActionInterval)(CCSequence.FromActions(sc, sc_back))));

            tamara = CCSprite.Create("Images/grossinis_sister1");
            bg.AddChild(tamara, 1, EffectAdvanceScene.kTagSprite2);
            tamara.Position = new CCPoint(2 * x / 3.0f, 200);
            CCActionInterval sc2 = new CCScaleBy(2, 5);
            CCFiniteTimeAction sc2_back = sc2.Reverse();
            tamara.RunAction(new CCRepeatForever ((CCActionInterval)(CCSequence.FromActions(sc2, sc2_back))));

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 28);

            label.Position = new CCPoint(x / 2, y - 80);
            AddChild(label);
            label.Tag = EffectAdvanceScene.kTagLabel;

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTTF l = CCLabelTTF.Create(strSubtitle, "arial", 16);
                AddChild(l, 101);
                l.Position = new CCPoint(size.Width / 2, size.Height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create("Images/b1", "Images/b2", backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create("Images/r1", "Images/r2", restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create("Images/f1", "Images/f2", nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(size.Width / 2 - 100, 30);
            item2.Position = new CCPoint(size.Width / 2, 30);
            item3.Position = new CCPoint(size.Width / 2 + 100, 30);

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

        public void restartCallback(object pSender)
        {
            CCScene s = new EffectAdvanceScene();
            s.AddChild(EffectAdvanceScene.restartEffectAdvanceAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new EffectAdvanceScene();
            s.AddChild(EffectAdvanceScene.nextEffectAdvanceAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new EffectAdvanceScene();
            s.AddChild(EffectAdvanceScene.backEffectAdvanceAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}
