using CocosSharp;

namespace tests.FontTest
{
    public class FontTestScene : TestScene
    {
        private static int fontIdx;

        private static readonly string[] fontList =
            {
//#if IOS || MACOS
//                "American Typewriter",
//                "Marker Felt",
//				"Chalkboard",
//#endif
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
                CCVerticalTextAlignment.Top,
                CCVerticalTextAlignment.Center,
                CCVerticalTextAlignment.Bottom
            };

        public override void runThisTest()
        {
            CCLayer pLayer = new FontTest();
            AddChild(pLayer);

            Scene.Director.ReplaceScene(this);
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

    public class Background : CCDrawNode
    {

        public Background(CCColor4B color)
        {
            Color = new CCColor3B(color);
            Opacity = color.A;
            //AnchorPoint = CCPoint.AnchorLowerLeft;
        }

        public Background(CCSize size, CCColor4B color)
        {
            Color = new CCColor3B(color);
            Opacity = color.A;
            //AnchorPoint = CCPoint.AnchorMiddle;
            ContentSize = size;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            var fillColor = new CCColor4B(Color.R, Color.G, Color.B, Opacity);
            DrawRect(new CCRect(0,0,ContentSize.Width,ContentSize.Height), fillColor, 1f, CCColor4B.White);
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            if (ContentSize == CCSize.Zero)
                ContentSize = VisibleBoundsWorldspace.Size;
        }
    }

    public class FontTest : TestNavigationLayer
    {
        private const int kTagLabel1 = 1;
        private const int kTagLabel2 = 2;
        private const int kTagLabel3 = 3;
        private const int kTagLabel4 = 4;

        public FontTest()
        {

            //showFont(FontTestScene.restartAction());
        }

        public override void OnEnter()
        {
            base.OnEnter();

            var s = VisibleBoundsWorldspace.Size;

            var blockSize = new CCSize(s.Width / 3, s.Height / 2);
            //blockSize = new CCSize(50, 50);
            var leftColor = new Background(blockSize, new CCColor4B(100, 100, 100, 255));
            var centerColor = new Background(blockSize, new CCColor4B(200, 100, 100, 255));
            var rightColor = new Background(blockSize, new CCColor4B(100, 100, 200, 255));

            leftColor.IgnoreAnchorPointForPosition = false;
            centerColor.IgnoreAnchorPointForPosition = false;
            rightColor.IgnoreAnchorPointForPosition = false;

            leftColor.AnchorPoint = new CCPoint(0, 0.5f);
            centerColor.AnchorPoint = new CCPoint(0, 0.5f);
            rightColor.AnchorPoint = new CCPoint(0, 0.5f);

            leftColor.Position = new CCPoint(0, s.Height / 2);
            centerColor.Position = new CCPoint(blockSize.Width, s.Height / 2);
            rightColor.Position = new CCPoint(blockSize.Width * 2, s.Height / 2);

            AddChild(leftColor, -1);
            AddChild(rightColor, -1);
            AddChild(centerColor, -1);

            showFont(FontTestScene.restartAction());
        }

        public void showFont(string pFont)
        {
            CCSize s = Layer.VisibleBoundsWorldspace.Size;

            var blockSize = new CCSize(s.Width / 3, s.Height / 2);
            float fontSize = 26;

            RemoveChildByTag(kTagLabel1, true);
            RemoveChildByTag(kTagLabel2, true);
            RemoveChildByTag(kTagLabel3, true);
            RemoveChildByTag(kTagLabel4, true);

            CCLabelTtf top = new CCLabelTtf(pFont, "Arial", 24);
            CCLabelTtf left = new CCLabelTtf("alignment left", pFont, fontSize,
                                             blockSize, CCTextAlignment.Left,
                                             FontTestScene.verticalAlignment[FontTestScene.vAlignIdx]);
            CCLabelTtf center = new CCLabelTtf("alignment center", pFont, fontSize,
                                               blockSize, CCTextAlignment.Center,
                                               FontTestScene.verticalAlignment[FontTestScene.vAlignIdx]);
            CCLabelTtf right = new CCLabelTtf("alignment right", pFont, fontSize,
                                              blockSize, CCTextAlignment.Right,
                                              FontTestScene.verticalAlignment[FontTestScene.vAlignIdx]);

            top.AnchorPoint = new CCPoint(0.5f, 1);
            left.AnchorPoint = new CCPoint(0, 0.5f);
            center.AnchorPoint = new CCPoint(0, 0.5f);
            right.AnchorPoint = new CCPoint(0, 0.5f);

            top.Position = TitleLabel.Position - new CCPoint(0, 20);
            left.Position = new CCPoint(0, s.Height / 2);
            center.Position = new CCPoint(blockSize.Width, s.Height / 2);
            right.Position = new CCPoint(blockSize.Width * 2, s.Height / 2);

            AddChild(left, 0, kTagLabel1);
            AddChild(right, 0, kTagLabel2);
            AddChild(center, 0, kTagLabel3);
            AddChild(top, 0, kTagLabel4);
        }

        public override void RestartCallback(object sender)
        {
            base.RestartCallback(sender);
            showFont(FontTestScene.restartAction());
        }

        public override void NextCallback(object sender)
        {
            base.NextCallback(sender);
            showFont(FontTestScene.nextAction());
        }

        public override void BackCallback(object sender)
        {
            base.BackCallback(sender);
            showFont(FontTestScene.backAction());
        }

        public override string Title
        {
            get
            {
                return "Font test";
            }
        }
    }
}