using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
using Windows.ApplicationModel.Activation;
using CocosSharp;

namespace $safeprojectname$
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        public GamePage(LaunchActivatedEventArgs args)
        {
            this.InitializeComponent();

            // Create the game.
            CCApplication.Create(new AppDelegate(), args, Window.Current.CoreWindow, this);
        }
    }
}
