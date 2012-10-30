using cocos2d;

namespace tests
{
    public class RenderTextureTestDemo : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 28);
            AddChild(label, 1);
            label.Position = new CCPoint(s.width / 2, s.height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTTF l = CCLabelTTF.Create(strSubtitle, "Thonburi", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.width / 2, s.height - 80);
            }

            CCMenuItemImage item1 = CCMenuItemImage.Create(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.width / 2 - 100, 30);
            item2.Position = new CCPoint(s.width / 2, 30);
            item3.Position = new CCPoint(s.width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public virtual string title()
        {
            return "Render Texture Test";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.restartTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.nextTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.backTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}