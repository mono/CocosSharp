using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CocosSharpPCLTest
{
	[Register ("AppDelegate")]
	public class Program : UIApplicationDelegate 
	{
		Game1 game;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			game = new Game1();
			game.Run();

			return true;
		}

		static void Main (string[] args)
		{
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
