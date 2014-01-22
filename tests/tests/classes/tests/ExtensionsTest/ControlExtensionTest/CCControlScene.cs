using System;
using CocosSharp;

namespace tests.Extensions
{
    public class CCControlScene : CCLayer
	{
        public CCControlScene()
		{  
			// Get the sceensize
			CCSize screensize = CCDirector.SharedDirector.WinSize;

			var pBackItem = new CCMenuItemFont("Back", toExtensionsMainLayer);
			pBackItem.Position = new CCPoint(screensize.Width - 50, 25);
			var pBackMenu = new CCMenu(pBackItem);
			pBackMenu.Position =  CCPoint.Zero;
			AddChild(pBackMenu, 10);

			// Add the generated background
			var background = new CCSprite("extensions/background");
			background.Position = new CCPoint(screensize.Width / 2, screensize.Height / 2);
			AddChild(background);
    
			// Add the ribbon
			var ribbon = new CCScale9SpriteFile("extensions/ribbon", new CCRect(1, 1, 48, 55));
			ribbon.ContentSize = new CCSize(screensize.Width, 57);
			ribbon.Position = new CCPoint(screensize.Width / 2.0f, screensize.Height - ribbon.ContentSize.Height / 2.0f);
			AddChild(ribbon);
    
			// Add the title
			setSceneTitleLabel(new CCLabelTTF(" ", "Arial", 12));
			m_pSceneTitleLabel.Position = new CCPoint(screensize.Width / 2, screensize.Height - m_pSceneTitleLabel.ContentSize.Height / 2 - 5);
			AddChild(m_pSceneTitleLabel, 1);

            // Add the subtitle
            setSceneSubtitleLabel(new CCLabelTTF(" ", "Arial", 12));
		    m_pSceneSubtitleLabel.Position = new CCPoint(screensize.Width / 2,
		                                                 screensize.Height - m_pSceneTitleLabel.ContentSize.Height -
		                                                 m_pSceneSubtitleLabel.ContentSize.Height / 2 - 10);
            AddChild(m_pSceneSubtitleLabel, 1);
    
			// Add the menu
			var item1 = new CCMenuItemImage("Images/b1", "Images/b2", previousCallback);
			var item2 = new CCMenuItemImage("Images/r1", "Images/r2", restartCallback);
			var item3 = new CCMenuItemImage("Images/f1", "Images/f2", nextCallback);
    
			var menu = new CCMenu(item1, item3, item2);
			menu.Position = CCPoint.Zero;
			item1.Position = new CCPoint(screensize.Width / 2 - 100, 37);
			item2.Position = new CCPoint(screensize.Width / 2, 35);
			item3.Position = new CCPoint(screensize.Width / 2 + 100, 37);
    
			AddChild(menu, 1);
		}

		// Menu Callbacks
		public void toExtensionsMainLayer(object sender)
		{
			var pScene = new ExtensionsTestScene();
			pScene.runThisTest();
		}

		public virtual void previousCallback(object sender)
		{
			CCDirector.SharedDirector.ReplaceScene(CCControlSceneManager.sharedControlSceneManager().previousControlScene());
		}

		public virtual void restartCallback(object sender)
		{
			CCDirector.SharedDirector.ReplaceScene(CCControlSceneManager.sharedControlSceneManager().currentControlScene());
		}
		
		public virtual void nextCallback(object sender)
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

        private CCLabelTTF m_pSceneSubtitleLabel;
        public virtual CCLabelTTF getSceneSubtitleLabel() { return m_pSceneSubtitleLabel; }
        public virtual void setSceneSubtitleLabel(CCLabelTTF var)
        {
            if (m_pSceneSubtitleLabel != var)
            {
                m_pSceneSubtitleLabel = var;
            }
        }


		/** Title label of the scene. */
		public static CCScene sceneWithTitle(string title)
		{
			var pScene = new CCScene();
		    var controlLayer = new CCControlScene();
		    if (controlLayer != null)
		    {
		        controlLayer.getSceneTitleLabel().Text = (title);
		        pScene.AddChild(controlLayer);
		    }
			return pScene;
		}
	}
}