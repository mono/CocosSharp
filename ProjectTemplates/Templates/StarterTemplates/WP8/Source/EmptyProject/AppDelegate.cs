using System.Reflection;
using Microsoft.Xna.Framework;
using CocosSharp;
using CocosDenshion;

namespace EmptyProject
{
	public class AppDelegate : CCApplication
	{

		// TODO: Set your design resolution
		private int preferredWidth;
		private int preferredHeight;

		public AppDelegate(Game game, GraphicsDeviceManager graphics = null)
			: base(game, graphics)
		{

			//preferredWidth = 480;
			//preferredHeight = 320;
            preferredWidth = 800;
            preferredHeight = 480;

			// TODO: Set your preferred window dimensions, this will set a resolution
			// that fits the hardware. You do not have to set this, so remove these lines
			// if you want default behavior.
			PreferredBackBufferWidth = 800;
			PreferredBackBufferHeight = 480;

			PreferMultiSampling = false;
			
		}
		
		/// <summary>
		/// Implement for initialize OpenGL instance, set source path, etc...
		/// </summary>
        //public override bool InitInstance()
        //{
        //    return base.InitInstance();
        //}
		
		/// <summary>
		///  Implement CCDirector and CCScene init code here.
		/// </summary>
		/// <returns>
		///  true  Initialize success, app continue.
		///  false Initialize failed, app terminate.
		/// </returns>
		public override bool ApplicationDidFinishLaunching()
		{

            ContentRootDirectory = "Content";

			//initialize director
			var director = CCDirector.SharedDirector;

			// 2D projection
			director.Projection = CCDirectorProjection.Projection2D;

			var resPolicy = CCResolutionPolicy.ShowAll; // This will letterbox your game

			CCDrawManager.SetDesignResolutionSize(preferredWidth, 
			                                      preferredHeight, 
			                                      resPolicy);

			// turn on display FPS
			//pDirector.DisplayStats = true;

			// set FPS. the default value is 1.0/60 if you don't call this
			director.AnimationInterval = 1.0 / 60;
			
			CCScene pScene = IntroLayer.Scene;

			director.RunWithScene(pScene);
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
            CCDirector.SharedDirector.Resume();
			
			// if you use SimpleAudioEngine, your background music track must resume here. 
			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic = false;

		}
	}
}