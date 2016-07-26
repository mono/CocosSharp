using System;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace EmptyProject.WindowsDX
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        //private readonly GraphicsDeviceManager graphics;

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

            // TODO: Add your update logic here


            base.Update(gameTime);
        }
    }
}