using CocosSharp;

namespace tests
{
    public class RenderTextureTestDemo : CCLayer
    {
        public override void OnEnter()
        {
            base.OnEnter();
            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = new CCLabelTTF(title(), "arial", 28);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTTF l = new CCLabelTTF(strSubtitle, "Thonburi", 16);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            CCMenuItemImage item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

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

        public void restartCallback(object pSender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.restartTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.nextTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(object pSender)
        {
            CCScene s = new RenderTextureScene();
            s.AddChild(RenderTextureScene.backTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}