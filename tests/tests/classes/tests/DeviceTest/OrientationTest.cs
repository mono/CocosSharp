using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace tests
{
    public class OrientationTest : CCLayer
    {
        static int MAX_LAYER = 1;
        static int sceneIdx = -1;
        public static DisplayOrientation s_currentOrientation = DisplayOrientation.LandscapeLeft;

        public static CCLayer CreateTestCaseLayer(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        Orientation1 pRet = new Orientation1();
                        pRet.Init();
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

        public override bool Init()
        {
            bool bRet = false;
            do
            {
                if (!base.Init())
                    break;

                CCSize s = CCDirector.SharedDirector.WinSize;

                CCLabelTTF label = CCLabelTTF.Create(title(), "Arial", 26);
                AddChild(label, 1);
                label.Position = new CCPoint(s.width / 2, s.height - 50);

                string sSubtitle = subtitle();
                if (sSubtitle.Length > 0)
                {
                    CCLabelTTF l = CCLabelTTF.Create(sSubtitle, "Arial", 16);
                    AddChild(l, 1);
                    l.Position = new CCPoint(s.width / 2, s.height - 80);
                }

                CCMenuItemImage item1 = CCMenuItemImage.Create(TestResource.s_pPathB1, TestResource.s_pPathB2,  new SEL_MenuHandler(BackCallback));
                CCMenuItemImage item2 = CCMenuItemImage.Create(TestResource.s_pPathR1, TestResource.s_pPathR2,  new SEL_MenuHandler(RestartCallback));
                CCMenuItemImage item3 = CCMenuItemImage.Create(TestResource.s_pPathF1, TestResource.s_pPathF2,  new SEL_MenuHandler(NextCallback));

                CCMenu menu = CCMenu.Create(item1, item2, item3);
                menu.Position = new CCPoint();
                item1.Position = new CCPoint(s.width / 2 - 100, 30);
                item2.Position = new CCPoint(s.width / 2, 30);
                item3.Position = new CCPoint(s.width / 2 + 100, 30);

                bRet = true;
            } while (false);

            return bRet;
        }

        public void RestartCallback(CCObject pSender)
        {
            CCScene s = new OrientationTestScene();
            s.AddChild(RestartOrientationTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void NextCallback(CCObject pSender)
        {
            CCScene s = new OrientationTestScene();
            s.AddChild(NextOrientationTestCase());
            CCDirector.SharedDirector.ReplaceScene(s);
        }

        public void BackCallback(CCObject pSender)
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
        public override bool Init()
        {
            bool bRet = false;
            do
            {
                if (!base.Init())
                    break;
                TouchEnabled = true;
                CCSize s = CCDirector.SharedDirector.WinSize;


                CCMenuItem item = CCMenuItemFont.Create("Rotate Device", new SEL_MenuHandler(RotateDevice));
                CCMenu menu = CCMenu.Create(item);
                menu.Position = new CCPoint(s.width / 2, s.height / 2);
                AddChild(menu);

                bRet = true;
            } while (false);

            return bRet;
        }

        public void NewOrientation()
        {
            switch (s_currentOrientation)
            {
                case DisplayOrientation.LandscapeLeft:
                    s_currentOrientation = DisplayOrientation.Portrait;
                    break;
                case DisplayOrientation.Portrait:
                    s_currentOrientation = DisplayOrientation.LandscapeRight;
                    break;
                case DisplayOrientation.LandscapeRight:
                    s_currentOrientation = DisplayOrientation.LandscapeLeft;
                    break;
            }
            DrawManager.SetOrientation(s_currentOrientation);
        }

        public void RotateDevice(CCObject pSender)
        {
            NewOrientation();
            RestartCallback(null);
        }


        public override void TouchesEnded(List<CCTouch> touches, CCEvent eventarg)
        {

            foreach (CCTouch touch in touches)
            {
                if (touch == null)
                    break;
                CCPoint a = touch.LocationInView;

                CCDirector director = CCDirector.SharedDirector;
                CCPoint b = director.ConvertToUi(director.ConvertToGl(a));
                //CCLog("(%d,%d) == (%d,%d)", (int) a.x, (int)a.y, (int)b.x, (int)b.y );
                CCLog.Log("({0},{1}) == ({2},{3})", (int)a.x, (int)a.y, (int)b.x, (int)b.y);
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
        public override void runThisTest()
        {
            OrientationTest.s_currentOrientation = DisplayOrientation.LandscapeLeft;
            CCLayer pLayer = OrientationTest.NextOrientationTestCase();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }

        public override void MainMenuCallback(CCObject pSender)
        {
            DrawManager.graphicsDevice.PresentationParameters.DisplayOrientation = DisplayOrientation.LandscapeLeft;
            base.MainMenuCallback(pSender);
        }

    }
}
