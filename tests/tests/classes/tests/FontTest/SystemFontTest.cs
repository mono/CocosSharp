using CocosSharp;

namespace tests.FontTest
{
	public class SystemFontTestScene : TestScene
	{
		private static int fontIdx;

		private static readonly string[] fontList =
		{
#if IOS || IPHONE || MACOS
			"Chalkduster",
			"Noteworthy",
			"Marker Felt",
			"Papyrus",
			"American Typewriter",
			"Arial",
			"fonts/A Damn Mess.ttf",
			"fonts/Abberancy.ttf",
			"fonts/Abduction.ttf",
			"fonts/American Typewriter.ttf",
			"fonts/Courier New.ttf",
			"fonts/Marker Felt.ttf",
			"fonts/Paint Boy.ttf",
			"fonts/Schwarzwald Regular.ttf",
			"fonts/Scissor Cuts.ttf",
			"fonts/tahoma.ttf",
			"fonts/Thonburi.ttf",
			"fonts/ThonburiBold.ttf"

#endif
#if WINDOWS || WINDOWSGL || NETFX_CORE

            // .ttf fonts are loaded from embedded resources
            // look in Assets/Content/fonts

            "A Damn Mess",  // A Damn Mess.ttf
            "Abberancy",    // Abberancy.ttf
            "Abduction",    //Abduction.ttf
            "AmerType Md BT",  // American Typewriter.ttf
            "Scissor Cuts", //Scissor Cuts.ttf
            "Felt",         //Marker Felt.ttf
            "arial",
            "Courier New",
            "Marker Felt",
			"Comic Sans MS",
			"Felt",
			"MoolBoran",
			"Courier New",
			"Georgia",
			"Symbol",
            "Wingdings",
			"Arial",

            //"fonts/A Damn Mess.ttf",
            //"fonts/Abberancy.ttf",
            //"fonts/Abduction.ttf",
            //"fonts/American Typewriter.ttf",
            //"fonts/arial.ttf",
            //"fonts/Courier New.ttf",
            //"fonts/Marker Felt.ttf",
            //"fonts/Paint Boy.ttf",
            //"fonts/Schwarzwald Regular.ttf",
            //"fonts/Scissor Cuts.ttf",
            //"fonts/tahoma.ttf",
            //"fonts/Thonburi.ttf",
            //"fonts/ThonburiBold.ttf"
#endif
#if ANDROID
			"Arial",
			"Courier New",
			"Georgia",

            "fonts/A Damn Mess.ttf",
            "fonts/Abberancy.ttf",
            "fonts/Abduction.ttf",
            "fonts/American Typewriter.ttf",
            "fonts/arial.ttf",
            "fonts/Courier New.ttf",
            "fonts/Marker Felt.ttf",
            "fonts/Paint Boy.ttf",
            "fonts/Schwarzwald Regular.ttf",
            "fonts/Scissor Cuts.ttf",
            "fonts/tahoma.ttf",
            "fonts/Thonburi.ttf",
            "fonts/ThonburiBold.ttf"
#endif
		};

		public static int verticalAlignIdx = 0;

		public static CCVerticalTextAlignment[] verticalAlignment =
		{
			CCVerticalTextAlignment.Top,
			CCVerticalTextAlignment.Center,
			CCVerticalTextAlignment.Bottom
		};

		public override void runThisTest()
		{
			CCLayer pLayer = new SystemFontTest();
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
				verticalAlignIdx = (verticalAlignIdx + 1) % verticalAlignment.Length;
			}
			return fontList[fontIdx];
		}

		public static string backAction()
		{
			fontIdx--;
			if (fontIdx < 0)
			{
				fontIdx = fontList.Length - 1;
				verticalAlignIdx--;
				if (verticalAlignIdx < 0)
					verticalAlignIdx = verticalAlignment.Length - 1;
			}

			return fontList[fontIdx];
		}

		public static string restartAction()
		{
			return fontList[fontIdx];
		}
	}



	public class SystemFontTest : TestNavigationLayer
	{
		private const int kTagLabel1 = 1;
		private const int kTagLabel2 = 2;
		private const int kTagLabel3 = 3;
		private const int kTagLabel4 = 4;

		private CCSize blockSize;
        private CCSize visibleRect;
		private float fontSize = 36;


		public SystemFontTest()
		{
		}


        public override void OnEnter()
        {
            base.OnEnter();

            var leftColor = new Background(blockSize, new CCColor4B(100, 100, 100, 255));
            var centerColor = new Background(blockSize, new CCColor4B(200, 100, 100, 255));
            var rightColor = new Background(blockSize, new CCColor4B(100, 100, 200, 255));

            leftColor.IgnoreAnchorPointForPosition = false;
            centerColor.IgnoreAnchorPointForPosition = false;
            rightColor.IgnoreAnchorPointForPosition = false;

            leftColor.AnchorPoint = new CCPoint(0, 0.5f);
            centerColor.AnchorPoint = new CCPoint(0, 0.5f);
            rightColor.AnchorPoint = new CCPoint(0, 0.5f);

            leftColor.Position = new CCPoint(0, visibleRect.Height / 2);
            centerColor.Position = new CCPoint(blockSize.Width, visibleRect.Height / 2);
            rightColor.Position = new CCPoint(blockSize.Width * 2, visibleRect.Height / 2);

            AddChild(leftColor, -1);
            AddChild(rightColor, -1);
            AddChild(centerColor, -1);

            showFont(SystemFontTestScene.restartAction());
        }


        protected override void AddedToScene()
        {
            base.AddedToScene();

            visibleRect = VisibleBoundsWorldspace.Size;
            blockSize = new CCSize(visibleRect.Width / 3, visibleRect.Height / 2);
        }


		public void showFont(string pFont)
		{

			RemoveChildByTag(kTagLabel1, true);
			RemoveChildByTag(kTagLabel2, true);
			RemoveChildByTag(kTagLabel3, true);
			RemoveChildByTag(kTagLabel4, true);

            var top = new CCLabel(pFont,"Helvetica", 32, CCLabelFormat.SystemFont);

            var center = new CCLabel("alignment center", pFont, fontSize,
                blockSize, new CCLabelFormat( CCLabelFormatFlags.SystemFont ) { Alignment = CCTextAlignment.Center,
                LineAlignment = SystemFontTestScene.verticalAlignment[SystemFontTestScene.verticalAlignIdx]}
                );

            var left = new CCLabel("alignment left", pFont, fontSize,
                blockSize, new CCLabelFormat( CCLabelFormatFlags.SystemFont ) { Alignment = CCTextAlignment.Left,
                LineAlignment = SystemFontTestScene.verticalAlignment[SystemFontTestScene.verticalAlignIdx]});

			var right = new CCLabel("alignment right", pFont, fontSize,
                blockSize, new CCLabelFormat( CCLabelFormatFlags.SystemFont ) { Alignment = CCTextAlignment.Right,
                LineAlignment = SystemFontTestScene.verticalAlignment[SystemFontTestScene.verticalAlignIdx]});

            top.AnchorPoint = CCPoint.AnchorMiddleTop;
            left.AnchorPoint = CCPoint.AnchorMiddleLeft;
            center.AnchorPoint = CCPoint.AnchorMiddleLeft;
            right.AnchorPoint = CCPoint.AnchorMiddleLeft;

            top.Position = TitleLabel.Position - new CCPoint(0, 20);
            left.Position = new CCPoint(0, visibleRect.Height / 2);
            center.Position = new CCPoint(blockSize.Width, visibleRect.Height / 2);
            right.Position = new CCPoint(blockSize.Width * 2, visibleRect.Height / 2);

            AddChild(left, 0, kTagLabel1);
            AddChild(right, 0, kTagLabel2);
            AddChild(center, 0, kTagLabel3);
            AddChild(top, 0, kTagLabel4);
		}

        public override void RestartCallback(object sender)
        {
            base.RestartCallback(sender);
			showFont(SystemFontTestScene.restartAction());
		}

        public override void NextCallback(object sender)
        {
            base.NextCallback(sender);
			showFont(SystemFontTestScene.nextAction());
		}

        public override void BackCallback(object sender)
        {
            base.BackCallback(sender);
			showFont(SystemFontTestScene.backAction());
		}

        public override string Title
        {
            get
            {
                return "System Font test";
            }
        }
	}
}