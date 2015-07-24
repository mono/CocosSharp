using System;
using CocosSharp;

namespace tests.Extensions
{
    public class CCControlScene : CCLayer
	{

        public virtual CCLabel SceneTitleLabel { get; set; }
        public virtual CCLabel SceneSubtitleLabel { get; set; }

        public CCControlScene()
		{  
            
            SceneTitleLabel = new CCLabel(" ", "Arial", 12, CCLabelFormat.SpriteFont);
            AddChild(SceneTitleLabel, 1);

		}

        public override void OnEnter()
        {
            base.OnEnter();

            // Get the screensize
            CCSize screensize = Layer.VisibleBoundsWorldspace.Size;

            var pBackItem = new CCMenuItemFont("Back", toExtensionsMainLayer);
            pBackItem.Position = new CCPoint(screensize.Width - 50, 25);
            var pBackMenu = new CCMenu(pBackItem);
            pBackMenu.Position =  CCPoint.Zero;
            AddChild(pBackMenu, 10);

            // Add the generated background
            var background = new CCSprite("extensions/background");
            background.Position = screensize.Center;
            AddChild(background);

            // Add the ribbon
            //var ribbon = new CCScale9SpriteFile("extensions/ribbon", new CCRect(1, 1, 48, 55));
            //ribbon.ContentSize = new CCSize(screensize.Width, 57);
            //ribbon.Position = new CCPoint(screensize.Width / 2.0f, screensize.Height - ribbon.ContentSize.Height / 2.0f);
            //AddChild(ribbon);

            // Add the title
            SceneTitleLabel.Position = new CCPoint(screensize.Width / 2, screensize.Height - SceneTitleLabel.ContentSize.Height / 2 - 5);

            // Add the subtitle
            SceneSubtitleLabel = new CCLabel(" ", "Arial", 12, CCLabelFormat.SpriteFont);
            SceneSubtitleLabel.Position = new CCPoint(screensize.Width / 2,
                screensize.Height - SceneTitleLabel.ContentSize.Height -
            SceneSubtitleLabel.ContentSize.Height / 2 - 10);
            AddChild(SceneSubtitleLabel, 1);

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
			Scene.Director.ReplaceScene(CCControlSceneManager.sharedControlSceneManager().previousControlScene());
		}

		public virtual void restartCallback(object sender)
		{
			Scene.Director.ReplaceScene(CCControlSceneManager.sharedControlSceneManager().currentControlScene());
		}
		
		public virtual void nextCallback(object sender)
		{
			Scene.Director.ReplaceScene(CCControlSceneManager.sharedControlSceneManager().nextControlScene());
		}

//        private CCLabel sceneSubtitleLabel;
//        public virtual CCLabel getSceneSubtitleLabel() { return sceneSubtitleLabel; }
//
//
//        public virtual void setSceneSubtitleLabel(CCLabel var)
//        {
//            if (sceneSubtitleLabel != var)
//            {
//                sceneSubtitleLabel = var;
//            }
//        }


		/** Title label of the scene. */
        public static CCScene sceneWithTitle(string title, CCScene scene)
		{
			var pScene = new CCScene(scene);
		    var controlLayer = new CCControlScene();
		    if (controlLayer != null)
		    {
		        controlLayer.SceneTitleLabel.Text = (title);
		        pScene.AddChild(controlLayer);
		    }
			return pScene;
		}
	}
}