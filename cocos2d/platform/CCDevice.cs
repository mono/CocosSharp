#if ANDROID
using Android.App;
using Android.Util;
#endif

#if WINDOWS

#endif

namespace Cocos2D
{

    public static class CCDevice
    {
        private static float _dpi;

        public static float GetDPI()
        {
            if (_dpi == 0)
            {
#if ANDROID
                var contex = (Activity)CCApplication.SharedApplication.Game.Window.Context;
                var display = contex.WindowManager.DefaultDisplay;
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
