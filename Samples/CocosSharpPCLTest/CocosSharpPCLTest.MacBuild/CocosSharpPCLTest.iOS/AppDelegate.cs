using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CocosSharp;

namespace CocosSharpPCLTest
{
	public class AppDelegate : AppDelegateCommon
	{
		public override string PlatformMessage()
		{
			return "From Xamarin.iOS - One PCL to rule them all.";
		}

		public AppDelegate(Game game, GraphicsDeviceManager graphics = null)
			: base(game, graphics)
		{
		}
	}
}

