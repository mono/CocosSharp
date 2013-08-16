using System;
using Microsoft.Xna.Framework.Input;

namespace Cocos2D
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
		void KeyPressed(Keys key);

		// Current Keyboard State
		void KeyboardCurrentState(KeyboardState currentState);

		// Key was released
		void KeyReleased(Keys key);

		CCKeyboardMode KeyboardMode { get; set; }

	}

	public class CCKeyboardHandler 
	{
		protected ICCKeyboardDelegate m_pDelegate;

		public CCKeyboardHandler (ICCKeyboardDelegate pDelegate)
		{
			InitWithDelegate (pDelegate);
		}

		public ICCKeyboardDelegate Delegate
		{
			get { return m_pDelegate; }
			set { m_pDelegate = value; }
		}

		/** initializes a CCKeyboardHandler with a delegate */

		public virtual bool InitWithDelegate(ICCKeyboardDelegate pDelegate)
		{
			m_pDelegate = pDelegate;
			return true;
		}

	}
}

