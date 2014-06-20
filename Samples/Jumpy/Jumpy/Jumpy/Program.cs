using CocosSharp;
using System;

namespace Jumpy
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            CCApplication sharedApp = CCApplication.SharedApplication;
            sharedApp.ApplicationDelegate = new AppDelegate();
            CCApplication.SharedApplication.StartGame();
        }
    }
#endif
}

