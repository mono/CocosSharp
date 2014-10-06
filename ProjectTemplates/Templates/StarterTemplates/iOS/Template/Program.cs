using System;
using System.Diagnostics;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using CocosSharp;

namespace $safeprojectname$
{
    [Register("AppDelegate")]
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
            UIApplication.Main(args, null, "AppDelegate");
        }
    }

}



