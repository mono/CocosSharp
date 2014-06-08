using System.Reflection;
using Microsoft.Xna.Framework;
using CocosSharp;
using CocosDenshion;

namespace spine_cocossharp
{
	public class AppDelegate : CCApplicationDelegate
	{

		int preferredWidth;
		int preferredHeight;

		/// <summary>
		///  Implement CCDirector and CCScene init code here.
		/// </summary>
		/// <returns>
		///  true  Initialize success, app continue.
		///  false Initialize failed, app terminate.
		/// </returns>
		public override void ApplicationDidFinishLaunching(CCApplication application)
		{
			preferredWidth = 1024;
			preferredHeight = 768;

			application.PreferredBackBufferWidth = preferredWidth;
			application.PreferredBackBufferHeight = preferredHeight;


			application.PreferMultiSampling = true;
			application.ContentRootDirectory = "Content";

			//CCSpriteFontCache.FontScale = 0.5f;
			//CCSpriteFontCache.RegisterFont("MarkerFelt", 22);
			//CCSpriteFontCache.RegisterFont("arial", 12, 24);

			CCDirector director = CCApplication.SharedApplication.MainWindowDirector;
			director.DisplayStats = true;
			director.AnimationInterval = 1.0 / 60;

			CCSize designSize = new CCSize(480, 320);

			if (CCDrawManager.FrameSize.Height > 320)
			{
				//CCSize resourceSize = new CCSize(960, 640);
				CCSize resourceSize = new CCSize(480, 320);
				application.ContentSearchPaths.Add("hd");
				director.ContentScaleFactor = resourceSize.Height / designSize.Height;
			}

			CCDrawManager.SetDesignResolutionSize(designSize.Width, designSize.Height, CCResolutionPolicy.ShowAll);

			// turn on display FPS
			director.DisplayStats = true;

			// set FPS. the default value is 1.0/60 if you don't call this
			director.AnimationInterval = 1.0 / 60;

			CCScene pScene = GoblinLayer.Scene;

			director.RunWithScene(pScene);
		}

		/// <summary>
		/// The function be called when the application enters the background
		/// </summary>
//		public override void ApplicationDidEnterBackground()
//		{
//			// stop all of the animation actions that are running.
//			CCDirector.SharedDirector.Pause();
//
//			// if you use SimpleAudioEngine, your music must be paused
//			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic = true;
//		}

		/// <summary>
		/// The function be called when the application enter foreground  
		/// </summary>
//		public override void ApplicationWillEnterForeground()
//		{
//			CCDirector.SharedDirector.Resume();
//
//			// if you use SimpleAudioEngine, your background music track must resume here. 
//			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic = false;
//
//		}
	}
}