using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CocosSharp
{
	public class CCEventListenerTouchAllAtOnce : CCEventListener
	{
		public static string LISTENER_ID = "__cc_touch_all_at_once";

		// Event callback function for Touches Began events
		public Action<List<CCTouch>,CCEvent> OnTouchesBegan { get; set; }
		// Event callback function for Touches Moved events
		public Action<List<CCTouch>,CCEvent> OnTouchesMoved { get; set; }
		// Event callback function for Touches Ended events
		public Action<List<CCTouch>,CCEvent> OnTouchesEnded { get; set; }
		// Event callback function for Touch Canceled events
		public Action<List<CCTouch>,CCEvent> OnTouchesCancelled { get; set; }

		public override bool IsAvailable {
			get {
				if (OnTouchesBegan == null && OnTouchesMoved == null
					&& OnTouchesEnded == null && OnTouchesCancelled == null)
				{
					Debug.Assert(false, "Invalid EventListenerTouchAllAtOnce!");
					return false;
				}

				return true;			
			}
		}


		public CCEventListenerTouchAllAtOnce() : base(CCEventListenerType.TOUCH_ALL_AT_ONCE, LISTENER_ID)
		{	}	
	}
}

