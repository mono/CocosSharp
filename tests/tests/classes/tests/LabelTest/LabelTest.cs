using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using tests;
using System.Diagnostics;

namespace CocosSharp
{
    public enum TagSprite
    {
        kTagTileMap = 1,
        kTagSpriteManager = 1,
        kTagAnimation1 = 1,
        kTagBitmapAtlas1 = 1,
        kTagBitmapAtlas2 = 2,
        kTagBitmapAtlas3 = 3,

        kTagSprite1,
        kTagSprite2,
        kTagSprite3,
        kTagSprite4,
        kTagSprite5,
        kTagSprite6,
        kTagSprite7,
        kTagSprite8
    }

    public class AtlasDemo : CCLayer
    {
        //protected:

        public AtlasDemo()
        {

        }

        public enum LabelTestConstant
        {
            IDC_NEXT = 100,
            IDC_BACK,
            IDC_RESTART
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

			var label = new CCLabelTtf(title(), "arial", 32);
			AddChild(label, 9999);
			label.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 30);

			var strSubtitle = subtitle();
			if (!string.IsNullOrEmpty(strSubtitle))
            {
				var l = new CCLabelTtf(strSubtitle, "Thonburi", 16);
				AddChild(l, 9999);
				l.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 60);
            }

			var item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
			var item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
			var item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

			var menu = new CCMenu(item1, item2, item3);

			menu.Position = CCPoint.Zero;
			item1.Position = new CCPoint (CCVisibleRect.Center.X - item2.ContentSize.Width * 2, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
			item2.Position = new CCPoint (CCVisibleRect.Center.X, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);
			item3.Position = new CCPoint (CCVisibleRect.Center.X + item2.ContentSize.Width * 2, CCVisibleRect.Bottom.Y + item2.ContentSize.Height / 2);

			AddChild(menu, 999);

        }

        public void restartCallback(object pSender)
        {
            CCScene s = new AtlasTestScene();
            s.AddChild(AtlasTestScene.restartAtlasAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {

            CCScene s = new AtlasTestScene();

            s.AddChild(AtlasTestScene.nextAtlasAction());

            CCDirector.SharedDirector.ReplaceScene(s);

        }

        public void backCallback(object pSender)
        {

            CCScene s = new AtlasTestScene();

            s.AddChild(AtlasTestScene.backAtlasAction());

            CCDirector.SharedDirector.ReplaceScene(s);

        }

    }

}
