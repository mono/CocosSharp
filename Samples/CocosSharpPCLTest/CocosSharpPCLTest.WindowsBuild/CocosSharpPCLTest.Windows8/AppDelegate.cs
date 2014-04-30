using System.Reflection;
using Microsoft.Xna.Framework;
using CocosSharp;
using CocosDenshion;

namespace CocosSharpPCLTest.Windows8
{
	public class AppDelegate : CCApplication
	{

		int preferredWidth;
		int preferredHeight;

        public static string PlatformMessage()
        {
            return "From Windows 8 - One PCL to rule them all.";
        }

		public AppDelegate(Game game, GraphicsDeviceManager graphics)
			: base(game, graphics)
		{

            SupportedOrientations = CCDisplayOrientation.LandscapeRight | CCDisplayOrientation.LandscapeLeft;
			PreferMultiSampling = false;
			
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
            ContentRootDirectory = "Content";

            //CCSpriteFontCache.FontScale = 0.6f;
            //CCSpriteFontCache.RegisterFont("MarkerFelt", 22);

            CCDirector director = CCDirector.SharedDirector;
            director.DisplayStats = true;
            director.AnimationInterval = 1.0 / 60;

			// turn on display FPS
			director.DisplayStats = true;

			// set FPS. the default value is 1.0/60 if you don't call this
			director.AnimationInterval = 1.0 / 60;

            CCScene scene = new CCScene();

            var label = TestClass.PCLLabel(AppDelegate.PlatformMessage());

            scene.AddChild(label);

            director.RunWithScene(scene);
            
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