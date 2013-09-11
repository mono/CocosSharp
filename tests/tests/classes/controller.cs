using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;
using tests.Clipping;
using tests.FontTest;
using tests.Extensions;
using tests.classes.tests.Box2DTestBet;
using Box2D.TestBed;

namespace tests
{
    public class TestController : CCLayer
    {
        static int LINE_SPACE = 40;
        static CCPoint s_tCurPos = new CCPoint(0.0f, 0.0f);
        private CCGamePadButtonDelegate _GamePadButtonDelegate;
        private CCGamePadDPadDelegate _GamePadDPadDelegate;
        private List<CCMenuItem> _Items = new List<CCMenuItem>();
        private int _CurrentItemIndex = 0;
        private CCSprite _menuIndicator;

        public TestController()
        {
            // add close menu
            var pCloseItem = new CCMenuItemImage(TestResource.s_pPathClose, TestResource.s_pPathClose, closeCallback);
            var pMenu = new CCMenu(pCloseItem);
            var s = CCDirector.SharedDirector.WinSize;
#if !XBOX && !OUYA
            TouchEnabled = true;
#else
            GamePadEnabled = true;
			KeypadEnabled = true;
#endif
#if WINDOWS || WINDOWSGL || MACOS
			GamePadEnabled = true;
#endif

            pMenu.Position = CCPoint.Zero;
            pCloseItem.Position = new CCPoint(s.Width - 30, s.Height - 30);
#if !PSM && !WINDOWS_PHONE
#if NETFX_CORE
            CCLabelTTF versionLabel = new CCLabelTTF("v" + this.GetType().GetAssemblyName().Version.ToString(), "arial", 12);
#else
            CCLabelTTF versionLabel = new CCLabelTTF("v" + this.GetType().Assembly.GetName().Version.ToString(), "arial", 12);
#endif
            versionLabel.Position = new CCPoint(versionLabel.ContentSizeInPixels.Width/2f, s.Height - 18f);
            versionLabel.HorizontalAlignment = CCTextAlignment.Left;
            AddChild(versionLabel, 20000);
#endif
            // add menu items for tests
            m_pItemMenu = new CCMenu();
            for (int i = 0; i < (int)(TestCases.TESTS_COUNT); ++i)
            {
                var label = new CCLabelTTF(Tests.g_aTestNames[i], "arial", 24);
                var pMenuItem = new CCMenuItemLabel(label, menuCallback);

                pMenuItem.UserData = i;
                m_pItemMenu.AddChild(pMenuItem, 10000);
#if XBOX || OUYA
                pMenuItem.Position = new CCPoint(s.Width / 2, -(i + 1) * LINE_SPACE);
#else
                pMenuItem.Position = new CCPoint(s.Width / 2, (s.Height - (i + 1) * LINE_SPACE));
#endif
                _Items.Add(pMenuItem);
            }

            m_pItemMenu.ContentSize = new CCSize(s.Width, ((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE);
#if XBOX || OUYA
            CCSprite sprite = new CCSprite("Images/aButton");
            AddChild(sprite, 10001);
            _menuIndicator = sprite;
            // Center the menu on the first item so that it is 
            // in the center of the screen
            _HomePosition = new CCPoint(0f, s.Height / 2f + LINE_SPACE / 2f);
            _LastPosition = new CCPoint(0f, _HomePosition.Y - (_Items.Count - 1) * LINE_SPACE);

#else
            _HomePosition = s_tCurPos;
#endif
            m_pItemMenu.Position = _HomePosition;
            AddChild(m_pItemMenu);

            AddChild(pMenu, 1);

            _GamePadDPadDelegate = new CCGamePadDPadDelegate(MyOnGamePadDPadUpdate);
            _GamePadButtonDelegate = new CCGamePadButtonDelegate(MyOnGamePadButtonUpdate);

            // set the first one to have the selection highlight
            _CurrentItemIndex = 0;
            SelectMenuItem();
        }

        private CCPoint _HomePosition;
        private CCPoint _LastPosition;

        private void SelectMenuItem()
        {
            _Items[_CurrentItemIndex].Selected();
            if (_menuIndicator != null)
            {
                _menuIndicator.Position = new CCPoint(
                    m_pItemMenu.Position.X + _Items[_CurrentItemIndex].Position.X - _Items[_CurrentItemIndex].ContentSizeInPixels.Width / 2f - _menuIndicator.ContentSizeInPixels.Width / 2f - 5f,
                    m_pItemMenu.Position.Y + _Items[_CurrentItemIndex].Position.Y
                    );
            }
        }

        private void NextMenuItem() 
        {
            _Items[_CurrentItemIndex].Unselected();
            _CurrentItemIndex = (_CurrentItemIndex + 1) % _Items.Count;
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            m_pItemMenu.Position = (new CCPoint(0, _HomePosition.Y + _CurrentItemIndex * LINE_SPACE));
            s_tCurPos = m_pItemMenu.Position;
            SelectMenuItem();
        }
        private void PreviousMenuItem() 
        {
            _Items[_CurrentItemIndex].Unselected();
            _CurrentItemIndex--;
            if(_CurrentItemIndex < 0) {
                _CurrentItemIndex = _Items.Count - 1;
            }
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            m_pItemMenu.Position = (new CCPoint(0, _HomePosition.Y + _CurrentItemIndex * LINE_SPACE));
            s_tCurPos = m_pItemMenu.Position;
            SelectMenuItem();
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
            CCApplication.SharedApplication.GamePadDPadUpdate += _GamePadDPadDelegate;
            CCApplication.SharedApplication.GamePadButtonUpdate += _GamePadButtonDelegate;
        }

        private bool _aButtonWasPressed = false;

        private void MyOnGamePadButtonUpdate(CCGamePadButtonStatus backButton, CCGamePadButtonStatus startButton, CCGamePadButtonStatus systemButton, CCGamePadButtonStatus aButton, CCGamePadButtonStatus bButton, CCGamePadButtonStatus xButton, CCGamePadButtonStatus yButton, CCGamePadButtonStatus leftShoulder, CCGamePadButtonStatus rightShoulder, Microsoft.Xna.Framework.PlayerIndex player)
        {
            if (aButton == CCGamePadButtonStatus.Pressed)
            {
                _aButtonWasPressed = true;
            }
            else if (aButton == CCGamePadButtonStatus.Released && _aButtonWasPressed)
            {
                // Select the menu
                _Items[_CurrentItemIndex].Activate();
                _Items[_CurrentItemIndex].Unselected();
            }
        }

        private long _FirstTicks;
        private bool _bDownPress = false;
        private bool _bUpPress = false;

        private void MyOnGamePadDPadUpdate(CCGamePadButtonStatus leftButton, CCGamePadButtonStatus upButton, CCGamePadButtonStatus rightButton, CCGamePadButtonStatus downButton, Microsoft.Xna.Framework.PlayerIndex player)
        {
            // Down and Up only
            if (downButton == CCGamePadButtonStatus.Pressed)
            {
                if (_FirstTicks == 0L)
                {
                    _FirstTicks = DateTime.Now.Ticks;
                    _bDownPress = true;
                }
            }
            else if (downButton == CCGamePadButtonStatus.Released && _FirstTicks > 0L && _bDownPress)
            {
                _FirstTicks = 0L;
                NextMenuItem();
                _bDownPress = false;
            }
            if (upButton == CCGamePadButtonStatus.Pressed)
            {
                if (_FirstTicks == 0L)
                {
                    _FirstTicks = DateTime.Now.Ticks;
                    _bUpPress = true;
                }
            }
            else if (upButton == CCGamePadButtonStatus.Released && _FirstTicks > 0L && _bUpPress)
            {
                _FirstTicks = 0L;
                PreviousMenuItem();
                _bUpPress = false;
            }
        }

        ~TestController()
        {
        }

        public void menuCallback(object pSender)
        {
            // get the userdata, it's the index of the menu item clicked
            CCMenuItem pMenuItem = (CCMenuItem)(pSender);
            int nIdx = (int)pMenuItem.UserData;

            // create the test scene and run it
            TestScene pScene = CreateTestScene(nIdx);
            if (pScene != null)
            {
                CCApplication.SharedApplication.GamePadDPadUpdate -= _GamePadDPadDelegate;
                CCApplication.SharedApplication.GamePadButtonUpdate -= _GamePadButtonDelegate;
                pScene.runThisTest();
            }
        }

        public void closeCallback(object pSender)
        {
            CCDirector.SharedDirector.End();
            CCApplication.SharedApplication.Game.Exit();
        }

        public override void TouchesBegan(List<CCTouch> pTouches)
        {
            CCTouch touch = pTouches.FirstOrDefault();

            m_tBeginPos = touch.Location;
        }

        public override void TouchesMoved(List<CCTouch> pTouches)
        {
            CCTouch touch = pTouches.FirstOrDefault();

            var touchLocation = touch.Location;
            float nMoveY = touchLocation.Y - m_tBeginPos.Y;

            CCPoint curPos = m_pItemMenu.Position;
            CCPoint nextPos = new CCPoint(curPos.X, curPos.Y + nMoveY);
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            if (nextPos.Y < 0.0f)
            {
                m_pItemMenu.Position = new CCPoint(0, 0);
                return;
            }

            if (nextPos.Y > (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - winSize.Height))
            {
                m_pItemMenu.Position = (new CCPoint(0, (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - winSize.Height)));
                return;
            }

            m_pItemMenu.Position = nextPos;
            m_tBeginPos = touchLocation;
            s_tCurPos = nextPos;
        }

        public static TestScene CreateTestScene(int nIdx)
        {
            CCDirector.SharedDirector.PurgeCachedData();

            TestScene pScene = null;

            switch (nIdx)
            {
                case (int)TestCases.TEST_ACTIONS:
                    pScene = new ActionsTestScene(); break;
                case (int)TestCases.TEST_TRANSITIONS:
                    pScene = new TransitionsTestScene(); break;
                case (int)TestCases.TEST_PROGRESS_ACTIONS:
                    pScene = new ProgressActionsTestScene(); break;
                case (int)TestCases.TEST_EFFECTS:
                    pScene = new EffectTestScene(); break;
                case (int)TestCases.TEST_CLICK_AND_MOVE:
                    pScene = new ClickAndMoveTest(); break;
                case (int)TestCases.TEST_ROTATE_WORLD:
                    pScene = new RotateWorldTestScene(); break;
                case (int)TestCases.TEST_PARTICLE:
                    pScene = new ParticleTestScene(); break;
                case (int)TestCases.TEST_EASE_ACTIONS:
                    pScene = new EaseActionsTestScene(); break;
                case (int)TestCases.TEST_MOTION_STREAK:
                    pScene = new MotionStreakTestScene(); break;
                case (int)TestCases.TEST_DRAW_PRIMITIVES:
                    pScene = new DrawPrimitivesTestScene(); break;
                case (int)TestCases.TEST_COCOSNODE:
                    pScene = new CocosNodeTestScene(); break;
                case (int)TestCases.TEST_TOUCHES:
                    pScene = new PongScene(); break;
                case (int)TestCases.TEST_MENU:
                    pScene = new MenuTestScene(); break;
                case (int)TestCases.TEST_ACTION_MANAGER:
                    pScene = new ActionManagerTestScene(); break;
                case (int)TestCases.TEST_LAYER:
                    pScene = new LayerTestScene(); break;
                case (int)TestCases.TEST_SCENE:
                    pScene = new SceneTestScene(); break;
                case (int)TestCases.TEST_PARALLAX:
                    pScene = new ParallaxTestScene(); break;
                case (int)TestCases.TEST_TILE_MAP:
                    pScene = new TileMapTestScene(); break;
                case (int)TestCases.TEST_INTERVAL:
                    pScene = new IntervalTestScene(); break;
                //    case TEST_CHIPMUNK:
                //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
                //        pScene = new ChipmunkTestScene(); break;
                //#else
                //#ifdef AIRPLAYUSECHIPMUNK
                //#if	(AIRPLAYUSECHIPMUNK == 1)
                //        pScene = new ChipmunkTestScene(); break;
                //#endif
                //#endif
                //#endif
                case (int)TestCases.TEST_LABEL:
                    pScene = new AtlasTestScene(); break;
                    case (int)TestCases.TEST_TEXT_INPUT:
                        pScene = new TextInputTestScene(); break;
                case (int)TestCases.TEST_SPRITE:
                    pScene = new SpriteTestScene(); break;
                case (int)TestCases.TEST_SCHEDULER:
                    pScene = new SchedulerTestScene(); break;
                case (int)TestCases.TEST_RENDERTEXTURE:
                    pScene = new RenderTextureScene(); break;
                case (int)TestCases.TEST_TEXTURE2D:
                    pScene = new TextureTestScene(); break;
                case (int)TestCases.TEST_BOX2D:
                    pScene = new Box2DTestScene(); break;
                case (int)TestCases.TEST_BOX2DBED:
                         pScene = new tests.classes.tests.Box2DTestBet.Box2dTestBedScene(); break;
                case (int)TestCases.TEST_BOX2DBED2:
                    pScene = new Box2D.TestBed.Box2dTestBedScene(); break;
                case (int)TestCases.TEST_EFFECT_ADVANCE:
                    pScene = new EffectAdvanceScene(); break;
                case (int)TestCases.TEST_ACCELEROMRTER:
                    pScene = new AccelerometerTestScene(); break;
                //    case TEST_KEYPAD:
                //        pScene = new KeypadTestScene(); break;
                case (int)TestCases.TEST_COCOSDENSHION:
                    pScene = new CocosDenshionTestScene(); break;
                case (int)TestCases.TEST_PERFORMANCE:
                    pScene = new PerformanceTestScene(); break;
                case (int)TestCases.TEST_ZWOPTEX:
                    pScene = new ZwoptexTestScene(); break;
                //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
                //    case TEST_CURL:
                //        pScene = new CurlTestScene(); break;
                //case (int)TestCases.TEST_USERDEFAULT:
                //    pScene = new UserDefaultTestScene(); break;
                //#endif
                //    case TEST_BUGS:
                //        pScene = new BugsTestScene(); break;
                //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
                
                case (int)TestCases.TEST_FONTS:
                        pScene = new FontTestScene(); break;
#if IPHONE || IOS || MACOS || WINDOWSGL || WINDOWS || (ANDROID && !OUYA) || NETFX_CORE
                case (int)TestCases.TEST_SYSTEM_FONTS:
                    pScene = new SystemFontTestScene(); break;
#endif
                //    case TEST_CURRENT_LANGUAGE:
                //        pScene = new CurrentLanguageTestScene(); break;
                //        break;
                //#endif
                case (int)TestCases.TEST_CLIPPINGNODE:
                        pScene = new ClippingNodeTestScene();
                        break;

                case (int)TestCases.TEST_EXTENSIONS:
                        pScene = new ExtensionsTestScene();
                        break;
                case (int)TestCases.TEST_ORIENTATION:
                        pScene = new OrientationTestScene();
                        break;
                case(int)TestCases.TEST_MULTITOUCH:
                    pScene = new MultiTouchTestScene();
                    break;
                default:
                    break;
            }

            return pScene;
        }

        private CCPoint m_tBeginPos;
        private CCMenu m_pItemMenu;
    }
}