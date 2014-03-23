using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
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
        private List<CCMenuItem> _Items = new List<CCMenuItem>();
        private int _CurrentItemIndex = 0;
        private CCSprite _menuIndicator;

        public TestController()
        {
            // add close menu
            var pCloseItem = new CCMenuItemImage(TestResource.s_pPathClose, TestResource.s_pPathClose, closeCallback);
            var pMenu = new CCMenu(pCloseItem);
            var s = CCDirector.SharedDirector.WinSize;

            pMenu.Position = CCPoint.Zero;
            pCloseItem.Position = new CCPoint(s.Width - 30, s.Height - 30);
#if !PSM && !WINDOWS_PHONE
#if NETFX_CORE
            CCLabelTtf versionLabel = new CCLabelTtf("v" + this.GetType().GetAssemblyName().Version.ToString(), "arial", 12);
#else
            CCLabelTtf versionLabel = new CCLabelTtf("v" + this.GetType().Assembly.GetName().Version.ToString(), "arial", 12);
#endif
            versionLabel.Position = new CCPoint(versionLabel.ContentSizeInPixels.Width/2f, s.Height - 18f);
            versionLabel.HorizontalAlignment = CCTextAlignment.Left;
            AddChild(versionLabel, 20000);
#endif
            // add menu items for tests
            m_pItemMenu = new CCMenu();
            for (int i = 0; i < (int)(TestCases.TESTS_COUNT); ++i)
            {
                var label = new CCLabelTtf(Tests.g_aTestNames[i], "arial", 24);
                var pMenuItem = new CCMenuItemLabelTTF(label, menuCallback);

                pMenuItem.UserData = i;
				m_pItemMenu.AddChild(pMenuItem, i + 10000);
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

			// add listeners
#if !XBOX && !OUYA
			var touchListener = new CCEventListenerTouchOneByOne();
			touchListener.IsSwallowTouches = true;
			touchListener.OnTouchBegan = onTouchBegan;
			touchListener.OnTouchMoved = onTouchMoved;

			EventDispatcher.AddEventListener(touchListener, this);

			var mouseListener = new CCEventListenerMouse();
			mouseListener.OnMouseScroll = onMouseScroll;
			EventDispatcher.AddEventListener (mouseListener, this);

#else
			//KeypadEnabled = true;

#endif
			#if WINDOWS || WINDOWSGL || MACOS
			EnableGamePad();
			#endif
			   
			// set the first one to have the selection highlight
            _CurrentItemIndex = 0;
            SelectMenuItem();


        }


        private CCPoint _HomePosition;
        private CCPoint _LastPosition;

        private void SelectMenuItem()
        {
            _Items[_CurrentItemIndex].Selected = true;
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
            _Items[_CurrentItemIndex].Selected = false;
            _CurrentItemIndex = (_CurrentItemIndex + 1) % _Items.Count;
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            m_pItemMenu.Position = (new CCPoint(0, _HomePosition.Y + _CurrentItemIndex * LINE_SPACE));
            s_tCurPos = m_pItemMenu.Position;
            SelectMenuItem();
        }
        private void PreviousMenuItem() 
        {
            _Items[_CurrentItemIndex].Selected = false;
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
        }
        public override void OnEnter()
        {
            base.OnEnter();
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
                pScene.runThisTest();
            }
        }

        public void closeCallback(object pSender)
        {
            CCDirector.SharedDirector.End();
            CCApplication.SharedApplication.Game.Exit();
        }

		void EnableGamePad()
		{

			var AButtonWasPressed = false;

			var gamePadListener = new CCEventListenerGamePad ();

			gamePadListener.OnButtonStatus = (buttonStatus) => 
			{
				Console.WriteLine("gamepad button status");
				if (buttonStatus.A == CCGamePadButtonStatus.Pressed)
				{
					AButtonWasPressed = true;
				}
				else if (buttonStatus.A == CCGamePadButtonStatus.Released && AButtonWasPressed)
				{
					// Select the menu
					_Items[_CurrentItemIndex].Activate();
					_Items[_CurrentItemIndex].Selected = false;
				}
			};

			long firstTicks = 0;
			bool isDownPressed = false;
			bool isUpPressed = false;

			gamePadListener.OnDPadStatus = (dpadStatus) => 
			{
				// Down and Up only
				if (dpadStatus.Down == CCGamePadButtonStatus.Pressed) 
				{
					if (firstTicks == 0L) 
					{
						firstTicks = DateTime.Now.Ticks;
						isDownPressed = true;
					}
				} 
				else if (dpadStatus.Down == CCGamePadButtonStatus.Released && firstTicks > 0L && isDownPressed) 
				{
					firstTicks = 0L;
					NextMenuItem ();
					isDownPressed = false;
				}
				if (dpadStatus.Up == CCGamePadButtonStatus.Pressed) 
				{
					if (firstTicks == 0L) {
						firstTicks = DateTime.Now.Ticks;
						isUpPressed = true;
					}
				} 
				else if (dpadStatus.Up == CCGamePadButtonStatus.Released && firstTicks > 0L && isUpPressed) 
				{
					firstTicks = 0L;
					PreviousMenuItem ();
					isUpPressed = false;
				}


			};

			gamePadListener.OnConnectionStatus = (connectionStatus) => 
			{
				Console.WriteLine("Player {0} is connected {1}", connectionStatus.Player, connectionStatus.IsConnected);
			};

			EventDispatcher.AddEventListener (gamePadListener, this);

			// We will enable the gamepad last so that we get connection events.
			GamePadEnabled = true;

		}

		bool onTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            m_tBeginPos = touch.Location;
			return true;
        }

		void onTouchMoved(CCTouch touch, CCEvent touchEvent)
        {

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

			if (nextPos.Y > (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - CCVisibleRect.VisibleRect.Size.Height))
            {
				m_pItemMenu.Position = (new CCPoint(0, (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - CCVisibleRect.VisibleRect.Size.Height)));
                return;
            }

            m_pItemMenu.Position = nextPos;
            m_tBeginPos = touchLocation;
            s_tCurPos = nextPos;
        }

		void onMouseScroll(CCEventMouse mouseEvent)
		{

			// Due to a bug in MonoGame the menu will jump around on Mac when hitting the top element
			// https://github.com/mono/MonoGame/issues/2276
			var delta = mouseEvent.ScrollY;

			CCSize winSize = CCDirector.SharedDirector.WinSize;
			var curPos = m_pItemMenu.Position;
			var nextPos = curPos;
			nextPos.Y += (delta / CCDirector.SharedDirector.ContentScaleFactor) / LINE_SPACE;

			if (nextPos.Y < 0) 
			{
				m_pItemMenu.Position = CCPoint.Zero;
				return;
			}

			if (nextPos.Y > (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - CCVisibleRect.VisibleRect.Size.Height))
			{
				m_pItemMenu.Position = (new CCPoint(0, (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - CCVisibleRect.VisibleRect.Size.Height)));
				return;
			}

			m_pItemMenu.Position = nextPos;
			s_tCurPos   = nextPos;
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
				case(int)TestCases.TEST_EVENTDISPATCHER:
					pScene = new EventDispatcherTestScene();
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