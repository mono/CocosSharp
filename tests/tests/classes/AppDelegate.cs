using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CocosSharp;


namespace tests
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
            //application.SupportedOrientations = CCDisplayOrientation.LandscapeRight | CCDisplayOrientation.LandscapeLeft;
            //application.AllowUserResizing = true;
            application.PreferMultiSampling = false;
            application.ContentRootDirectory = "Content";

            sharedWindow = mainWindow;

            mainWindow.SetDesignResolutionSize(960, 640, CCSceneResolutionPolicy.ShowAll);

            #if WINDOWS || WINDOWSGL || WINDOWSDX 
			//application.PreferredBackBufferWidth = 1024;
			//application.PreferredBackBufferHeight = 768;
            #elif MACOS
            //application.PreferredBackBufferWidth = 960;
            //application.PreferredBackBufferHeight = 640;
            #endif

            #if WINDOWS_PHONE8
            application.HandleMediaStateAutomatically = false; // Bug in MonoGame - https://github.com/Cocos2DXNA/cocos2d-xna/issues/325
            #endif

            CCSpriteFontCache.FontScale = 0.6f;
            CCSpriteFontCache.RegisterFont("arial", 12, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 38, 50, 64);
            CCSpriteFontCache.RegisterFont("MarkerFelt", 16, 18, 22, 32);
            CCSpriteFontCache.RegisterFont("MarkerFelt-Thin", 12, 18);
            CCSpriteFontCache.RegisterFont("Paint Boy", 26);
            CCSpriteFontCache.RegisterFont("Schwarzwald Regular", 26);
            CCSpriteFontCache.RegisterFont("Scissor Cuts", 26);
            CCSpriteFontCache.RegisterFont("A Damn Mess", 26);
            CCSpriteFontCache.RegisterFont("Abberancy", 26);
            CCSpriteFontCache.RegisterFont("Abduction", 26);

            mainWindow.DisplayStats = true;

            
//            if (mainWindow.WindowSizeInPixels.Height > 320)
//            {
//                application.ContentSearchPaths.Insert(0,"HD");
//            }

            CCScene scene = new CCScene(sharedWindow);
            CCLayer layer = new TestController();

            scene.AddChild(layer);
            sharedWindow.RunWithScene(scene);
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