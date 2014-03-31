using System;
using System.Diagnostics;

namespace CocosSharp
{
	public class CCEventListenerAccelerometer : CCEventListener
	{
		public static string LISTENER_ID = "__cc_accel";

		// Event callback function for Accelerometer events
		public Action<CCEventAccelerate> OnAccelerate { get; set; }

		public override bool IsAvailable 
		{
			get { return true; }
		}


		public CCEventListenerAccelerometer() : base(CCEventListenerType.ACCELEROMETER, LISTENER_ID)
		{
			// Set our call back action to be called on accelerometer events so they can be 
			// propagated to the listener.
			Action<CCEvent> listener = kEvent =>
			{
				var accEvent = (CCEventAccelerate)kEvent;
				if (OnAccelerate != null)
					OnAccelerate(accEvent);
			};

			OnEvent = listener;
		}	

		internal CCEventListenerAccelerometer (CCEventListenerAccelerometer accelerometer)
			: this()
		{
			OnAccelerate = accelerometer.OnAccelerate;
		}

		public override CCEventListener Copy()
		{
			return new CCEventListenerAccelerometer (this);
		}
	}
}

