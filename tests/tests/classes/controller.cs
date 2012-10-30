using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using tests.FontTest;
using tests.Extensions;
using tests.classes.tests.Box2DTestBet;

namespace tests
{
    public class TestController : CCLayer
    {
        static int LINE_SPACE = 40;
        static CCPoint s_tCurPos = new CCPoint(0.0f, 0.0f);

        public TestController()
        {
            // add close menu
            var pCloseItem = CCMenuItemImage.Create(TestResource.s_pPathClose, TestResource.s_pPathClose, closeCallback);
            var pMenu = CCMenu.Create(pCloseItem);
            var s = CCDirector.SharedDirector.WinSize;

            pMenu.Position = new CCPoint(0.0f, 0.0f);
            pCloseItem.Position = new CCPoint(s.width - 30, s.height - 30);

            // add menu items for tests
            m_pItemMenu = CCMenu.Create(null);
            for (int i = 0; i < (int)(TestCases.TESTS_COUNT); ++i)
            {
                var label = CCLabelTTF.Create(Tests.g_aTestNames[i], "arial", 24);
                var pMenuItem = CCMenuItemLabel.Create(label, menuCallback);

                m_pItemMenu.AddChild(pMenuItem, i + 10000);
                pMenuItem.Position = new CCPoint(s.width / 2, (s.height - (i + 1) * LINE_SPACE));
            }

            m_pItemMenu.ContentSize = new CCSize(s.width, ((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE);
            m_pItemMenu.Position = s_tCurPos;
            AddChild(m_pItemMenu);

            TouchEnabled = true;

            AddChild(pMenu, 1);
        }

        ~TestController()
        {
        }

        public void menuCallback(CCObject pSender)
        {
            // get the userdata, it's the index of the menu item clicked
            CCMenuItem pMenuItem = (CCMenuItem)(pSender);
            int nIdx = pMenuItem.ZOrder - 10000;

            // create the test scene and run it
            TestScene pScene = CreateTestScene(nIdx);
            if (pScene != null)
            {
                pScene.runThisTest();
            }
        }

        public void closeCallback(CCObject pSender)
        {
            CCDirector.SharedDirector.End();
            CCApplication.SharedApplication.Game.Exit();
        }

        public override void TouchesBegan(List<CCTouch> pTouches, CCEvent pEvent)
        {
            CCTouch touch = pTouches.FirstOrDefault();

            m_tBeginPos = touch.LocationInView;
            m_tBeginPos = CCDirector.SharedDirector.ConvertToGl(m_tBeginPos);
        }

        public override void TouchesMoved(List<CCTouch> pTouches, CCEvent pEvent)
        {
            CCTouch touch = pTouches.FirstOrDefault();

            CCPoint touchLocation = touch.LocationInView;
            touchLocation = CCDirector.SharedDirector.ConvertToGl(touchLocation);
            float nMoveY = touchLocation.y - m_tBeginPos.y;

            CCPoint curPos = m_pItemMenu.Position;
            CCPoint nextPos = new CCPoint(curPos.x, curPos.y + nMoveY);
            CCSize winSize = CCDirector.SharedDirector.WinSize;
            if (nextPos.y < 0.0f)
            {
                m_pItemMenu.Position = new CCPoint(0, 0);
                return;
            }

            if (nextPos.y > (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - winSize.height))
            {
                m_pItemMenu.Position = (new CCPoint(0, (((int)TestCases.TESTS_COUNT + 1) * LINE_SPACE - winSize.height)));
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
                //#if (CC_TARGET_PLATFORM != CC_PLATFORM_AIRPLAY)
                //    case TEST_TEXT_INPUT:
                //        pScene = new TextInputTestScene(); break;
                //#endif
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
                         pScene = new Box2dTestBedScene(); break;
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
                
                //    case TEST_CURRENT_LANGUAGE:
                //        pScene = new CurrentLanguageTestScene(); break;
                //        break;
                //#endif

                case (int)TestCases.TEST_EXTENSIONS:
                        pScene = new ExtensionsTestScene();
                        break;
                case (int)TestCases.TEST_ORIENTATION:
                        pScene = new OrientationTestScene();
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