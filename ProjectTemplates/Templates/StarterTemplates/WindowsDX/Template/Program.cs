using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

using CocosSharp;

namespace $safeprojectname$
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			CCApplication application = new CCApplication(false, new CCSize(1024f, 768f));
			application.ApplicationDelegate = new AppDelegate();

			application.StartGame();
		}
    }


}

