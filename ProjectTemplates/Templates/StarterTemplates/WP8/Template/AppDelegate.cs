using System.Reflection;
using Microsoft.Xna.Framework;
using CocosSharp;
using CocosDenshion;

namespace $safeprojectname$
{
    public class AppDelegate : CCApplicationDelegate
    {
        static CCWindow sharedWindow;

        public static CCWindow SharedWindow
        {
            get { return sharedWindow; }
        }

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            application.ContentRootDirectory = "Content";

            sharedWindow = mainWindow;

            application.HandleMediaStateAutomatically = false;

	    var introScene = IntroLayer.Scene(mainWindow);
            sharedWindow.RunWithScene(introScene);
        }

        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            application.Paused = true;
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            application.Paused = false;
        }
    }
}