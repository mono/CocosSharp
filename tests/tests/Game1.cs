using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CocosSharp;

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

            // Extend battery life under lock.
            //InactiveSleepTime = TimeSpan.FromSeconds(1);

            //CCApplication application = new AppDelegate(this);
            //Components.Add(application);

#if !WINDOWS_PHONE && !XBOX && !WINRT && !WINDOWSDX && !NETFX_CORE
            //GamerServicesComponent component = new GamerServicesComponent(this);
            //this.Components.Add(component);
#endif
        }
            
#if OUYA
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
			CCDrawManager.SpriteBatch.Begin();
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

				CCDrawManager.SpriteBatch.DrawString(CCSpriteFontCache.SharedInstance["arial-20"], textToDraw, new Vector2(16, y), Color.White);
                y += 25;
            }
			CCDrawManager.SpriteBatch.End();

        }
#endif

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            #if !IOS
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            #endif

            // TODO: Add your update logic here


            base.Update(gameTime);
        }
    }
}