using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using cocos2d;

namespace tests
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333 / 2);

            // Extend battery life under lock.
            //InactiveSleepTime = TimeSpan.FromSeconds(1);

            CCApplication application = new AppDelegate(this, graphics);
            Components.Add(application);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here


            base.Update(gameTime);
        }
    }
}