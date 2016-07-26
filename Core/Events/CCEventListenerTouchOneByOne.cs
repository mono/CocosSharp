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
					Debug.Assert(false, "Invalid EventListenerTouchOneByOne!.  OnTouchBegan is not defined.");
					return false;
				}

				return true;			
			}
		}


		public CCEventListenerTouchOneByOne() : base(CCEventListenerType.TOUCH_ONE_BY_ONE, LISTENER_ID)
		{
			ClaimedTouches = new List<CCTouch> ();
		}	

		internal CCEventListenerTouchOneByOne(CCEventListenerTouchOneByOne eventListener) 
			: this()
		{
			OnTouchBegan = eventListener.OnTouchBegan;
			OnTouchMoved = eventListener.OnTouchMoved;
			OnTouchEnded = eventListener.OnTouchEnded;
			OnTouchCancelled = eventListener.OnTouchCancelled;

            ClaimedTouches.AddRange(eventListener.ClaimedTouches);
			IsSwallowTouches = eventListener.IsSwallowTouches;

		}

		public override CCEventListener Copy()
		{
			return new CCEventListenerTouchOneByOne (this);
		}
	}
}

