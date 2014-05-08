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
#if MACOS
using MonoMac.AppKit;
using MonoMac;
#endif
#if IPHONE || IOS
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif
using CocosSharp;
using Microsoft.Xna.Framework.Content;

namespace tests
{
#if IPHONE || IOS
	[Register ("AppDelegate")]
	class Program : UIApplicationDelegate 
	{
		private Game1 game;

		public override void FinishedLaunching (UIApplication app)
		{
            // More shameless hacking to bypass AOT
            //var hHack = new ReflectiveReader<CCBMFontConfiguration>();
            //var hFoo = new PlistDocument.PlistDocumentReader ();

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
	#if MACOS
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
	#if WINDOWS || WINDOWSGL || XBOX || PSM

#if !NETFX_CORE
    static class Program
    {
#if WINDOWS || WINDOWSGL
        private static Game1 game;
#endif
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
#if WINDOWS || WINDOWSGL
        [STAThread]
#endif
        static void Main(string[] args)
        {
#if WINDOWS || WINDOWSGL
            game = new Game1();
            game.Run();
#else
            using (Game1 game = new Game1())
            {
                game.Run();
            }
#endif
        }
    }
#endif
#endif

#if ANDROID
    [Activity(
        Label = "Tests",
        AlwaysRetainTaskState = true,
        Icon = "@drawable/Icon",
        Theme = "@style/Theme.NoTitleBar",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
        LaunchMode = Android.Content.PM.LaunchMode.SingleInstance,
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
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
            Ouya.Console.Api.OuyaFacade.Instance.Init(this, "f3366755-190b-4b95-af21-ca4a01a99478"); // Our UUID dev ID
#endif
            var game = new Game1();
			this.SetContentView((View)game.Services.GetService(typeof(View)));
            game.Run(GameRunBehavior.Asynchronous);
        }
    }
#endif
#if NETFX_CORE 
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main() {
            var factory = new MonoGame.Framework.GameFrameworkViewSource<Game1>();
            Windows.ApplicationModel.Core.CoreApplication.Run(factory);
        }
    }
#endif
}

