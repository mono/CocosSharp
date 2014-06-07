using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CocosSharp;


namespace tests
{
    public class AppDelegate : CCApplicationDelegate
    {
        public override void ApplicationDidFinishLaunching(CCApplication application)
        {
            application.SupportedOrientations = CCDisplayOrientation.LandscapeRight | CCDisplayOrientation.LandscapeLeft;
            application.AllowUserResizing = true;
            application.PreferMultiSampling = false;
            application.ContentRootDirectory = "Content";

            #if WINDOWS || WINDOWSGL || WINDOWSDX 
            application.PreferredBackBufferWidth = 1024;
            application.PreferredBackBufferHeight = 768;
            #elif MACOS
            application.PreferredBackBufferWidth = 960;
            application.PreferredBackBufferHeight = 640;
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

			CCDirector director = CCDirector.SharedDirector;
            director.DisplayStats = true;
            director.AnimationInterval = 1.0 / 60;

            CCSize designSize = new CCSize(480, 320);

            if (CCDrawManager.FrameSize.Height > 320)
            {
				CCSize resourceSize = new CCSize(960, 640);
                application.ContentSearchPaths.Add("hd");
				director.ContentScaleFactor = resourceSize.Height / designSize.Height;
            }

            CCDrawManager.SetDesignResolutionSize(designSize.Width, designSize.Height, CCResolutionPolicy.ShowAll);

            CCScene scene = new CCScene();
            CCLayer layer = new TestController();

            scene.AddChild(layer);
            director.RunWithScene(scene);
        }

        public override void ApplicationDidEnterBackground(CCApplication application)
        {
            CCDirector.SharedDirector.Pause();
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
            CCDirector.SharedDirector.Resume();
        }
    }
}