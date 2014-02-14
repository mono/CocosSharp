using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class EffectAdvanceTextLayer : CCLayer
    {
        protected CCTextureAtlas m_atlas;
        protected string m_strTitle;
		protected CCNode _bgNode;
		protected CCNode _target1;
		protected CCNode _target2;

        public EffectAdvanceTextLayer()
        {

        }

        protected CCSprite grossini;
        protected CCSprite tamara;
        public override void OnEnter()
        {
            base.OnEnter();

			_bgNode = new CCNode ();
			_bgNode.AnchorPoint = CCPoint.AnchorMiddle;
			AddChild (_bgNode);

			//_bgNode.Position = CCVisibleRect.Center;

			var bg = new CCSprite("Images/background3");
            bg.Position = CCVisibleRect.Center;
			//AddChild(bg, 0, EffectAdvanceScene.kTagBackground);
			_bgNode.AddChild (bg);

			_target1 = new CCNode ();
			_target1.AnchorPoint = CCPoint.AnchorMiddle;
            grossini = new CCSprite("Images/grossinis_sister2");
			//bg.AddChild(grossini, 1, EffectAdvanceScene.kTagSprite1);
			_target1.AddChild (grossini);
			_bgNode.AddChild (_target1);

			//_target1.Position = new CCPoint(CCVisibleRect.Left.X + CCVisibleRect.VisibleRect.Size.Width / 3, CCVisibleRect.Bottom.Y + 200);
			grossini.Position = new CCPoint(CCVisibleRect.Left.X + CCVisibleRect.VisibleRect.Size.Width / 3, CCVisibleRect.Bottom.Y + 200);

			var sc = new CCScaleBy(2, 5);
			var sc_back = sc.Reverse();
			grossini.RepeatForever (sc, sc_back);

			_target2 = new CCNode ();
			_target2.AnchorPoint = CCPoint.AnchorMiddle;

            tamara = new CCSprite("Images/grossinis_sister1");
			//bg.AddChild(tamara, 1, EffectAdvanceScene.kTagSprite2);

			_target2.AddChild (tamara);
			_bgNode.AddChild (_target2);
			//_target2.Position = new CCPoint (CCVisibleRect.Left.X + 2 * CCVisibleRect.VisibleRect.Size.Width / 3, CCVisibleRect.Bottom.Y + 200);
			tamara.Position = new CCPoint (CCVisibleRect.Left.X + 2 * CCVisibleRect.VisibleRect.Size.Width / 3, CCVisibleRect.Bottom.Y + 200);



			var sc2 = new CCScaleBy(2, 5);
			var sc2_back = sc2.Reverse();
			tamara.RepeatForever(sc2, sc2_back);

			var label = new CCLabelTTF(title(), "arial", 28);

            label.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 80);
			AddChild(label,TestScene.TITLE_LEVEL);
            label.Tag = EffectAdvanceScene.kTagLabel;

			if (!string.IsNullOrEmpty(subtitle())) 
			{
				var subLabel = new CCLabelTTF(subtitle(), "arial", 20);
				subLabel.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 100);
				AddChild(subLabel,TestScene.TITLE_LEVEL);
				//label.Tag = EffectAdvanceScene.kTagLabel;
			}

			var item1 = new CCMenuItemImage("Images/b1", "Images/b2", backCallback);
			var item2 = new CCMenuItemImage("Images/r1", "Images/r2", restartCallback);
			var item3 = new CCMenuItemImage("Images/f1", "Images/f2", nextCallback);

			var menu = new CCMenu(item1, item2, item3);

            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(CCVisibleRect.Center.X - item2.ContentSize.Width * 2,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
            item2.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
            item3.Position = new CCPoint(CCVisibleRect.Center.X + item2.ContentSize.Width * 2,
                                         CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);

			AddChild(menu, TestScene.MENU_LEVEL);
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
			var s = new EffectAdvanceScene();
            s.AddChild(EffectAdvanceScene.restartEffectAdvanceAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
			var s = new EffectAdvanceScene();
            s.AddChild(EffectAdvanceScene.nextEffectAdvanceAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
			var s = new EffectAdvanceScene();
            s.AddChild(EffectAdvanceScene.backEffectAdvanceAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}
