using cocos2d;

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

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 32);
            AddChild(label, 1);
            label.Position = (new CCPoint(s.Width / 2, s.Height - 50));

            string strSubtitle = subtitle();
            if (!string.IsNullOrEmpty(strSubtitle))
            {
                CCLabelTTF l = CCLabelTTF.Create(strSubtitle, "arial", 16);
                AddChild(l, 1);
                l.Position = (new CCPoint(s.Width / 2, s.Height - 80));
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.restartCocosNodeAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.nextCocosNodeAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new CocosNodeTestScene(); //CCScene.node();
            s.AddChild(CocosNodeTestScene.backCocosNodeAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}