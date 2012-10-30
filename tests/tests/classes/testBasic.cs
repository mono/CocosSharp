
using cocos2d;

namespace tests
{
    public abstract class TestScene : CCScene
    {
        public TestScene()
        {
            base.Init();
        }

        public TestScene(bool bPortrait)
        {
            base.Init();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            //add the menu item for back to main menu
            CCLabelTTF label = CCLabelTTF.Create("MainMenu", "arial", 20);
            CCMenuItemLabel pMenuItem = CCMenuItemLabel.Create(label, MainMenuCallback);

            CCMenu pMenu = CCMenu.Create(pMenuItem);
            CCSize s = CCDirector.SharedDirector.WinSize;
            pMenu.Position = CCPoint.Zero;
            pMenuItem.Position = new CCPoint(s.width - 50, 25);

            AddChild(pMenu, 1);
        }

        public virtual void MainMenuCallback(CCObject pSender)
        {
            CCScene pScene = CCScene.Create();
            CCLayer pLayer = new TestController();

            pScene.AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(pScene);
        }

        public abstract void runThisTest();
    }
}