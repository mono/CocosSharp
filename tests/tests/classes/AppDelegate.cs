using System.Reflection;
using Microsoft.Xna.Framework;
using Cocos2D;


namespace tests
{
    public class AppDelegate : CCApplication
    {
        public AppDelegate(Game game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
            s_pSharedApplication = this;
            CCDrawManager.InitializeDisplay(game, graphics, DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft);


            graphics.PreferMultiSampling = false;

            //graphics.PreferredBackBufferWidth = 480;
            //graphics.PreferredBackBufferHeight = 320;
        }

        /// <summary>
        /// Implement for initialize OpenGL instance, set source path, etc...
        /// </summary>
        public override bool InitInstance()
        {
            return base.InitInstance();
        }

        /// <summary>
        ///  Implement CCDirector and CCScene init code here.
        /// </summary>
        /// <returns>
        ///  true  Initialize success, app continue.
        ///  false Initialize failed, app terminate.
        /// </returns>
        public override bool ApplicationDidFinishLaunching()
        {
            //initialize director
            CCDirector pDirector = CCDirector.SharedDirector;
            pDirector.SetOpenGlView();

            CCSpriteFontCache.RegisterFont("arial", 12, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 38, 50, 64);
            CCSpriteFontCache.RegisterFont("MarkerFelt", 16, 18, 22);
            CCSpriteFontCache.RegisterFont("MarkerFelt-Thin", 12, 18);
            CCSpriteFontCache.RegisterFont("Paint Boy", 26);
            CCSpriteFontCache.RegisterFont("Schwarzwald Regular", 26);
            CCSpriteFontCache.RegisterFont("Scissor Cuts", 26);
            CCSpriteFontCache.RegisterFont("A Damn Mess", 26);
            CCSpriteFontCache.RegisterFont("Abberancy", 26);
            CCSpriteFontCache.RegisterFont("Abduction", 26);

            // turn on display FPS
            pDirector.DisplayStats = true;
            // set FPS. the default value is 1.0/60 if you don't call this
            pDirector.AnimationInterval = 1.0 / 60;

            CCSize designSize = new CCSize(480, 320);

            if (CCDrawManager.FrameSize.Height > 320)
            {
                CCSize resourceSize = new CCSize(960, 640);
                CCContentManager.SharedContentManager.SearchPaths.Add("hd");
                pDirector.ContentScaleFactor = resourceSize.Height / designSize.Height;
            }

            CCDrawManager.SetDesignResolutionSize(designSize.Width, designSize.Height, CCResolutionPolicy.ShowAll);

/*
#if WINDOWS || WINDOWSGL
            CCDrawManager.SetDesignResolutionSize(1280, 768, CCResolutionPolicy.ExactFit);
#else
            CCDrawManager.SetDesignResolutionSize(800, 480, CCResolutionPolicy.ShowAll);
            //CCDrawManager.SetDesignResolutionSize(480, 320, CCResolutionPolicy.ShowAll);
#endif
*/

            // create a scene. it's an autorelease object
            CCScene pScene = new CCScene();
            CCLayer pLayer = new TestController();
            
            /*           
            CCScene pScene = CCScene.node();
            var pLayer = Box2DView.viewWithEntryID(0);
            pLayer.scale = 10;
            pLayer.anchorPoint = new CCPoint(0, 0);
            pLayer.position = new CCPoint(CCDirector.sharedDirector().getWinSize().width / 2, CCDirector.sharedDirector().getWinSize().height / 4);
            */

            pScene.AddChild(pLayer);
            pDirector.RunWithScene(pScene);

            return true;
        }

        /// <summary>
        /// The function be called when the application enter background
        /// </summary>
        public override void ApplicationDidEnterBackground()
        {
            CCDirector.SharedDirector.Pause();

            // if you use SimpleAudioEngine, it must be pause
            // SimpleAudioEngine::sharedEngine()->pauseBackgroundMusic();
        }

        /// <summary>
        /// The function be called when the application enter foreground  
        /// </summary>
        public override void ApplicationWillEnterForeground()
        {
            CCDirector.SharedDirector.Resume();

            // if you use SimpleAudioEngine, it must resume here
            // SimpleAudioEngine::sharedEngine()->resumeBackgroundMusic();
        }
    }
}