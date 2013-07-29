using Cocos2D;

namespace tests.Extensions
{
	public class ExtensionsMainLayer : CCLayer
	{
		public override void OnEnter()
		{
			base.OnEnter();

			var	s = CCDirector.SharedDirector.WinSize;

			var pMenu = new CCMenu();
			pMenu.Position = CCPoint.Zero;
			CCMenuItemFont.FontName = "arial";
			CCMenuItemFont.FontSize = 24;
			for (int i = 0; i < ExtensionsTestScene.TEST_MAX_COUNT; ++i)
			{
				var pItem = new CCMenuItemFont(ExtensionsTestScene.testsName[i], menuCallback);
				pItem.Position = new CCPoint(s.Width / 2, s.Height - (i + 1) * ExtensionsTestScene.LINE_SPACE);
				pMenu.AddChild(pItem, ExtensionsTestScene.kItemTagBasic + i);
			}

			AddChild(pMenu);

		}

		public void menuCallback(object pSender)
		{
			var pItem = (CCMenuItemFont)pSender;
			var nIndex = pItem.ZOrder - ExtensionsTestScene.kItemTagBasic;

			switch (nIndex)
			{
				//case TEST_NOTIFICATIONCENTER:
				//    runNotificationCenterTest();
				//    break;
				case ExtensionsTestScene.TEST_CCCONTROLBUTTON:
				    var pManager = CCControlSceneManager.sharedControlSceneManager();
				    CCDirector.SharedDirector.ReplaceScene(pManager.currentControlScene());
				    break;
				case ExtensionsTestScene.TEST_COCOSBUILDER:
					var pScene = new CocosBuilderTest();
					if (pScene != null)
						pScene.runThisTest();
					break;
				//case TEST_HTTPCLIENT:
				//    runHttpClientTest();
				//    break;
			//#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS) || (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
				//case TEST_EDITBOX:
				//    runEditBoxTest();
				//    break;
			//#endif
				case ExtensionsTestScene.TEST_TABLEVIEW:
					TableViewTestLayer.runTableViewTest();
					break;
                case ExtensionsTestScene.TEST_Scale9Sprite:
                    var Manager = Scale9SpriteSceneManager.sharedSprite9SceneManager();
				    CCDirector.SharedDirector.ReplaceScene(Manager.currentControlScene());
			        break;
				default:
					break;
			}
		}
	}
}