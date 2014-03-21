using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace tests
{
    public class OrientationTest : CCLayer
    {
        static int MAX_LAYER = 1;
        static int sceneIdx = -1;
		public static CCDisplayOrientation s_currentOrientation = CCDisplayOrientation.LandscapeLeft;

        public OrientationTest()
        {
            InitOrientationTest();
        }


        public static CCLayer CreateTestCaseLayer(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        Orientation1 pRet = new Orientation1();
                        return pRet;
                    }
                default:
                    return null;
            }
        }

        public static CCLayer NextOrientationTestCase()
        {
            sceneIdx++;
            sceneIdx = sceneIdx % MAX_LAYER;

            return CreateTestCaseLayer(sceneIdx);
        }

        public static CCLayer BackOrientationTestCase()
        {
            sceneIdx--;
            if (sceneIdx < 0)
                sceneIdx += MAX_LAYER;

            return CreateTestCaseLayer(sceneIdx);
        }

        public static CCLayer RestartOrientationTestCase()
        {
            return CreateTestCaseLayer(sceneIdx);
        }

        private bool InitOrientationTest ()
        {
            bool bRet = false;
            do
            {

                CCSize s = CCDirector.SharedDirector.WinSize;

                CCLabelTtf label = new CCLabelTtf(title(), "Arial", 26);
                AddChild(label, 1);
                label.Position = new CCPoint(s.Width / 2, s.Height - 50);

                string sSubtitle = subtitle();
                if (sSubtitle.Length > 0)
                {
                    CCLabelTtf l = new CCLabelTtf(sSubtitle, "Arial", 16);
                    AddChild(l, 1);
                    l.Position = new CCPoint(s.Width / 2, s.Height - 80);
                }

                CCMenuItemImage item1 = new CCMenuItemImage(TestResource.s_pPathB1, TestResource.s_pPathB2,  BackCallback);
                CCMenuItemImage item2 = new CCMenuItemImage(TestResource.s_pPathR1, TestResource.s_pPathR2,  RestartCallback);
                CCMenuItemImage item3 = new CCMenuItemImage(TestResource.s_pPathF1, TestResource.s_pPathF2,  NextCallback);

                CCMenu menu = new CCMenu(item1, item2, item3);
                menu.Position = new CCPoint();
                item1.Position = new CCPoint(s.Width / 2 - 100, 30);
                item2.Position = new CCPoint(s.Width / 2, 30);
                item3.Position = new CCPoint(s.Width / 2 + 100, 30);

                bRet = true;
            } while (false);

            return bRet;
        }

        public void RestartCallback(object pSender)
        {
            CCScene s = new OrientationTestScene();
            s.AddChild(RestartOrientationTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void NextCallback(object pSender)
        {
            CCScene s = new OrientationTestScene();
            s.AddChild(NextOrientationTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void BackCallback(object pSender)
        {
            CCScene s = new OrientationTestScene();
            s.AddChild(BackOrientationTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public virtual string title()
        {
            return "No title";
        }

        public virtual string subtitle()
        {
            return "";
        }
    }

    public class Orientation1 : OrientationTest
    {

        public Orientation1()
        {
            InitOrientation1();
        }

        private bool InitOrientation1()
        {
            bool bRet = false;
            do
            {

				// Register Touch Event
				var touchListener = new CCEventListenerTouchAllAtOnce();
				touchListener.OnTouchesEnded = onTouchesEnded;

				EventDispatcher.AddEventListener(touchListener, this);

                CCSize s = CCDirector.SharedDirector.WinSize;

                CCMenuItem item = new CCMenuItemFont("Rotate Device", RotateDevice);
                CCMenu menu = new CCMenu(item);
                menu.Position = new CCPoint(s.Width / 2, s.Height / 2);
                AddChild(menu);

                bRet = true;
            } while (false);

            return bRet;
        }

        public void NewOrientation()
        {
            switch (s_currentOrientation)
            {
			case CCDisplayOrientation.LandscapeLeft:
				s_currentOrientation = CCDisplayOrientation.Portrait;
                break;
			case CCDisplayOrientation.Portrait:
				s_currentOrientation = CCDisplayOrientation.LandscapeRight;
                    break;
			case CCDisplayOrientation.LandscapeRight:
				s_currentOrientation = CCDisplayOrientation.LandscapeLeft;
                break;
            }
            CCDrawManager.SetOrientation(s_currentOrientation);
        }

        public void RotateDevice(object pSender)
        {
            NewOrientation();
            RestartCallback(null);
        }


		void onTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        {

            foreach (CCTouch touch in touches)
            {
                if (touch == null)
                    break;
                CCPoint a = touch.LocationInView;

                CCDirector director = CCDirector.SharedDirector;
                CCPoint b = director.ConvertToUi(director.ConvertToGl(a));
                //CCLog("(%d,%d) == (%d,%d)", (int) a.x, (int)a.y, (int)b.x, (int)b.y );
                CCLog.Log("({0},{1}) == ({2},{3})", (int)a.X, (int)a.Y, (int)b.X, (int)b.Y);
            }
        }

        public override string title()
        {
            return "Testing conversion";
        }

        public override string subtitle()
        {
            return "Tap screen and see the debug console";
        }

    }

    public class OrientationTestScene : TestScene
    {
        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }
        public override void runThisTest()
        {
			OrientationTest.s_currentOrientation = CCDisplayOrientation.LandscapeLeft;
            CCLayer pLayer = OrientationTest.NextOrientationTestCase();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }

        public override void MainMenuCallback(object pSender)
        {
            CCDrawManager.GraphicsDevice.PresentationParameters.DisplayOrientation = DisplayOrientation.LandscapeLeft;
            base.MainMenuCallback(pSender);
        }

    }
}
