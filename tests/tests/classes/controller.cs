using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using tests.Clipping;
using tests.FontTest;
using tests.Extensions;
using Box2D.TestBed;

namespace tests
{
    public class TestController : CCLayer
    {
        static int LINE_SPACE = 40;
        static CCPoint curPos = CCPoint.Zero;

        int currentItemIndex = 0;
        CCPoint homePosition;
        CCPoint lastPosition;

        CCPoint beginTouchPos;

        CCSprite menuIndicator;
        CCLabelTtf versionLabel;

        CCMenu testListMenu;
        List<CCMenuItem> testListMenuItems = new List<CCMenuItem>();

        CCMenu closeMenu;
        CCMenuItem closeMenuItem;


        #region Constructors

        public TestController()
        {
            // Add close menu
            closeMenuItem = new CCMenuItemImage(TestResource.s_pPathClose, TestResource.s_pPathClose, CloseCallback);
            closeMenu = new CCMenu(closeMenuItem);

            CCMenuItemFont.FontName = "MarkerFelt";
            CCMenuItemFont.FontSize = 22;


            #if !PSM && !WINDOWS_PHONE
            #if NETFX_CORE
            versionLabel = new CCLabelTtf("v" + this.GetType().GetAssemblyName().Version.ToString(), "arial", 12);
            #else
            versionLabel = new CCLabelTtf("v" + this.GetType().Assembly.GetName().Version.ToString(), "arial", 12);
            #endif
            AddChild(versionLabel, 20000);
            #endif

            // Add test list menu
            testListMenu = new CCMenu();
            for (int i = 0; i < (int)(TestCases.TESTS_COUNT); ++i)
            {
                CCLabelTtf label = new CCLabelTtf(Tests.g_aTestNames[i], "arial", 24);
                CCMenuItem menuItem = new CCMenuItemLabelTTF(label, MenuCallback);

                menuItem.UserData = i;
                testListMenu.AddChild(menuItem, i + 10000);
                testListMenuItems.Add(menuItem);
            }

            #if XBOX || OUYA
            CCSprite sprite = new CCSprite("Images/aButton");
            AddChild(sprite, 10001);
            menuIndicator = sprite;
            #endif

            AddChild(testListMenu);
            AddChild(closeMenu, 1);
        }

        #endregion Constructors


        #region Setup content

        protected override void RunningOnNewWindow(CCSize windowSize)
        {
            base.RunningOnNewWindow(windowSize);

            // Laying out content based on window size
            closeMenu.Position = CCPoint.Zero;
            closeMenuItem.Position = new CCPoint(windowSize.Width - 30, windowSize.Height - 30);

            versionLabel.Position = new CCPoint(versionLabel.ContentSizeInPixels.Width/2f, windowSize.Height - 18f);
            versionLabel.HorizontalAlignment = CCTextAlignment.Left;

            testListMenu.ContentSize = new CCSize(windowSize.Width, ((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE);

            int i = 0;
            foreach (CCMenuItem testItem in testListMenuItems) 
            {
                #if XBOX || OUYA
                testItem.Position = new CCPoint(windowSize.Width / 2, -(i + 1) * LINE_SPACE);
                #else
                testItem.Position = new CCPoint(windowSize.Width / 2, (windowSize.Height - (i + 1) * LINE_SPACE));
                #endif

                i++;
            }
                
            #if XBOX || OUYA
            // Center the menu on the first item so that it is 
            // in the center of the screen
            homePosition = new CCPoint(0f, windowSize.Height / 2f + LINE_SPACE / 2f);
            lastPosition = new CCPoint(0f, homePosition.Y - (testListMenuItems.Count - 1) * LINE_SPACE);
            #else
			homePosition = curPos;
            #endif

            testListMenu.Position = homePosition;

            // Add listeners
            #if !XBOX && !OUYA
            var touchListener = new CCEventListenerTouchOneByOne ();
            touchListener.IsSwallowTouches = true;
            touchListener.OnTouchBegan = OnTouchBegan;
            touchListener.OnTouchMoved = OnTouchMoved;

            AddEventListener(touchListener);

            var mouseListener = new CCEventListenerMouse ();
            mouseListener.OnMouseScroll = OnMouseScroll;
            AddEventListener(mouseListener);

            #endif

            #if WINDOWS || WINDOWSGL || MACOS
            EnableGamePad();
            #endif

            // set the first one to have the selection highlight
            currentItemIndex = 0;
            SelectMenuItem();
        }

        #endregion Setup content


        #region Menu item handling

        void SelectMenuItem()
        {
            if (currentItemIndex < testListMenuItems.Count) 
            {
                testListMenuItems [currentItemIndex].Selected = true;
                if (menuIndicator != null) {
                    menuIndicator.Position = new CCPoint (
                        testListMenu.Position.X + testListMenuItems [currentItemIndex].Position.X 
                        - testListMenuItems[currentItemIndex].ContentSizeInPixels.Width / 2f - menuIndicator.ContentSizeInPixels.Width / 2f - 5f,
                        testListMenu.Position.Y + testListMenuItems [currentItemIndex].Position.Y
                    );
                }
            }
        }

        void NextMenuItem() 
        {
            testListMenuItems[currentItemIndex].Selected = false;
            currentItemIndex = (currentItemIndex + 1) % testListMenuItems.Count;
            CCSize winSize = Director.WindowSizeInPoints;
            testListMenu.Position = (new CCPoint(0, homePosition.Y + currentItemIndex * LINE_SPACE));
            curPos = testListMenu.Position;
            SelectMenuItem();
        }

        void PreviousMenuItem() 
        {
            testListMenuItems[currentItemIndex].Selected = false;
            currentItemIndex--;
            if(currentItemIndex < 0) {
                currentItemIndex = testListMenuItems.Count - 1;
            }
            CCSize winSize = Director.WindowSizeInPoints;
            testListMenu.Position = (new CCPoint(0, homePosition.Y + currentItemIndex * LINE_SPACE));
            curPos = testListMenu.Position;
            SelectMenuItem();
        }

        #endregion Menu item handling


        void MenuCallback(object sender)
        {
            // get the userdata, it's the index of the menu item clicked
            CCMenuItem menuItem = (CCMenuItem)(sender);
            int nIdx = (int)menuItem.UserData;

            // create the test scene and run it
            TestScene scene = CreateTestScene(nIdx);
            if (scene != null)
            {
                scene.runThisTest();
            }
        }

        void CloseCallback(object sender)
        {
            CCApplication.SharedApplication.ExitGame();
        }

        void EnableGamePad()
        {
            var AButtonWasPressed = false;

            var gamePadListener = new CCEventListenerGamePad ();

            gamePadListener.OnButtonStatus = (buttonStatus) => 
            {
                if (buttonStatus.A == CCGamePadButtonStatus.Pressed)
                {
                    AButtonWasPressed = true;
                }
                else if (buttonStatus.A == CCGamePadButtonStatus.Released && AButtonWasPressed)
                {
                    // Select the menu
                    testListMenuItems[currentItemIndex].Activate();
                    testListMenuItems[currentItemIndex].Selected = false;
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
                CCLog.Log("Player {0} is connected {1}", connectionStatus.Player, connectionStatus.IsConnected);
            };

            AddEventListener(gamePadListener);
        }


        #region Event handling

        bool OnTouchBegan(CCTouch touch, CCEvent touchEvent)
        {
            beginTouchPos = touch.Location;
            return true;
        }

        void OnTouchMoved(CCTouch touch, CCEvent touchEvent)
        {

            var touchLocation = touch.Location;
            float nMoveY = touchLocation.Y - beginTouchPos.Y;

            curPos = testListMenu.Position;
            CCPoint nextPos = new CCPoint(curPos.X, curPos.Y + nMoveY);
            CCSize winSize = Director.WindowSizeInPoints;
            if (nextPos.Y < 0.0f)
            {
                testListMenu.Position = new CCPoint(0, 0);
                return;
            }

            if (nextPos.Y > (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - CCVisibleRect.VisibleRect.Size.Height))
            {
                testListMenu.Position = (new CCPoint(0, (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - CCVisibleRect.VisibleRect.Size.Height)));
                return;
            }

            testListMenu.Position = nextPos;
            beginTouchPos = touchLocation;
            curPos = nextPos;
        }

        void OnMouseScroll(CCEventMouse mouseEvent)
        {

            // Due to a bug in MonoGame the menu will jump around on Mac when hitting the top element
            // https://github.com/mono/MonoGame/issues/2276
            var delta = mouseEvent.ScrollY;

            CCSize winSize = Director.WindowSizeInPoints;
            curPos = testListMenu.Position;
            var nextPos = curPos;
            nextPos.Y += (delta / Director.ContentScaleFactor) / LINE_SPACE;

            if (nextPos.Y < 0) 
            {
                testListMenu.Position = CCPoint.Zero;
                return;
            }

            if (nextPos.Y > (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - CCVisibleRect.VisibleRect.Size.Height))
            {
                testListMenu.Position = (new CCPoint(0, (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - CCVisibleRect.VisibleRect.Size.Height)));
                return;
            }

            testListMenu.Position = nextPos;
            curPos   = nextPos;
        }
       #endregion Event handling


        public static TestScene CreateTestScene(int index)
        {
            CCApplication.SharedApplication.PurgeAllCachedData();

            TestScene scene = null;

            switch(index)
            {
                case (int)TestCases.TEST_ACTIONS:
                    scene = new ActionsTestScene(); break;
                case (int)TestCases.TEST_TRANSITIONS:
                    scene = new TransitionsTestScene(); break;
                case (int)TestCases.TEST_PROGRESS_ACTIONS:
                    scene = new ProgressActionsTestScene(); break;
                case (int)TestCases.TEST_EFFECTS:
                    scene = new EffectTestScene(); break;
                case (int)TestCases.TEST_CLICK_AND_MOVE:
                    scene = new ClickAndMoveTest(); break;
                case (int)TestCases.TEST_ROTATE_WORLD:
                    scene = new RotateWorldTestScene(); break;
                case (int)TestCases.TEST_PARTICLE:
                    scene = new ParticleTestScene(); break;
                case (int)TestCases.TEST_EASE_ACTIONS:
                    scene = new EaseActionsTestScene(); break;
                case (int)TestCases.TEST_MOTION_STREAK:
                    scene = new MotionStreakTestScene(); break;
                case (int)TestCases.TEST_DRAW_PRIMITIVES:
                    scene = new DrawPrimitivesTestScene(); break;
                case (int)TestCases.TEST_COCOSNODE:
                    scene = new CocosNodeTestScene(); break;
                case (int)TestCases.TEST_TOUCHES:
                    scene = new PongScene(); break;
                case (int)TestCases.TEST_MENU:
                    scene = new MenuTestScene(); break;
                case (int)TestCases.TEST_ACTION_MANAGER:
                    scene = new ActionManagerTestScene(); break;
                case (int)TestCases.TEST_LAYER:
                    scene = new LayerTestScene(); break;
                case (int)TestCases.TEST_SCENE:
                    scene = new SceneTestScene(); break;
                case (int)TestCases.TEST_PARALLAX:
                    scene = new ParallaxTestScene(); break;
                case (int)TestCases.TEST_TILE_MAP:
                    scene = new TileMapTestScene(); break;
                case (int)TestCases.TEST_INTERVAL:
                    scene = new IntervalTestScene(); break;
                case (int)TestCases.TEST_LABEL:
                    scene = new AtlasTestScene(); break;
                case (int)TestCases.TEST_TEXT_INPUT:
                    scene = new TextInputTestScene(); break;
                case (int)TestCases.TEST_SPRITE:
                    scene = new SpriteTestScene(); break;
                case (int)TestCases.TEST_SCHEDULER:
                    scene = new SchedulerTestScene(); break;
                case (int)TestCases.TEST_RENDERTEXTURE:
                    scene = new RenderTextureScene(); break;
                case (int)TestCases.TEST_TEXTURE2D:
                    scene = new TextureTestScene(); break;
                case (int)TestCases.TEST_BOX2D:
                    scene = new Box2DTestScene(); break;
                case (int)TestCases.TEST_BOX2DBED2:
                    scene = new Box2D.TestBed.Box2dTestBedScene(); break;
                case (int)TestCases.TEST_EFFECT_ADVANCE:
                    scene = new EffectAdvanceScene(); break;
                case (int)TestCases.TEST_ACCELEROMRTER:
                    scene = new AccelerometerTestScene(); break;
                case (int)TestCases.TEST_COCOSDENSHION:
                    scene = new CocosDenshionTestScene(); break;
                case (int)TestCases.TEST_PERFORMANCE:
                    scene = new PerformanceTestScene(); break;
                case (int)TestCases.TEST_ZWOPTEX:
                    scene = new ZwoptexTestScene(); break;
                case (int)TestCases.TEST_FONTS:
                    scene = new FontTestScene(); break;
                    #if IPHONE || IOS || MACOS || WINDOWSGL || WINDOWS || (ANDROID && !OUYA) || NETFX_CORE
                case (int)TestCases.TEST_SYSTEM_FONTS:
                    scene = new SystemFontTestScene(); break;
                    #endif
                case (int)TestCases.TEST_CLIPPINGNODE:
                    scene = new ClippingNodeTestScene();
                    break;

                case (int)TestCases.TEST_EXTENSIONS:
                    scene = new ExtensionsTestScene();
                    break;
                case (int)TestCases.TEST_ORIENTATION:
                    scene = new OrientationTestScene();
                    break;
                case(int)TestCases.TEST_MULTITOUCH:
                    scene = new MultiTouchTestScene();
                    break;
                case(int)TestCases.TEST_EVENTDISPATCHER:
                    scene = new EventDispatcherTestScene();
                    break;
                default:
                    break;
            }

            return scene;
        }
    }
}