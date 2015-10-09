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
    internal class Program : UIApplicationDelegate 
    {
        public override UIWindow Window {
            get;
            set;
        }


        public static UIStoryboard Storyboard = UIStoryboard.FromName ("TestsStoryboard", null);
        public static UIViewController initialViewController;


        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow (UIScreen.MainScreen.Bounds);

            initialViewController = Storyboard.InstantiateInitialViewController () as UIViewController;

            Window.RootViewController = initialViewController;
            Window.MakeKeyAndVisible ();

            return true;
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
        public override void DidFinishLaunching (MonoMac.Foundation.NSNotification notification)
        {

//            CCApplication application = new CCApplication(false, new CCSize(1024f, 768f));
//            application.ApplicationDelegate = new AppDelegate();
//
//            application.StartGame();
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
    public class Activity1 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var gameView = (CCGameView)FindViewById(Resource.Id.MyGameView);
            gameView.ViewCreated += LoadGame;

            AppDelegate.SharedWindow = gameView;

        }

        void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null) 
            {
                gameView.DesignResolution = new CCSizeI (1024, 768);
                gameView.Stats.Enabled = true;
                CCScene gameScene = new CCScene (gameView);
                gameScene.AddLayer(new TestController());
                gameView.RunWithScene (gameScene);
            }
        }
    }
#endif

#if NETFX_CORE && !WINDOWS_PHONE81
    public static class Program 
    {
        static void Main() 
        {
            //CCApplication.Create(new AppDelegate());
        }

    }
#endif
}

