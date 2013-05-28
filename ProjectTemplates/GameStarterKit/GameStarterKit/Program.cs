using System;
using System.Diagnostics;

#if ANDROID
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;
using Microsoft.Xna.Framework;
#endif
#if IPHONE || IOS
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif
#if MONOMAC
using MonoMac.AppKit;
using MonoMac;
#endif

namespace $safeprojectname$
{
#if IPHONE || IOS
	[Register ("AppDelegate")]
	class Program : UIApplicationDelegate 
	{
		private Game1 game;

		public override void FinishedLaunching (UIApplication app)
		{
			// Fun begins..
			game = new Game1();
			game.Run();
		}
		
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
#endif
	#if MONOMAC
	class Program : NSApplicationDelegate 
	{
		private Game1 game;
		
		public override void FinishedLaunching (MonoMac.Foundation.NSObject notification)
		{
			#if DEBUG
			/* Create a listener that outputs to the console screen, and 
  			* add it to the debug listeners. */
			TextWriterTraceListener debugConsoleWriter = new 
				TextWriterTraceListener(System.Console.Out);
			Debug.Listeners.Add(debugConsoleWriter);
			#endif
			// Fun begins..
			game = new Game1();
			game.Run();
		}
		
		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
		
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			NSApplication.Init ();
			
			using (var p = new MonoMac.Foundation.NSAutoreleasePool ()) {
				NSApplication.SharedApplication.Delegate = new Program();
				NSApplication.Main(args);
			}
			
		}
	}
	#endif
#if WINDOWS || XBOX || PSM
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif

#if ANDROID
    [Activity(
        Label = "$safeprojectname$",
               AlwaysRetainTaskState = true,
               Icon = "@drawable/ic_launcher",
               Theme = "@style/Theme.NoTitleBar",
               ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
               LaunchMode = Android.Content.PM.LaunchMode.SingleInstance,
        MainLauncher = true,
        ConfigurationChanges =  ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
#if OUYA
    [IntentFilter(new[] { Intent.ActionMain }, Categories = new[] { Intent.CategoryLauncher, "ouya.intent.category.GAME" })]
#endif
    public class Activity1 : AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
#if OUYA
            Ouya.Console.Api.OuyaFacade.Instance.Init(this, "XXXXXXXXXXXXXX"); // Our UUID dev ID
#endif

            Game1.Activity = this;
            var game = new Game1();

            var frameLayout = new FrameLayout(this);
            frameLayout.AddView(game.Window);
            this.SetContentView(frameLayout);

            //SetContentView(game.Window);
            game.Run(GameRunBehavior.Asynchronous);
        }
    }
#endif


}

