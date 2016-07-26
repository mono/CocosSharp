using Foundation;
using System;
using System.Collections.Generic;
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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad ();

            gameView = View as CCGameView;
            gameView.ViewCreated += LoadGame;

            AppDelegate.SharedWindow = gameView;
        }

        public override void ViewWillDisappear (bool animated)
        {
            base.ViewWillDisappear (animated);

            if (gameView != null)
                gameView.Paused = true;
        }

        public override void ViewDidAppear (bool animated)
        {
            base.ViewDidAppear (animated);

            if (gameView != null)
                gameView.Paused = false;
        }

        void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null) 
            {
                CCSpriteFontCache sharedCache = gameView.SpriteFontCache;
                sharedCache.RegisterFont("arial", 12, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 38, 50, 64);
                sharedCache.RegisterFont("MarkerFelt", 16, 18, 22, 32);
                sharedCache.RegisterFont("MarkerFelt-Thin", 12, 18);
                sharedCache.RegisterFont("Paint Boy", 26);
                sharedCache.RegisterFont("Schwarzwald Regular", 26);
                sharedCache.RegisterFont("Scissor Cuts", 26);
                sharedCache.RegisterFont("A Damn Mess", 26);
                sharedCache.RegisterFont("Abberancy", 26);
                sharedCache.RegisterFont("Abduction", 26);

                gameView.ContentManager.SearchPaths = new List<string>() { "", "images", "fonts" };

                gameView.DesignResolution = new CCSizeI (1024, 768);
                gameView.Stats.Enabled = true;
                CCScene gameScene = new CCScene (gameView);
                gameScene.AddLayer(new TestController());
                gameView.RunWithScene (gameScene);
            }
        }
    }
}
