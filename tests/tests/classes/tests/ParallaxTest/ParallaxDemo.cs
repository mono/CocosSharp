using cocos2d;

namespace tests
{
    public class ParallaxDemo : CCLayer
    {
        protected CCTextureAtlas m_atlas;

        private const string s_pPathB1 = "Images/b1";
        private const string s_pPathB2 = "Images/b2";
        private const string s_pPathF1 = "Images/f1";
        private const string s_pPathF2 = "Images/f2";
        private const string s_pPathR1 = "Images/r1";
        private const string s_pPathR2 = "Images/r2";

        public virtual string title()
        {
            return "No title";
        }

        public override void OnEnter()
        {
            base.OnEnter();

            CCSize s = CCDirector.SharedDirector.WinSize;

            CCLabelTTF label = CCLabelTTF.Create(title(), "arial", 28);
            AddChild(label, 1);
            label.Position = new CCPoint(s.width / 2, s.height - 50);

            CCMenuItemImage item1 = CCMenuItemImage.Create(s_pPathB1, s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(s_pPathR1, s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(s_pPathF1, s_pPathF2, nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);

            menu.Position = new CCPoint(0, 0);
            item1.Position = new CCPoint(s.width / 2 - 100, 30);
            item2.Position = new CCPoint(s.width / 2, 30);
            item3.Position = new CCPoint(s.width / 2 + 100, 30);

            AddChild(menu, 1);
        }

        public void restartCallback(CCObject pSender)
        {
            CCScene s = new ParallaxTestScene();
            s.AddChild(ParallaxTestScene.restartParallaxAction());

            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void nextCallback(CCObject pSender)
        {
            CCScene s = new ParallaxTestScene();
            s.AddChild(ParallaxTestScene.nextParallaxAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void backCallback(CCObject pSender)
        {
            CCScene s = new ParallaxTestScene();
            s.AddChild(ParallaxTestScene.backParallaxAction());
            CCDirector.SharedDirector.ReplaceScene(s);
        }
    }
}