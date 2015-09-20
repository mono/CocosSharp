using System;
using System.Collections.Generic;

namespace CocosSharp
{

    public enum CCEventType
    {
        TOUCH,
        KEYBOARD,
        ACCELERATION,
        MOUSE,
		GAMEPAD,
        CUSTOM
    }

    public class CCEvent
    {

		// The application will come to foreground.
		// This message is used for reloading resources before come to foreground on Android.
		public static string EVENT_COME_TO_FOREGROUND = "event_come_to_foreground";

		// The application will come to background.
		// This message is used for doing something before coming to background, such as save RenderTexture.
		public static string EVENT_COME_TO_BACKGROUND = "event_come_to_background";

		/// <summary>
        /// The event type
        /// </summary>
        internal CCEventType Type { get; private set; }
        /// <summary>
        /// Returns or sets whether propogation for the event is stopped or not
        /// </summary>
        public bool IsStopped { get; set; }
        /// <summary>
        /// The current target that this event is working on
        /// </summary>
		public CCNode CurrentTarget { get; internal set; }

        internal CCEvent(CCEventType type)
        {
            Type = type;
			IsStopped = false;
        }

		/// <summary>
		/// Stops propagation for current event
		/// </summary>
		public void StopPropogation()
		{
			IsStopped = true;
		}
    }
}
