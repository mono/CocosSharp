using System;
using MonoMac.AppKit;
using MonoMac;
using CocosSharp;

namespace ${SolutionName}
{
    public class AppDelegate : NSApplicationDelegate
    {
        public override void FinishedLaunching(MonoMac.Foundation.NSObject notification)
        {
            CCApplication application = new CCApplication();
            application.ApplicationDelegate = new GameAppDelegate();

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
                NSApplication.SharedApplication.Delegate = new AppDelegate();
                NSApplication.Main(args);
            }
        }
    }
}

