using System;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{

	public enum CCKeyboardEventType
	{
		KEYBOARD_NONE,
		KEYBOARD_PRESS,
		KEYBOARD_RELEASE,
		KEYBOARD_STATE
	}

	public class CCEventKeyboard : CCEvent
	{
		public CCKeyboardEventType KeyboardEventType { get; internal set; }

		// Set the Keys data 
		public Keys Keys { get; internal set; }

		internal CCEventKeyboard(CCKeyboardEventType keyboardEventType)
			: base (CCEventType.KEYBOARD)
		{
			KeyboardEventType = keyboardEventType;
		}
	}
}

