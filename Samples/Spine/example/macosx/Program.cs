using System;
using MonoMac;
using MonoMac.AppKit;
using MonoMac.Foundation;
using CocosSharp;
using Microsoft.Xna.Framework;

namespace spine_cocossharp
{
	class Program : NSApplicationDelegate 
	{
		//Game1 game;

		static void Main (string[] args)
		{
			NSApplication.Init ();

			using (var p = new NSAutoreleasePool()) 
			{
				NSApplication.SharedApplication.Delegate = new Program();
				NSApplication.Main(args);
			}

		}

		public override void FinishedLaunching (NSObject notification)
		{
			CCApplication sharedApp = CCApplication.SharedApplication;
			sharedApp.ApplicationDelegate = new AppDelegate();

			CCApplication.SharedApplication.StartGame();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
}

