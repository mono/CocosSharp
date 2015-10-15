using Microsoft.Xna.Framework;
#if ANDROID
using Android.App;
using Android.Util;
#endif

#if NETFX_CORE
using Windows.Devices;
using Windows.Graphics.Display;
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
#elif NETFX_CORE
                DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
                dpi = displayInformation.LogicalDpi;
#else
				//TODO: Implementing GetDPI for all platforms
				dpi = 96;
#endif

				return dpi;
			}
        }

        /// <summary>
        /// Gets the scale factor of the Display Device
        /// </summary>
        public static float ResolutionScaleFactor
        {
            get
            {
#if NETFX_CORE
                return (float)DisplayInformation.GetForCurrentView().ResolutionScale / 100f;
#else
                return 1;
#endif
            }
        }

        public static bool IsMousePresent
        {
            get
            {
#if NETFX_CORE
                var mouseCapabilities = new Windows.Devices.Input.MouseCapabilities();
                return (mouseCapabilities.MousePresent != 0);
#else
                return false;
#endif
            }
        }
        public static bool IsKeyboardPresent
        {
            get
            {
#if NETFX_CORE
                var keyboardCapabilities = new Windows.Devices.Input.KeyboardCapabilities();
                return (keyboardCapabilities.KeyboardPresent != 0);
#else
                return false;
#endif
            }
        }
    }
}