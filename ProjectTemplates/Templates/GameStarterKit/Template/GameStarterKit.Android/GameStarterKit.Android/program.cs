using System;
using System.Diagnostics;

using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;
using Microsoft.Xna.Framework;

namespace GameStarterKit
{

    [Activity(
        Label = "$safeprojectname$",
               AlwaysRetainTaskState = true,
               Icon = "@drawable/ic_launcher",
               Theme = "@style/Theme.NoTitleBar",
		// This is where you set the orientations supported by your game.
               ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
               LaunchMode = Android.Content.PM.LaunchMode.SingleInstance,
        MainLauncher = true,
		// If your game has orientation changes, then you need to enable them here
        ConfigurationChanges =  ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
    public class Activity1 : AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Game1.Activity = this;
            var game = new Game1();

            var frameLayout = new FrameLayout(this);
            frameLayout.AddView(game.Window);
            this.SetContentView(frameLayout);

            //SetContentView(game.Window);
            game.Run(GameRunBehavior.Asynchronous);
        }
		/*
		Implement other lifecycle methods for your Activity. Your game will launch through this
		activity class.
		*/
    }
}

