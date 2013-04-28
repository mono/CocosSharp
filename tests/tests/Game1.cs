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
            
            graphics.IsFullScreen = false;

            // Frame rate is 30 fps by default for Windows Phone.
            // Divide by 2 to make it 60 fps
            TargetElapsedTime = TimeSpan.FromTicks(333333 / 2);
            IsFixedTimeStep = true;

            // Extend battery life under lock.
            //InactiveSleepTime = TimeSpan.FromSeconds(1);

            CCApplication application = new AppDelegate(this, graphics);
            Components.Add(application);
        }

#if OUYA
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawManager.spriteBatch.Begin();
            float y = 15;
            for (int i = 0; i < 4; ++i)
            {
                GamePadState gs = GamePad.GetState((PlayerIndex)i, GamePadDeadZone.Circular);
                string textToDraw = string.Format(
                    "Pad: {0} Connected: {1} LS: ({2:F2}, {3:F2}) RS: ({4:F2}, {5:F2}) LT: {6:F2} RT: {7:F2}",
                    i, gs.IsConnected,
                    gs.ThumbSticks.Left.X, gs.ThumbSticks.Left.Y,
                    gs.ThumbSticks.Right.X, gs.ThumbSticks.Right.Y,
                    gs.Triggers.Left, gs.Triggers.Right);

                DrawManager.spriteBatch.DrawString(CCSpriteFontCache.SharedInstance.GetFont("arial-20"), textToDraw, new Vector2(16, y), Color.White);
                y += 25;
            }
            DrawManager.spriteBatch.End();

        }
#endif

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