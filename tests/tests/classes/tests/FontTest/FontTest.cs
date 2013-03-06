using cocos2d;

namespace tests.FontTest
{
    public class FontTestScene : TestScene
    {
        private static int fontIdx;

        private static readonly string[] fontList =
            {
#if IOS
                "American Typewriter",
                "Marker Felt",
#endif
                "A Damn Mess",
                "Abberancy",
                "Abduction",
                "Paint Boy",
                "Schwarzwald Regular",
                "Scissor Cuts",
            };

        public static int vAlignIdx = 0;

        public static CCVerticalTextAlignment[] verticalAlignment =
            {
                CCVerticalTextAlignment.CCVerticalTextAlignmentTop,
                CCVerticalTextAlignment.CCVerticalTextAlignmentCenter,
                CCVerticalTextAlignment.CCVerticalTextAlignmentBottom
            };

        public override void runThisTest()
        {
            CCLayer pLayer = new FontTest();
            AddChild(pLayer);

            CCDirector.SharedDirector.ReplaceScene(this);
        }

        protected override void NextTestCase()
        {
            nextAction();
        }
        protected override void PreviousTestCase()
        {
            backAction();
        }
        protected override void RestTestCase()
        {
            restartAction();
        }
        public static string nextAction()
        {
            fontIdx++;
            if (fontIdx >= fontList.Length)
            {
                fontIdx = 0;
                vAlignIdx = (vAlignIdx + 1) % verticalAlignment.Length;
            }
            return fontList[fontIdx];
        }

        public static string backAction()
        {
            fontIdx--;
            if (fontIdx < 0)
            {
                fontIdx = fontList.Length - 1;
                vAlignIdx--;
                if (vAlignIdx < 0)
                    vAlignIdx = verticalAlignment.Length - 1;
            }

            return fontList[fontIdx];
        }

        public static string restartAction()
        {
            return fontList[fontIdx];
        }
    }


    public class FontTest : CCLayer
    {
        private const int kTagLabel1 = 1;
        private const int kTagLabel2 = 2;
        private const int kTagLabel3 = 3;
        private const int kTagLabel4 = 4;

        public FontTest()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            CCMenuItemImage item1 = CCMenuItemImage.Create(TestResource.s_pPathB1, TestResource.s_pPathB2, backCallback);
            CCMenuItemImage item2 = CCMenuItemImage.Create(TestResource.s_pPathR1, TestResource.s_pPathR2, restartCallback);
            CCMenuItemImage item3 = CCMenuItemImage.Create(TestResource.s_pPathF1, TestResource.s_pPathF2, nextCallback);

            CCMenu menu = CCMenu.Create(item1, item2, item3);
            menu.Position = CCPoint.Zero;
            item1.Position = new CCPoint(s.Width / 2 - item2.ContentSize.Width * 2, item2.ContentSize.Height / 2);
            item2.Position = new CCPoint(s.Width / 2, item2.ContentSize.Height / 2);
            item3.Position = new CCPoint(s.Width / 2 + item2.ContentSize.Width * 2, item2.ContentSize.Height / 2);
            AddChild(menu, 1);

            showFont(FontTestScene.restartAction());
        }

        public void showFont(string pFont)
        {
            CCSize s = CCDirector.SharedDirector.WinSize;

            var blockSize = new CCSize(s.Width / 3, 200);
            float fontSize = 26;

            RemoveChildByTag(kTagLabel1, true);
            RemoveChildByTag(kTagLabel2, true);
            RemoveChildByTag(kTagLabel3, true);
            RemoveChildByTag(kTagLabel4, true);

            CCLabelTTF top = CCLabelTTF.Create(pFont, pFont, 24);
            CCLabelTTF left = CCLabelTTF.Create("alignment left", pFont, fontSize,
                                                blockSize, CCTextAlignment.CCTextAlignmentLeft,
                                                FontTestScene.verticalAlignment[FontTestScene.vAlignIdx]);
            CCLabelTTF center = CCLabelTTF.Create("alignment center", pFont, fontSize,
                                                  blockSize, CCTextAlignment.CCTextAlignmentCenter,
                                                  FontTestScene.verticalAlignment[FontTestScene.vAlignIdx]);
            CCLabelTTF right = CCLabelTTF.Create("alignment right", pFont, fontSize,
                                                 blockSize, CCTextAlignment.CCTextAlignmentRight,
                                                 FontTestScene.verticalAlignment[FontTestScene.vAlignIdx]);

            CCLayerColor leftColor = CCLayerColor.Create(new CCColor4B(100, 100, 100, 255), blockSize.Width, blockSize.Height);
            CCLayerColor centerColor = CCLayerColor.Create(new CCColor4B(200, 100, 100, 255), blockSize.Width, blockSize.Height);
            CCLayerColor rightColor = CCLayerColor.Create(new CCColor4B(100, 100, 200, 255), blockSize.Width, blockSize.Height);

            leftColor.IgnoreAnchorPointForPosition = false;
            centerColor.IgnoreAnchorPointForPosition = false;
            rightColor.IgnoreAnchorPointForPosition = false;


            top.AnchorPoint = new CCPoint(0.5f, 1);
            left.AnchorPoint = new CCPoint(0, 0.5f);
            leftColor.AnchorPoint = new CCPoint(0, 0.5f);
            center.AnchorPoint = new CCPoint(0, 0.5f);
            centerColor.AnchorPoint = new CCPoint(0, 0.5f);
            right.AnchorPoint = new CCPoint(0, 0.5f);
            rightColor.AnchorPoint = new CCPoint(0, 0.5f);

            top.Position = new CCPoint(s.Width / 2, s.Height - 20);
            left.Position = new CCPoint(0, s.Height / 2);
            leftColor.Position = left.Position;
            center.Position = new CCPoint(blockSize.Width, s.Height / 2);
            centerColor.Position = center.Position;
            right.Position = new CCPoint(blockSize.Width * 2, s.Height / 2);
            rightColor.Position = right.Position;

            AddChild(leftColor, -1);
            AddChild(left, 0, kTagLabel1);
            AddChild(rightColor, -1);
            AddChild(right, 0, kTagLabel2);
            AddChild(centerColor, -1);
            AddChild(center, 0, kTagLabel3);
            AddChild(top, 0, kTagLabel4);
        }

        public void restartCallback(CCObject pSender)
        {
            showFont(FontTestScene.restartAction());
        }

        public void nextCallback(CCObject pSender)
        {
            showFont(FontTestScene.nextAction());
        }

        public void backCallback(CCObject pSender)
        {
            showFont(FontTestScene.backAction());
        }

        public virtual string title()
        {
            return "Font test";
        }
    }
}