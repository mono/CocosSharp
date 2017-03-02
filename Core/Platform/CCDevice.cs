using Microsoft.Xna.Framework;
#if ANDROID
using Android.App;
using Android.Util;
using Android.Runtime;
using Android.Views;
#endif

#if NETFX_CORE
using Windows.Devices;
using Windows.Graphics.Display;
#endif
#if IOS
using CoreAnimation;
using UIKit;
using Foundation;
#endif
#if MACOS
using MonoMac.AppKit;
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
                var context = Android.App.Application.Context;
                var windowManager = context.GetSystemService("window").JavaCast<IWindowManager>(); 
                var display = windowManager.DefaultDisplay;
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
#elif IOS
                return (float)UIScreen.MainScreen.Scale;
#elif MACOS
                return NSScreen.MainScreen.BackingScaleFactor;
#elif ANDROID
                var context = Android.App.Application.Context;
                var windowManager = context.GetSystemService("window").JavaCast<IWindowManager>(); 
                var display = windowManager.DefaultDisplay;
				var metrics = new DisplayMetrics();

				display.GetMetrics(metrics);

				return metrics.ScaledDensity;
#else
                return 1;
#endif
            }
        }

        public static bool IsTouchScreenPresent
        {
            get
            {
#if NETFX_CORE
                var touchCapabilities = new Windows.Devices.Input.TouchCapabilities();
                return (touchCapabilities.TouchPresent != 0);
#elif MACOS
                // We will just always assume that it doesn't
                return false;
#else
                return true;
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
#elif MACOS
                // We will just always assume that it does
                return true;
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
#elif MACOS
                // We will just always assume that it does
                return true;
#else
                return false;
#endif
            }
        }

        public static bool IsGamepadPresent
        {
            get
            {
#if NETFX_CORE
                return Windows.Gaming.Input.Gamepad.Gamepads.Count > 0;
#else
                return false;
#endif
            }
        }
    }
}