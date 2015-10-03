using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using CocosSharp;

namespace tests
{
    partial class GameViewController : UIViewController
    {
        CCGameView gameView;

        public GameViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);

            gameView = View as CCGameView;
            gameView.ViewCreated += LoadGame;

            AppDelegate.SharedWindow = gameView;
        }

        void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null) 
            {
                gameView.DesignResolution = new CCSizeI (1024, 768);
                gameView.Stats.Enabled = true;
                CCScene gameScene = new CCScene (gameView);
                gameScene.AddLayer(new TestController());
                gameView.RunWithScene (gameScene);
            }
        }
    }
}
