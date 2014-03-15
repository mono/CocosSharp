using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{

    public enum CCEventType
    {
        TOUCH,
        KEYBOARD,
        ACCELERATION,
        MOUSE,
        CUSTOM
    }

    public class CCEvent
    {

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
