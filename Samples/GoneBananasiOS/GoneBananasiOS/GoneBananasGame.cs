using System;
using Microsoft.Xna.Framework;
using CocosSharp;

namespace GoneBananas
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GoneBananasGame : Game
    {
        public GoneBananasGame()
        {
			// Here we tell our game where to load our content.
			Content.RootDirectory = "Content";

			CCApplication application = new GoneBananasApplication (this);
            Components.Add(application);
        }
    }
}