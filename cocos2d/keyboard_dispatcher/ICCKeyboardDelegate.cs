using System;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{
    [Flags]
	public enum CCKeyboardMode
	{
		KeyPressed = 0x1,
		KeyReleased = 0x2,
		KeyboardState = 0x4,
		All = KeyPressed | KeyReleased | KeyboardState
	}

	public interface ICCKeyboardDelegate
	{
		// Key was pressed
		void KeyPressed(Keys keys);

		// Current Keyboard State
		void KeyboardCurrentState(KeyboardState currentState);

		// Key was released
		void KeyReleased(Keys keys);

		CCKeyboardMode KeyboardMode { get; set; }

	}

	public class CCKeyboardHandler 
	{
		public CCKeyboardHandler (ICCKeyboardDelegate keyboardDelegate)
		{
			Delegate = keyboardDelegate;
		}

		public ICCKeyboardDelegate Delegate { get; set; }
	}
}

