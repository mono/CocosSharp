
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.OpenGL;
using CGRect = System.Drawing.RectangleF;

using CocosSharp;

namespace tests
{
    public partial class MacGameController : MonoMac.AppKit.NSWindowController
    {

        #region Constructors

        // Called when created from unmanaged code
        public MacGameController(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }
		
        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MacGameController(NSCoder coder)
            : base(coder)
        {
            Initialize();
        }
		
        // Call to load from the XIB/NIB file
        public MacGameController()
            : base("MacGameWindow")
        {
            Initialize();
        }
		
        public override void AwakeFromNib()
        {

            base.AwakeFromNib();

            gameView.ViewCreated += LoadGame;
            AppDelegate.SharedWindow = gameView;

        }
        // Shared initialization code
        void Initialize()
        {
            
        }

        #endregion

        void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null) 
            {
                var contentSearchPaths = new List<string>() { "fonts", "sounds" };
                gameView.DesignResolution = new CCSizeI (1024, 768);
                gameView.Stats.Enabled = true;

                gameView.ContentManager.SearchPaths = contentSearchPaths;

                CCScene gameScene = new CCScene (gameView);

                gameScene.AddLayer(new TestController());
                gameView.RunWithScene (gameScene);
            }
        }

        //strongly typed window accessor
        public new MacGameWindow Window
        {
            get
            {
                return (MacGameWindow)base.Window;
            }
        }

    }
}

