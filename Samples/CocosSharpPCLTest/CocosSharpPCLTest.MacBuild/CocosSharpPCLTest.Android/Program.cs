using System;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;
using Microsoft.Xna.Framework;
using CocosSharp;
using Microsoft.Xna.Framework.Content;

namespace CocosSharpPCLTest
{
	[Activity(
	Label = "PCLTest",
	AlwaysRetainTaskState = true,
	Icon = "@drawable/Icon",
	ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
	LaunchMode = Android.Content.PM.LaunchMode.SingleInstance,
	MainLauncher = true,
	ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
	]

	public class Activity1 : AndroidGameActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			Android.Util.Log.Info ("CocosSharp", "OnCreate");

			Game1.Activity = this;
			var game = new Game1();

			var frameLayout = new FrameLayout(this);
			frameLayout.AddView(game.Window);
			this.SetContentView(frameLayout);

			game.Run(GameRunBehavior.Asynchronous);
		}
	}
}

