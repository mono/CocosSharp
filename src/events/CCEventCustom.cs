using System;

namespace CocosSharp
{
	public class CCEventCustom : CCEvent
	{
		// Set the UserData
		public object UserData { get; set; }
		public string EventName { get; internal set; }

		public CCEventCustom(string eventName)
			: base (CCEventType.CUSTOM)
		{ 
			EventName = eventName;
		}

		public CCEventCustom(string eventName, object userData)
			: this (eventName)
		{ 
			UserData = userData;
		}

	}
}

