using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CocosSharp;

namespace PCLTest
{
	public class AppDelegate : CCApplication
	{
		public static string PlatformMessage()
		{
			return "From MonoMac - One PCL to rule them all."
		}

		public AppDelegate(Game game, GraphicsDeviceManager graphics = null)
			: base(game, graphics)
		{
			SupportedOrientations = CCDisplayOrientation.LandscapeRight | CCDisplayOrientation.LandscapeLeft;
			AllowUserResizing = true;
			PreferMultiSampling = false;
			PreferredBackBufferWidth = 960;
			PreferredBackBufferHeight = 640;
		}

		public override bool ApplicationDidFinishLaunching()
		{
			ContentRootDirectory = "Content";

			CCSpriteFontCache.FontScale = 0.6f;
			CCSpriteFontCache.RegisterFont("MarkerFelt", 22);

			CCDirector director = CCDirector.SharedDirector;
			director.DisplayStats = true;
			director.AnimationInterval = 1.0 / 60;

			CCSize designSize = new CCSize (480, 320);

			CCDrawManager.SetDesignResolutionSize (designSize.Width, designSize.Height, CCResolutionPolicy.ShowAll);

			CCScene scene = new CCScene();

			scene.AddChild(TestClass.PCLLabel(AppDelegate.PlatformMessage()));

			director.RunWithScene(scene);

			return true;
		}
	}
}

