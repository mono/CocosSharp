using CocosSharp;

namespace tests
{
    public class SpriteDemo : CCLayer
    {
        private string s_pPathB1 = "Images/b1";
        private string s_pPathB2 = "Images/b2";
        private string s_pPathF1 = "Images/f1";
        private string s_pPathF2 = "Images/f2";
        private string s_pPathR1 = "Images/r1";
        private string s_pPathR2 = "Images/r2";

        public virtual string title()
        {
            return "ProgressActionsTest";
        }

        public virtual string subtitle()
        {
            return "";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCApplication.SharedApplication.MainWindowDirector.WinSize;

            CCLabelTtf label = new CCLabelTtf(title(), "arial", 18);
            AddChild(label, 1);
            label.Position = new CCPoint(s.Width / 2, s.Height - 50);

            string strSubtitle = subtitle();
            if (strSubtitle != null)
            {
                CCLabelTtf l = new CCLabelTtf(strSubtitle, "arial", 22);
                AddChild(l, 1);
                l.Position = new CCPoint(s.Width / 2, s.Height - 80);
            }

            CCMenuItemImage item1 = new CCMenuItemImage(s_pPathB1, s_pPathB2, backCallback);
            CCMenuItemImage item2 = new CCMenuItemImage(s_pPathR1, s_pPathR2, restartCallback);
            CCMenuItemImage item3 = new CCMenuItemImage(s_pPathF1, s_pPathF2, nextCallback);

            CCMenu menu = new CCMenu(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.Width / 2 - 100, 30);
            item2.Position = new CCPoint(s.Width / 2, 30);
            item3.Position = new CCPoint(s.Width / 2 + 100, 30);

            AddChild(menu, 1);

            CCLayerColor background = new CCLayerColor(new CCColor4B(255,0,0,255));
            AddChild(background, -10);
        }

        public void restartCallback(object pSender)
        {
            CCScene s = new ProgressActionsTestScene();
            s.AddChild(ProgressActionsTestScene.restartAction());

            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(s);
            //s->release();
        }

        public void nextCallback(object pSender)
        {
            CCScene s = new ProgressActionsTestScene();
            s.AddChild(ProgressActionsTestScene.nextAction());
            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(s);
            //s->release();
        }

        public void backCallback(object pSender)
        {
            CCScene s = new ProgressActionsTestScene();
            s.AddChild(ProgressActionsTestScene.backAction());
            CCApplication.SharedApplication.MainWindowDirector.ReplaceScene(s);
            //s->release();
        }
    }
}