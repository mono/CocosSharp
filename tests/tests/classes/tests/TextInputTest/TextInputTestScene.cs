using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;
using tests;

namespace Cocos2D
{
    public class TextInputTestScene : TestScene
    {

        int kTextFieldTTFDefaultTest = 0;
        int kTextFieldTTFActionTest = 1;
        int kTextInputTestsCount = 2;

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
            var rc = new CCRect();

            rc.Origin = node.Position;
            rc.Size = node.ContentSize;
            rc.Origin.X -= rc.Size.Width * node.AnchorPoint.X;
            rc.Origin.Y -= rc.Size.Height * node.AnchorPoint.Y;
            
            return rc;
        }
    }
}
