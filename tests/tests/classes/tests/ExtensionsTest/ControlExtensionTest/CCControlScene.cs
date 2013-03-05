using System;
using cocos2d;

namespace tests.Extensions
{
    public class CCControlScene : CCLayer
	{
		public override bool Init()
		{
			if (base.Init())
			{    
				// Get the sceensize
				CCSize screensize = CCDirector.SharedDirector.WinSize;

				var pBackItem = CCMenuItemFont.Create("Back", toExtensionsMainLayer);
				pBackItem.Position = new CCPoint(screensize.Width - 50, 25);
				var pBackMenu = CCMenu.Create(pBackItem);
				pBackMenu.Position =  CCPoint.Zero;
				AddChild(pBackMenu, 10);

				// Add the generated background
				var background = CCSprite.Create("extensions/background");
				background.Position = new CCPoint(screensize.Width / 2, screensize.Height / 2);
				AddChild(background);
        
				// Add the ribbon
				var ribbon = CCScale9Sprite.Create("extensions/ribbon", new CCRect(1, 1, 48, 55));
				ribbon.ContentSize = new CCSize(screensize.Width, 57);
				ribbon.Position = new CCPoint(screensize.Width / 2.0f, screensize.Height - ribbon.ContentSize.Height / 2.0f);
				AddChild(ribbon);
        
				// Add the title
				setSceneTitleLabel(CCLabelTTF.Create("Title", "arial", 12));
				m_pSceneTitleLabel.Position = new CCPoint(screensize.Width / 2, screensize.Height - m_pSceneTitleLabel.ContentSize.Height / 2 - 5);
				AddChild(m_pSceneTitleLabel, 1);
        
				// Add the menu
				var item1 = CCMenuItemImage.Create("Images/b1", "Images/b2", previousCallback);
				var item2 = CCMenuItemImage.Create("Images/r1", "Images/r2", restartCallback);
				var item3 = CCMenuItemImage.Create("Images/f1", "Images/f2", nextCallback);
        
				var menu = CCMenu.Create(item1, item3, item2);
				menu.Position = CCPoint.Zero;
				item1.Position = new CCPoint(screensize.Width / 2 - 100, 37);
				item2.Position = new CCPoint(screensize.Width / 2, 35);
				item3.Position = new CCPoint(screensize.Width / 2 + 100, 37);
        
				AddChild(menu ,1);

				return true;
			}
			return false;

		}

		// Menu Callbacks
		public void toExtensionsMainLayer(CCObject sender)
		{
			var pScene = new ExtensionsTestScene();
			pScene.runThisTest();
		}

		public void previousCallback(CCObject sender)
		{
			CCDirector.SharedDirector.ReplaceScene(CCControlSceneManager.sharedControlSceneManager().previousControlScene());
		}

		public void restartCallback(CCObject sender)
		{
			CCDirector.SharedDirector.ReplaceScene(CCControlSceneManager.sharedControlSceneManager().currentControlScene());
		}
		
		public void nextCallback(CCObject sender)
		{
			CCDirector.SharedDirector.ReplaceScene(CCControlSceneManager.sharedControlSceneManager().nextControlScene());
		}

		private CCLabelTTF m_pSceneTitleLabel; 
		public virtual CCLabelTTF getSceneTitleLabel() { return m_pSceneTitleLabel; } 
		public virtual void setSceneTitleLabel(CCLabelTTF var)   
		{ 
			if (m_pSceneTitleLabel != var) 
			{ 
				m_pSceneTitleLabel = var; 
			} 
		}


		/** Title label of the scene. */
		public static CCScene sceneWithTitle(string title)
		{
			var pScene = CCScene.Create();
		    var controlLayer = new CCControlScene();
		    if (controlLayer != null && controlLayer.Init())
		    {
		        controlLayer.getSceneTitleLabel().SetString(title);
		        pScene.AddChild(controlLayer);
		    }
			return pScene;
		}
	}
}