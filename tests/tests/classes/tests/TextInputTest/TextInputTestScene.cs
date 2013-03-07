using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;
using tests;

namespace cocos2d
{
    public class TextInputTestScene : TestScene
    {
        int kTextFieldTTFDefaultTest = 0;
        int kTextFieldTTFActionTest;
        int kTextInputTestsCount;

        public static string FONT_NAME = "Thonburi";
        public static int FONT_SIZE = 36;

        public static int testIdx = -1;

        public override void runThisTest()
        {
            CCLayer pLayer = nextTextInputTest();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }

        public KeyboardNotificationLayer createTextInputTest(int nIndex)
        {
            switch (nIndex)
            {
                //case kTextFieldTTFDefaultTest:
                //    return new TextFieldTTFDefaultTest();
                //case kTextFieldTTFActionTest:
                //    return new TextFieldTTFActionTest();
                //default: return 0;

                case 0:
                    return new TextFieldTTFDefaultTest();
                case 1:
                    return new TextFieldTTFActionTest();
                default: break;
            }

            return null;
        }

        protected override void NextTestCase()
        {
            nextTextInputTest();
        }
        protected override void PreviousTestCase()
        {
            backTextInputTest();
        }
        protected override void RestTestCase()
        {
            restartTextInputTest();
        }
        public CCLayer restartTextInputTest()
        {
            TextInputTest pContainerLayer = new TextInputTest();
            //pContainerLayer->autorelease();

            KeyboardNotificationLayer pTestLayer = createTextInputTest(testIdx);
            //pTestLayer->autorelease();
            pContainerLayer.addKeyboardNotificationLayer(pTestLayer);

            return pContainerLayer;
        }

        public CCLayer nextTextInputTest()
        {
            testIdx++;
            testIdx = testIdx % kTextInputTestsCount;

            return restartTextInputTest();
        }

        public CCLayer backTextInputTest()
        {
            testIdx--;
            int total = kTextInputTestsCount;
            if (testIdx < 0)
                testIdx += total;

            return restartTextInputTest();
        }

        public static CCRect getRect(CCNode node)
        {
            CCRect rc = new CCRect();
            rc.Origin = node.Position;
            rc.Size = node.ContentSize;
            rc.Origin.X -= rc.Size.Width / 2;
            rc.Origin.Y -= rc.Size.Height / 2;
            return rc;
        }
    }
}
