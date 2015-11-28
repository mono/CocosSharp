using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using tests;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CocosSharp.Tests.WindowsUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            GameView.ViewCreated += LoadGame;

            AppDelegate.SharedWindow = GameView;

        }

        void LoadGame(object sender, System.EventArgs e)
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
                gameView.DesignResolution = new CCSizeI(1024, 768);
                gameView.Stats.Enabled = true;
                gameView.ResolutionPolicy = CCViewResolutionPolicy.ShowAll;
                CCScene gameScene = new CCScene(gameView);
                gameScene.AddLayer(new TestController());
                gameView.RunWithScene(gameScene);
            }
        }
    }
}
