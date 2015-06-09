using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
    public partial class CCGameView
    {
        bool gameStarted;

        internal GraphicsDevice GraphicsDevice { get; set; }

        public void StartGame()
        {
            if(!gameStarted)
            {
                PlatformStartGame();
                gameStarted = true;
            }
        }

        internal void Present()
        {
            PlatformPresent();
        }
    }
}

