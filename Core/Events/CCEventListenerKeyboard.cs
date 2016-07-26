using System;
using System.Diagnostics;

namespace CocosSharp
{
	public class CCEventListenerKeyboard : CCEventListener
	{
		public static string LISTENER_ID = "__cc_keyboard";

		// Event callback function for Key Press events
		public Action<CCEventKeyboard> OnKeyPressed { get; set; }
		// Event callback function for Key Release events
		public Action<CCEventKeyboard> OnKeyReleased { get; set; }
		// Event callback function for Keyboard State events
		//public Action<CCEventKeyboard> OnKeyboardState { get; set; }

		public override bool IsAvailable {
			get {
				if (OnKeyPressed == null && OnKeyReleased == null)
				{
					Debug.Assert(false, "Invalid EventListenerKeyboard!");
					return false;
				}

				return true;			
			}
		}


		public CCEventListenerKeyboard() : base(CCEventListenerType.KEYBOARD, LISTENER_ID)
		{
			// Set our call back action to be called on mouse events so they can be 
			// propagated to the listener.
			Action<CCEvent> listener = kEvent =>
			{
				var keyboardEvent = (CCEventKeyboard)kEvent;
				switch (keyboardEvent.KeyboardEventType)
				{
				case CCKeyboardEventType.KEYBOARD_PRESS:
					if (OnKeyPressed != null)
						OnKeyPressed(keyboardEvent);
					break;
				case CCKeyboardEventType.KEYBOARD_RELEASE:
					if (OnKeyReleased != null)
						OnKeyReleased (keyboardEvent);
					break;
				default:
					break;
				}

			};
			OnEvent = listener;
		}	

		internal CCEventListenerKeyboard(CCEventListenerKeyboard keyboard)
			: this()
		{
			OnKeyPressed = keyboard.OnKeyPressed;
			OnKeyReleased = keyboard.OnKeyReleased;
		}

		public override CCEventListener Copy()
		{
			return new CCEventListenerKeyboard (this);
		}
	}
}

