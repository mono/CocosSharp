using System.Reflection;
using Microsoft.Xna.Framework;
using Cocos2D;
using CocosDenshion;

namespace GameStarterKit
{
	public class AppDelegate : CCApplication
	{

		private int preferredWidth;
		private int preferredHeight;

		public AppDelegate(Game game, GraphicsDeviceManager graphics)
			: base(game, graphics)
		{
			s_pSharedApplication = this;
			// Set the preferred dimensions of your game, this is also known as
			// your target resolution.
			preferredWidth = 480;
			preferredHeight = 320;
			graphics.PreferredBackBufferWidth = preferredWidth;
			graphics.PreferredBackBufferHeight = preferredHeight;
			// Note the orientation here, must match what you specify in the Activity attributes.
            CCDrawManager.InitializeDisplay(game, 
			                              graphics, 
			                              DisplayOrientation.LandscapeRight | DisplayOrientation.LandscapeLeft);
			
			// Anti-aliasing
			graphics.PreferMultiSampling = false;
			
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


			// 2D projection
			pDirector.Projection = CCDirectorProjection.Projection2D;

			// var resPolicy = CCResolutionPolicy.ExactFit; // This will stretch out your game
			var resPolicy = CCResolutionPolicy.ShowAll; // This will letterbox your game

			CCDrawManager.SetDesignResolutionSize(preferredWidth, 
			                                      preferredHeight, 
			                                      resPolicy);

			// turn on display FPS which will show frame rate in the lower left corner
			//pDirector.DisplayStats = true;

			// set FPS. the default value is 1.0/60f if you don't call this
			pDirector.AnimationInterval = 1.0 / 30f;
			
			CCScene pScene = IntroLayer.Scene;

			pDirector.RunWithScene(pScene);
			return true;
		}
		
		/// <summary>
		/// The function be called when the application enters the background
		/// </summary>
		public override void ApplicationDidEnterBackground()
		{
            // stop all of the animation actions that are running.
			CCDirector.SharedDirector.Pause();
			
			// if you use SimpleAudioEngine, your music must be paused
			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic = true;
		}
		
		/// <summary>
		/// The function be called when the application enter foreground  
		/// </summary>
		public override void ApplicationWillEnterForeground()
		{
            CCDirector.SharedDirector.ResumeFromBackground();
			
			// if you use SimpleAudioEngine, your background music track must resume here. 
			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic = false;

		}
	}
}