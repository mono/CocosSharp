using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace CocosSharp
{
	public class CCEventListenerTouchOneByOne : CCEventListener
	{
		public static string LISTENER_ID = "__cc_touch_one_by_one";

		// Event callback function for Touch Began events
		public Func<CCTouch,CCEvent,bool> OnTouchBegan { get; set; }
		// Event callback function for Touch Moved events
		public Action<CCTouch,CCEvent> OnTouchMoved { get; set; }
		// Event callback function for Touch Ended events
		public Action<CCTouch,CCEvent> OnTouchEnded { get; set; }
		// Event callback function for Touch Canceled events
		public Action<CCTouch,CCEvent> OnTouchCancelled { get; set; }


		public bool IsSwallowTouches { get; set; }
		internal List<CCTouch> ClaimedTouches { get; set; }

		public override bool IsAvailable {
			get {
				if (OnTouchBegan == null)
				{
					Debug.Assert(false, "Invalid EventListenerTouchOneByOne!");
					return false;
				}

				return true;			
			}
		}


		public CCEventListenerTouchOneByOne() : base(CCEventListenerType.TOUCH_ONE_BY_ONE, LISTENER_ID)
		{
			ClaimedTouches = new List<CCTouch> ();
//			// Set our call back action to be called on touch events so they can be 
//			// propagated to the listener.
//			Action<CCEvent> listener = tEvent =>
//			{
//				var touchEvent = (CCEventTouch)tEvent;
//				switch (touchEvent.EventCode)
//				{
//				case CCEventCode.BEGAN:
//					if (OnTouchBegin != null)
//						OnTouchBegin(touchEvent);
//					break;
//				case CCEventCode.ENDED:
//					if (OnTouchEnded != null)
//						OnTouchEnded(touchEvent);
//					break;
//				case CCEventCode.MOVED:
//					if (OnTouchMoved != null)
//						OnTouchMoved(touchEvent);
//					break;
//				case CCEventCode.CANCELLED:
//					if (OnTouchCancelled != null)
//						OnTouchCancelled(touchEvent);
//					break;
//
//				default:
//					break;
//				}
//
//			};
//			OnEvent = listener;
		}	
	}
}

