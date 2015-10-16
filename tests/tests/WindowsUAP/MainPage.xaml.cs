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
