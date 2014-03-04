using System;
using CocosSharp;

namespace tests
{
    public abstract class TestScene : CCScene
    {

		public static int MENU_LEVEL = 99999;
		public static int TITLE_LEVEL = 99999;

        public TestScene()
        {
            _GamePadDPadDelegate = new CCGamePadDPadDelegate(MyOnGamePadDPadUpdate);
            _GamePadButtonDelegate = new CCGamePadButtonDelegate(MyOnGamePadButtonUpdate);
        }

        public TestScene(bool bPortrait)
        {
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
			var label = new CCLabelTtf("MainMenu", "arial", 20);
			var pMenuItem = new CCMenuItemLabelTTF(label, MainMenuCallback);

			var pMenu = new CCMenu(pMenuItem);
			var visiblePoint = new CCPoint (CCVisibleRect.Right.X - 50, CCVisibleRect.Bottom.Y + 25);
            pMenu.Position = CCPoint.Zero;
			pMenuItem.Position = visiblePoint;

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


    public class CCVisibleRect
    {
		private static CCRect visibleRect = CCRect.Zero;

        private static void LazyInit()
        {
            if (visibleRect.Size.Width == 0.0f && visibleRect.Size.Height == 0.0f)
            {
                visibleRect.Origin = CCDrawManager.VisibleOrigin;
                visibleRect.Size = CCDrawManager.VisibleSize;
            }
        }

        public static CCRect VisibleRect
        {
            get
            {
                LazyInit();
                return new CCRect(visibleRect.Origin.X, visibleRect.Origin.Y, visibleRect.Size.Width,
                                  visibleRect.Size.Height);
            }
        }

        public static CCPoint Left
        {
            get
            {
                LazyInit();
                return new CCPoint(visibleRect.Origin.X, visibleRect.Origin.Y + visibleRect.Size.Height / 2);
            }
        }

        public static CCPoint Right
        {
            get
            {
                LazyInit();
                return new CCPoint(visibleRect.Origin.X + visibleRect.Size.Width,
                                   visibleRect.Origin.Y + visibleRect.Size.Height / 2);
            }
        }

        public static CCPoint Top
        {
            get
            {
                LazyInit();
                return new CCPoint(visibleRect.Origin.X + visibleRect.Size.Width / 2,
                                   visibleRect.Origin.Y + visibleRect.Size.Height);
            }
        }

        public static CCPoint Bottom
        {
            get
            {
                LazyInit();
                return new CCPoint(visibleRect.Origin.X + visibleRect.Size.Width / 2, visibleRect.Origin.Y);
            }
        }

        public static CCPoint Center
        {
            get
            {
                LazyInit();
                return new CCPoint(visibleRect.Origin.X + visibleRect.Size.Width / 2,
                                   visibleRect.Origin.Y + visibleRect.Size.Height / 2);
            }
        }

        public static CCPoint LeftTop
        {
            get
            {
                LazyInit();
                return new CCPoint(visibleRect.Origin.X, visibleRect.Origin.Y + visibleRect.Size.Height);
            }
        }

        public static CCPoint RightTop
        {
            get
            {
                LazyInit();
                return new CCPoint(visibleRect.Origin.X + visibleRect.Size.Width,
                                   visibleRect.Origin.Y + visibleRect.Size.Height);
            }
        }

        public static CCPoint LeftBottom
        {
            get
            {
                LazyInit();
                return visibleRect.Origin;
            }
        }

        public static CCPoint RightBottom
        {
            get
            {
                LazyInit();
                return new CCPoint(visibleRect.Origin.X + visibleRect.Size.Width, visibleRect.Origin.Y);
            }
        }
    }
}