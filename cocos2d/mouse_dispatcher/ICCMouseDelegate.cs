using System;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{

	[Flags]
	public enum CCMouseMode
	{
		MouseMove = 0x1,
		ScrollWheel = 0x2,
		MouseDown = 0x4,
		MouseUp = 0x8,
		All = MouseMove | ScrollWheel | MouseDown | MouseUp
	}

	[Flags]
	public enum CCMouseButton
	{
		LeftButton = 0x1,
		MiddleButton = 0x2,
		RightButton = 0x4,
		ExtraButton1 = 0x8,
		ExtraButton2 = 0x16
	}

	public interface ICCMouseDelegate
	{

		// Mouse moved
		void MouseMove (int x, int y);

		// Mouse scroll
		void MouseScroll (int delta);

		// Mouse button pressed
		void MouseDown (CCMouseButton buttons, int x, int y);

		// Mouse button released
		void MouseUp (CCMouseButton button, int x, int y);

		CCMouseMode MouseMode { get; set; }
	}

	public class CCMouseHandler 
	{
		public CCMouseHandler (ICCMouseDelegate keyboardDelegate)
		{
			Delegate = keyboardDelegate;
		}

		public ICCMouseDelegate Delegate { get; set; }
	}
}

