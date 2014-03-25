using Microsoft.Xna.Framework;
using CocosDenshion;
using CocosSharp;

namespace GoneBananas
{
    public class GoneBananasApplication : CCApplication
    {
		public GoneBananasApplication (Game game, GraphicsDeviceManager graphics = null)
			: base (game, graphics)
        {
			// Set our supported orientations for those that can use them
			SupportedOrientations = CCDisplayOrientation.Portrait;
        }

        public override bool ApplicationDidFinishLaunching ()
        {
            //initialize director
            CCDirector director = CCDirector.SharedDirector;

            // turn on display FPS
			director.DisplayStats = true;

            // set FPS. the default value is 1.0/60 if you don't call this
            director.AnimationInterval = 1.0 / 60;

			// We will setup our Design Resolution here
			if (CCDrawManager.FrameSize.Height > 480)
			{
				CCContentManager.SharedContentManager.SearchPaths.Add("hd");
			}

            CCScene scene = GameStartLayer.Scene;
            director.RunWithScene (scene);

            // returning true indicates the app initialized successfully and can continue
            return true;
        }

        public override void ApplicationDidEnterBackground ()
        {
            // stop all of the animation actions that are running.
            CCDirector.SharedDirector.Pause ();
			
            // if you use SimpleAudioEngine, your music must be paused
            CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic ();
        }

        public override void ApplicationWillEnterForeground ()
        {
            CCDirector.SharedDirector.Resume ();
			
            // if you use SimpleAudioEngine, your background music track must resume here. 
            CCSimpleAudioEngine.SharedEngine.ResumeBackgroundMusic ();
        }
    }
}