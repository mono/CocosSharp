using System;
using cocos2d;

namespace tests.Extensions
{

	public class ExtensionsTestScene : TestScene
	{
		public override void runThisTest()
		{
			CCLayer pLayer = new ExtensionsMainLayer();
			AddChild(pLayer);

			CCDirector.SharedDirector.ReplaceScene(this);
		}

        protected override void NextTestCase()
        {
        }
        protected override void PreviousTestCase()
        {
        }
        protected override void RestTestCase()
        {
        }

		/*
		TEST_NOTIFICATIONCENTER = 0,
		TEST_HTTPCLIENT,
		#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS) || (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
			TEST_EDITBOX,
		#endif
		*/

		public const int TEST_CCCONTROLBUTTON = 0;
		public const int TEST_COCOSBUILDER = 1;
		public const int TEST_TABLEVIEW = 2;
		public static int TEST_MAX_COUNT = 3;
		
		public static int LINE_SPACE = 40;
		public static int kItemTagBasic = 1000;

        public static string[] testsName = new string[] 
		{ 
			//"NotificationCenterTest",
			"CCControlButtonTest",
			"CocosBuilderTest",
			//"HttpClientTest",
			//#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS) || (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
			//"EditBoxTest",
			//#endif
			"TableViewTest"
		};

	}
}