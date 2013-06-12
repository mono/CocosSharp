using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

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

            CCSprite bg = new CCSprite("Images/background3");
            AddChild(bg, 0, EffectAdvanceScene.kTagBackground);
            bg.Position = CCVisibleRect.Center;

            grossini = new CCSprite("Images/grossinis_sister2");
            bg.AddChild(grossini, 1, EffectAdvanceScene.kTagSprite1);
            grossini.Position = new CCPoint(CCVisibleRect.Left.X + CCVisibleRect.VisibleRect.Size.Width / 3,
                                            CCVisibleRect.Center.Y);
            CCActionInterval sc = new CCScaleBy(2, 5);
            CCFiniteTimeAction sc_back = sc.Reverse();
            grossini.RunAction(new CCRepeatForever ((CCActionInterval)(new CCSequence(sc, sc_back))));

            tamara = new CCSprite("Images/grossinis_sister1");
            bg.AddChild(tamara, 1, EffectAdvanceScene.kTagSprite2);
            tamara.Position = new CCPoint(CCVisibleRect.Left.X + 2 * CCVisibleRect.VisibleRect.Size.Width / 3,
                                          CCVisibleRect.Center.Y);
            CCActionInterval sc2 = new CCScaleBy(2, 5);
            CCFiniteTimeAction sc2_back = sc2.Reverse();
            tamara.RunAction(new CCRepeatForever ((CCActionInterval)(new CCSequence(sc2, sc2_back))));

            CCLabelTTF label = new CCLabelTTF(title(), "arial", 28);

            label.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 80);
            AddChild(label);
            label.Tag = EffectAdvanceScene.kTagLabel;

            CCMenuItemImage item1 = new CCMenuItemImage("Images/b1", "Images/b2", backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage("Images/r1", "Images/r2", restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage("Images/f1", "Images/f2", nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(CCVisibleRect.Center.X - item2.ContentSize.Width * 2,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
            item2.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
            item3.Position = new CCPoint(CCVisibleRect.Center.X + item2.ContentSize.Width * 2,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);

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
