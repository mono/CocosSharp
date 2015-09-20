using System;
using System.Collections.Generic;

namespace CocosSharp
{

	public enum CCEventCode
	{
		BEGAN,
		MOVED,
		ENDED,
		CANCELLED
	}

	public class CCEventTouch : CCEvent
	{
		public CCEventCode EventCode { get; internal set; }

		public List<CCTouch> Touches { get; internal set; }

		internal CCEventTouch(CCEventCode touchCode)
			: base (CCEventType.TOUCH)
		{
			EventCode = touchCode;
            Touches = new List<CCTouch> (5);
		}
	}
}

