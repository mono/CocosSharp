using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TextInputTestScene : TestScene
    {

        public static int MAX_TESTS = 0;

        public static string FONT_NAME = "Thonburi";
        public static int FONT_SIZE = 36;

        public static int testIdx = -1;

        public TextInputTestScene() : base()
        {
            MAX_TESTS = textinputCreateFunctions.Length;
        }

        public override void runThisTest()
        {
            CCLayer pLayer = nextTextInputTest();
            AddChild(pLayer);

            Scene.Director.ReplaceScene(this);
        }

        static Func<KeyboardNotificationLayer>[] textinputCreateFunctions =
            {
                () => new TextFieldDefaultTest(),
                () => new TextFieldActionTest(),
                () => new TextFieldUpperCaseTest(),

            };
        
        public KeyboardNotificationLayer createTextInputTest(int nIndex)
        {
            return textinputCreateFunctions[nIndex]();
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

            KeyboardNotificationLayer pTestLayer = createTextInputTest(testIdx);
            pContainerLayer.addKeyboardNotificationLayer(pTestLayer);

            return pContainerLayer;
        }

        public CCLayer nextTextInputTest()
        {
            testIdx++;
            testIdx = testIdx % MAX_TESTS;

            return restartTextInputTest();
        }

        public CCLayer backTextInputTest()
        {
            testIdx--;
            int total = MAX_TESTS;
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
