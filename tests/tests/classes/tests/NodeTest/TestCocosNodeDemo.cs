using CocosSharp;

namespace tests
{
    public class TestCocosNodeDemo : CCLayer
    {
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
			AddChild(label, TestScene.TITLE_LEVEL);
			label.Position = new CCPoint(CCVisibleRect.Center.X, CCVisibleRect.Top.Y - 30);

			var strSubtitle = subtitle();
			if (!string.IsNullOrEmpty(strSubtitle))
			{
				var l = new CCLabelTtf(strSubtitle, "Thonburi", 16);
				AddChild(l, TestScene.TITLE_LEVEL);
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

			AddChild(menu, TestScene.MENU_LEVEL);
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.restartCocosNodeAction());

            Scene.Director.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.nextCocosNodeAction());
            Scene.Director.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.backCocosNodeAction());
            Scene.Director.ReplaceScene(s);
        }
    }
}