using System;

#if ANDROID
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Microsoft.Xna.Framework;
#endif

namespace tests
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif

#if ANDROID
    [Activity(
        Label = "Tests",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)
    ]
    public class Activity1 : AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Game1.Activity = this;
            var game = new Game1();

            var frameLayout = new FrameLayout(this);
            frameLayout.AddView(game.Window);
            this.SetContentView(frameLayout);

            //SetContentView(game.Window);
            game.Run(GameRunBehavior.Asynchronous);
        }
    }
#endif

}

