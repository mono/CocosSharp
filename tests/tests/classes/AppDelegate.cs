using System.Reflection;
using FarseerPhysics.TestBed.Tests;
using Microsoft.Xna.Framework;
using cocos2d;
using tests.classes.tests.Box2DTestBet;


namespace tests
{
    public class AppDelegate : CCApplication
    {
        public AppDelegate(Game game, GraphicsDeviceManager graphics)
            : base(game, graphics)
        {
            s_pSharedApplication = this;
            DrawManager.InitializeDisplay(game, graphics, DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft);


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

            DrawManager.SetDesignResolutionSize(480, 320, ResolutionPolicy.ShowAll);

            // turn on display FPS
            pDirector.DisplayStats = true;

            // pDirector->setDeviceOrientation(kCCDeviceOrientationLandscapeLeft);

            // set FPS. the default value is 1.0/60 if you don't call this
            pDirector.AnimationInterval = 1.0 / 60;

            // create a scene. it's an autorelease object
            CCScene pScene = CCScene.Create();
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