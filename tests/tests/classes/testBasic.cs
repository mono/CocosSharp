using System;
using Cocos2D;

namespace tests
{
    public abstract class TestScene : CCScene
    {
        public TestScene()
        {
            base.Init();
            _GamePadDPadDelegate = new CCGamePadDPadDelegate(MyOnGamePadDPadUpdate);
            _GamePadButtonDelegate = new CCGamePadButtonDelegate(MyOnGamePadButtonUpdate);
        }

        public TestScene(bool bPortrait)
        {
            base.Init();
            _GamePadDPadDelegate = new CCGamePadDPadDelegate(MyOnGamePadDPadUpdate);
            _GamePadButtonDelegate = new CCGamePadButtonDelegate(MyOnGamePadButtonUpdate);
        }

        public override void OnExit()
        {
            base.OnExit();
            CCApplication.SharedApplication.GamePadDPadUpdate -= _GamePadDPadDelegate;
            CCApplication.SharedApplication.GamePadButtonUpdate -= _GamePadButtonDelegate;
        }
        public override void OnEnter()
        {
            base.OnEnter();

            //add the menu item for back to main menu
            CCLabelTTF label = new CCLabelTTF("MainMenu", "arial", 20);
            CCMenuItemLabel pMenuItem = new CCMenuItemLabel(label, MainMenuCallback);

            CCMenu pMenu = new CCMenu(pMenuItem);
            CCSize s = CCDirector.SharedDirector.WinSize;
            pMenu.Position = CCPoint.Zero;
            pMenuItem.Position = new CCPoint(s.Width - 50, 25);

            AddChild(pMenu, 1);
            CCApplication.SharedApplication.GamePadDPadUpdate += _GamePadDPadDelegate;
            CCApplication.SharedApplication.GamePadButtonUpdate += _GamePadButtonDelegate;
        }

        private CCGamePadButtonDelegate _GamePadButtonDelegate;
        private CCGamePadDPadDelegate _GamePadDPadDelegate;

        private bool _bButtonWasPressed = false;
        private bool _aButtonWasPressed = false;

        private void MyOnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, Microsoft.Xna.Framework.PlayerIndex player)
        {
            if (bButton == CCGamePadButtonStatus.Pressed)
            {
                _bButtonWasPressed = true;
            }
            else if (bButton == CCGamePadButtonStatus.Released && _bButtonWasPressed)
            {
                // Select the menu
                MainMenuCallback(null);
                _bButtonWasPressed = false;
            }
            if (aButton == CCGamePadButtonStatus.Pressed)
            {
                _aButtonWasPressed = true;
            }
            else if (aButton == CCGamePadButtonStatus.Released && _aButtonWasPressed)
            {
                // Select the menu
                RestTestCase();
                _aButtonWasPressed = false;
            }
        }

        private long _FirstTicks;
        private bool _bLeftPress = false;
        private bool _bRightPress = false;

        private void MyOnGamePadDPadUpdate(CCGamePadButtonStatus leftButton, CCGamePadButtonStatus upButton, CCGamePadButtonStatus rightButton, CCGamePadButtonStatus downButton, Microsoft.Xna.Framework.PlayerIndex player)
        {
            // Down and Up only
            if (leftButton == CCGamePadButtonStatus.Pressed)
            {
                if (_FirstTicks == 0L)
                {
                    _FirstTicks = DateTime.Now.Ticks;
                    _bLeftPress = true;
                }
            }
            else if (leftButton == CCGamePadButtonStatus.Released && _FirstTicks > 0L && _bLeftPress)
            {
                _FirstTicks = 0L;
                PreviousTestCase();
                _bLeftPress = false;
            }
            if (rightButton == CCGamePadButtonStatus.Pressed)
            {
                if (_FirstTicks == 0L)
                {
                    _FirstTicks = DateTime.Now.Ticks;
                    _bRightPress = true;
                }
            }
            else if (rightButton == CCGamePadButtonStatus.Released && _FirstTicks > 0L && _bRightPress)
            {
                _FirstTicks = 0L;
                NextTestCase();
                _bRightPress = false;
            }
        }

        protected abstract void NextTestCase();
        protected abstract void PreviousTestCase();
        protected abstract void RestTestCase();

        public virtual void MainMenuCallback(object pSender)
        {
            CCScene pScene = new CCScene();
            CCLayer pLayer = new TestController();

            pScene.AddChild(pLayer);
            CCDirector.SharedDirector.ReplaceScene(pScene);
        }

        public abstract void runThisTest();
    }
}