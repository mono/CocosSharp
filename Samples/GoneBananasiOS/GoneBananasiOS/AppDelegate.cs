using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace GoneBananas
{
    [Register ("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        GoneBananasGame game;

        public override void FinishedLaunching (UIApplication app)
        {
            game = new GoneBananasGame();
            game.Run();
        }
    }
}

