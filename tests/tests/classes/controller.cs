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
        const int MENU_ITEM_Z_ORDER = 10000;

        static int LINE_SPACE = 40;
        static CCPoint curPos = CCPoint.Zero;

        int currentItemIndex = 0;
        CCPoint homePosition;

        CCPoint beginTouchPos;

        CCSprite menuIndicator;
        CCLabel versionLabel;

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
            versionLabel = new CCLabel("v" + this.GetType().GetAssemblyName().Version.ToString(), "arial", 30);
            #else
            versionLabel = new CCLabel("v" + this.GetType().Assembly.GetName().Version.ToString(), "arial", 24, CCLabelFormat.SpriteFont);
            #endif
            AddChild(versionLabel, 20000);
            #endif

            // Add test list menu
            testListMenu = new CCMenu();

            var i = 0;
            foreach (var test in testCases.Keys)
            {
                var label = new CCLabel(test, "arial", 24, CCLabelFormat.SpriteFont);
                var menuItem = new CCMenuItemLabel(label, MenuCallback);

                testListMenu.AddChild(menuItem, i++ + MENU_ITEM_Z_ORDER);
                testListMenuItems.Add(menuItem);
            }

            LINE_SPACE = (int)(testListMenuItems[0].ContentSize.Height * 1.5f);

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

        public override void OnEnter()
        {
            base.OnEnter(); 

            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;

            // Laying out content based on window size
            closeMenu.Position = CCPoint.Zero;
            closeMenuItem.Position = new CCPoint(visibleBounds.Size.Width - 40, visibleBounds.Size.Height - 40);

#if !PSM && !WINDOWS_PHONE

            versionLabel.AnchorPoint = CCPoint.AnchorUpperLeft;
            versionLabel.Position = new CCPoint (10.0f, visibleBounds.Size.Height);
#endif
            testListMenu.ContentSize = new CCSize(visibleBounds.Size.Width, (testCases.Count + 1) * LINE_SPACE);

            int i = 0;
            foreach (CCMenuItem testItem in testListMenuItems) 
            {
                testItem.Position = new CCPoint(visibleBounds.Size.Center.X, (visibleBounds.Top().Y - (i + 1) * LINE_SPACE));
                i++;
            }
                
            #if XBOX || OUYA
            // Center the menu on the first item so that it is 
            // in the center of the screen
            homePosition = new CCPoint(0f, visibleBounds.Size.Height / 2f + LINE_SPACE / 2f);
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
            //SelectMenuItem();
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
                        - testListMenuItems[currentItemIndex].ContentSize.Width / 2f - menuIndicator.ContentSize.Width / 2f - 5f,
                        testListMenu.Position.Y + testListMenuItems [currentItemIndex].Position.Y
                    );
                }
            }
        }

        void NextMenuItem() 
        {
            testListMenuItems[currentItemIndex].Selected = false;
            currentItemIndex = (currentItemIndex + 1) % testListMenuItems.Count;

            testListMenu.Position = new CCPoint(0, homePosition.Y + currentItemIndex * LINE_SPACE);
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

            testListMenu.Position = (new CCPoint(0, homePosition.Y + currentItemIndex * LINE_SPACE));
            curPos = testListMenu.Position;
            SelectMenuItem();
        }

        #endregion Menu item handling


        void MenuCallback(object sender)
        {
            // get the userdata, it's the index of the menu item clicked
            CCMenuItem menuItem = (CCMenuItem)sender;
            var nIdx = menuItem.ZOrder - MENU_ITEM_Z_ORDER;

            // create the test scene and run it
            TestScene scene = CreateTestScene(nIdx);
            if (scene != null)
            {
                scene.runThisTest();
            }
        }

        void CloseCallback(object sender)
        {
            Application.ExitGame();
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
            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;
            if (nextPos.Y < 0.0f)
            {
                testListMenu.Position = new CCPoint(0, 0);
                return;
            }

            if (nextPos.Y > ((testCases.Count + 1) * LINE_SPACE - visibleBounds.Size.Height))
            {
                testListMenu.Position = (new CCPoint(0, ((testCases.Count + 1) * LINE_SPACE - visibleBounds.Size.Height)));
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

            CCRect visibleBounds = Layer.VisibleBoundsWorldspace;
            curPos = testListMenu.Position;

            var nextPos = curPos;
            nextPos.Y -= (delta) / LINE_SPACE;

            if (nextPos.Y < 0) 
            {
                testListMenu.Position = CCPoint.Zero;
                return;
            }

            if (nextPos.Y > ((testCases.Count + 1) * LINE_SPACE - visibleBounds.Size.Height))
            {
                testListMenu.Position = (new CCPoint(0, ((testCases.Count + 1) * LINE_SPACE - visibleBounds.Size.Height)));
                return;
            }

            testListMenu.Position = nextPos;
            curPos   = nextPos;
        }

        #endregion Event handling


        public static Dictionary<string, Func<TestScene>> testCases = new Dictionary<string, Func<TestScene>> 
            {

            {"Accelerometer", () => new AccelerometerTestScene()}, 
            {"ActionManagerTest", () => new ActionManagerTestScene()},
            {"ActionsEaseTest", () => new EaseActionsTestScene()},
            {"ActionsProgressTest", () => new ProgressActionsTestScene()},
            {"ActionsTest", () => new ActionsTestScene()},
            {"Box2dTest", () => new Box2DTestScene()},
            {"Box2dTestBed(Box2D)", () => new Box2D.TestBed.Box2dTestBedScene()},
            {"ClickAndMoveTest", () => new ClickAndMoveTest()},
            {"ClippingNodeTest", () => new ClippingNodeTestScene()},
            {"CocosDenshionTest", () => new CocosDenshionTestScene()},
            //{"DirectorTest", () => new AccelerometerTestScene()},
            {"DrawPrimitivesTest", () => new DrawPrimitivesTestScene()},
            {"EffectAdvancedTest", () => new EffectAdvanceScene()},
            {"EffectsTest", () => new EffectTestScene()},
            {"EventDispatcherTest", () => new EventDispatcherTestScene()},
            {"ExtensionsTest", () => new ExtensionsTestScene()},
            {"FontTest", () => new FontTestScene()},
            {"IntervalTest", () => new IntervalTestScene()},
            {"LabelTest", () => new AtlasTestScene()},
            {"LabelTest - New", () => new AtlasTestSceneNew()},
            {"LayerTest", () => new LayerTestScene()},
            {"MenuTest", () => new MenuTestScene()},
            {"MotionStreakTest", () => new MotionStreakTestScene()},
            {"MultiTouchTest", () => new MultiTouchTestScene()},
            {"NodeTest", () => new CocosNodeTestScene()},
            {"OrientationTest", () => new OrientationTestScene()},
            {"ParallaxTest", () => new ParallaxTestScene()},
            {"ParticleTest", () => new ParticleTestScene()},
            {"PerformanceTest", () => new PerformanceTestScene()},
#if USE_PHYSICS
            {"Physics", () => new PhysicsTestScene()},
#endif
            {"RenderTextureTest", () => new RenderTextureScene()},
            {"RotateWorldTest", () => new RotateWorldTestScene()},
            {"SceneTest", () => new SceneTestScene()},
            {"SchedulerTest", () => new SchedulerTestScene()},
            {"SpriteTest", () => new SpriteTestScene()},
            {"SystemFontTest", () => new SystemFontTestScene()},
            {"TextInputTest", () => new TextInputTestScene()},
            {"Texture2DTest", () => new TextureTestScene()},
            {"TileMapTest",  () => new TileMapTestScene()},
            {"TouchesTest",  () => new PongScene()},
            {"TransitionsTest", () => new TransitionsTestScene()},
            {"ZwoptexTest", () => new ZwoptexTestScene()},


            };
        


        public static TestScene CreateTestScene(int index)
        {
            //Application.PurgeAllCachedData();

            TestScene scene = null;

            scene = testCases.Values.ElementAt(index) ();
            return scene;
        }
    }
}