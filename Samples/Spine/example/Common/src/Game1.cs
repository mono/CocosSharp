using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CocosSharp;

namespace spine_cocossharp
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {

        public Game1()
        {

            CCApplication application = new AppDelegate(this);
            Components.Add(application);
        }

        private void ProcessBackClick()
        {
            if (CCDirector.SharedDirector.IsCanPopScene)
            {
                CCDirector.SharedDirector.PopScene();
            }
            else
            {
                Exit();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                ProcessBackClick();
            }

            // TODO: Add your update logic here


            base.Update(gameTime);
        }
    }
}