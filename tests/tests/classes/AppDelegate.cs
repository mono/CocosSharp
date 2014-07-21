using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CocosSharp;


namespace tests
{
    public class AppDelegate : CCApplicationDelegate
    {
        static CCDirector sharedDirector;
        static CCWindow sharedWindow;
        static CCViewport sharedViewport;
        static CCCamera sharedCamera;

        public static CCDirector SharedDirector
        {
            get { return sharedDirector; }
        }

        public static CCWindow SharedWindow
        {
            get { return sharedWindow; }
        }

        public static CCViewport SharedViewport
        {
            get { return sharedViewport; }
        }

        public static CCCamera SharedCamera
        {
            get { return sharedCamera; }
        }

        public override void ApplicationDidFinishLaunching(CCApplication application)
        {
            //application.SupportedOrientations = CCDisplayOrientation.LandscapeRight | CCDisplayOrientation.LandscapeLeft;
            //application.AllowUserResizing = true;
            //application.PreferMultiSampling = false;
            application.ContentRootDirectory = "Content";


            CCRect boundsRect = new CCRect(0.0f, 0.0f, 960, 640);

            sharedViewport = new CCViewport(new CCRect (0.0f, 0.0f, 1.0f, 1.0f));
            sharedWindow = application.MainWindow;
            sharedCamera = new CCCamera(boundsRect.Size, new CCPoint3(boundsRect.Center, 100.0f), new CCPoint3(boundsRect.Center, 0.0f));

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

            sharedDirector = new CCDirector();
            //director.DisplayStats = true;
            //director.AnimationInterval = 1.0 / 60;


//            if (sharedWindow.WindowSizeInPixels.Height > 320)
//            {
//                application.ContentSearchPaths.Insert(0,"HD");
//            }

            sharedWindow.AddSceneDirector(sharedDirector);

            CCScene scene = new CCScene(sharedWindow, sharedViewport, sharedDirector);
            CCLayer layer = new TestController();
            layer.Camera = sharedCamera;

            scene.AddChild(layer);
            sharedDirector.RunWithScene(scene);
        }

        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            application.PauseGame();
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            application.ResumeGame();
        }
    }
}