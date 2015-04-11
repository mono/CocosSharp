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
using Foundation;
using UIKit;
#endif
using CocosSharp;
using Microsoft.Xna.Framework.Content;

namespace tests
{
#if IPHONE || IOS
    [Register ("AppDelegate")]
    class Program : UIApplicationDelegate 
    {
        public override void FinishedLaunching(UIApplication app)
        {
            CCApplication application = new CCApplication();
            application.ApplicationDelegate = new AppDelegate();

            application.StartGame();
        }

        // This is the main entry point of the application.
        static void Main(string[] args)
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
        public override void FinishedLaunching(MonoMac.Foundation.NSObject notification)
        {
            CCApplication application = new CCApplication(false, new CCSize(1024f, 768f));
            application.ApplicationDelegate = new AppDelegate();

            application.StartGame();
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }

        // This is the main entry point of the application.
        static void Main (string[] args)
        {
            NSApplication.Init ();

            using (var p = new MonoMac.Foundation.NSAutoreleasePool ()) 
            {
                NSApplication.SharedApplication.Delegate = new Program();
                NSApplication.Main(args);
            }
        }
    }
#endif

#if WINDOWS || WINDOWSGL

#if !NETFX_CORE
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			CCApplication application = new CCApplication(false, new CCSize(1024f, 768f));
			application.ApplicationDelegate = new AppDelegate();

			application.StartGame();
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
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
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
			CCApplication application = new CCApplication();
			application.ApplicationDelegate = new AppDelegate();

			this.SetContentView(application.AndroidContentView);

			application.StartGame();

        }
    }
#endif

#if NETFX_CORE && !WINDOWS_PHONE81
    public static class Program 
    {
        static void Main() 
        {
            CCApplication.Create(new AppDelegate());
        }
    }
#endif
}

