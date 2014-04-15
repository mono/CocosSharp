using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CocosSharp;

namespace CocosSharpPCLTest
{
	public class Game1 : Game
	{
		readonly GraphicsDeviceManager graphics;

		public Game1()
		{
			CCApplication application = new AppDelegate(this);
			Components.Add(application);
		}

		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();

			base.Update(gameTime);
		}
	}
}