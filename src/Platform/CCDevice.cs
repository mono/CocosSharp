using Microsoft.Xna.Framework;
#if ANDROID
using Android.App;
using Android.Util;
#endif

#if NETFX_CORE
using Windows.Devices;
#endif
#if WINDOWS || WINDOWSGL

#endif

namespace CocosSharp
{

    public static class CCDevice
    {
        public static float DPI
        {
			get 
			{
				float dpi;
#if ANDROID
				var display = Game.Activity.WindowManager.DefaultDisplay;
				var metrics = new DisplayMetrics();

				display.GetMetrics(metrics);

				dpi = metrics.Density * 160.0f;
#else
				//TODO: Implementing GetDPI for all platforms
				dpi = 96;
#endif

				return dpi;
			}
        }

        public static bool IsMousePresent
        {
            get
            {
#if NETFX_CORE
                var MouseCapabilities = new Windows.Devices.Input.MouseCapabilities();
                return (MouseCapabilities.MousePresent != 0);
#else
                return false;
#endif
            }
        }
    }
}