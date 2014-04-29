using Microsoft.Xna.Framework;
#if ANDROID
using Android.App;
using Android.Util;
#endif

#if WINDOWS || WINDOWSGL

#endif

namespace CocosSharp
{

    public static class CCDevice
    {
        private static float _dpi;

        public static float GetDPI()
        {
            if (_dpi == 0)
            {
#if ANDROID
                var display = Game.Activity.WindowManager.DefaultDisplay;
                var metrics = new DisplayMetrics();

                display.GetMetrics(metrics);

                _dpi = metrics.Density * 160.0f;
#else
                //TODO: Implementing GetDPI for all platforms
                _dpi = 96;
#endif
            }
            return _dpi;
        }
    }
}