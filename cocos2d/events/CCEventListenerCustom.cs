using System;

namespace CocosSharp
{
	public class CCEventListenerCustom : CCEventListener
	{
		// Event callback function for Custom events
		public Action<CCEventCustom> OnCustomEvent { get; set; }

		public CCEventListenerCustom(string eventName, Action<CCEventCustom> callback) : base(CCEventListenerType.CUSTOM, eventName)
		{	
			OnCustomEvent = callback;

			// Set our call back action to be called on custom events so they can be 
			// propagated to the listener.
			Action<CCEvent> listener = cEvent =>
			{
				var customEvent = (CCEventCustom)cEvent;
				if (OnCustomEvent != null)
					OnCustomEvent(customEvent);
			};
			OnEvent = listener;
		}

		public override bool IsAvailable {
			get 
			{
				return (base.IsAvailable && OnCustomEvent != null);
			}
		}
	}
}

