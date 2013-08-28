using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Cocos2D;

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
            graphics.DeviceCreated += new EventHandler<EventArgs>(graphics_DeviceCreated);
            // graphics.ApplyChanges();
            Content.RootDirectory = "Content";

//            if (graphics.GraphicsDevice == null)
//            {
//                CCLog.Log("FOO");
//            }

            graphics.IsFullScreen = false;
#if WINDOWS || MACOS
            graphics.PreferredDepthStencilFormat = Microsoft.Xna.Framework.Graphics.DepthFormat.Depth24Stencil8;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
#endif
            // Frame rate is 30 fps by default for Windows Phone.
            // Divide by 2 to make it 60 fps
            TargetElapsedTime = TimeSpan.FromTicks(333333 / 2);
            IsFixedTimeStep = true;

            IsMouseVisible = true;

            // Extend battery life under lock.
            //InactiveSleepTime = TimeSpan.FromSeconds(1);

            CCApplication application = new AppDelegate(this, graphics);
            Components.Add(application);

#if !WINDOWS_PHONE && !XBOX && !WINRT && !WINDOWSDX && !NETFX_CORE
            //GamerServicesComponent component = new GamerServicesComponent(this);
            //this.Components.Add(component);
#endif
        }

        void graphics_DeviceCreated(object sender, EventArgs e)
        {
            CCLog.Log("Graphics device was created!");
        }

#if OUYA
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            CCDrawManager.spriteBatch.Begin();
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

                CCDrawManager.spriteBatch.DrawString(CCSpriteFontCache.SharedInstance.GetFont("arial-20"), textToDraw, new Vector2(16, y), Color.White);
                y += 25;
            }
            CCDrawManager.spriteBatch.End();

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