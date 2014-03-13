using System;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{

    public enum CCMouseEventType
    {
        MOUSE_NONE,
        MOUSE_DOWN,
        MOUSE_UP,
        MOUSE_MOVE,
        MOUSE_SCROLL,
    }

	[Flags]
	public enum CCMouseButton
	{
		None = 0x0,
		LeftButton = 0x1,
		MiddleButton = 0x2,
		RightButton = 0x4,
		ExtraButton1 = 0x8,
		ExtraButton2 = 0x16
	}
    public class CCEventMouse : CCEvent
    {
		public CCMouseEventType MouseEventType { get; internal set; }

        // Set mouse scroll data 
        public float ScrollX { get; internal set; }
        public float ScrollY { get; internal set; }

        // Set mouse position
        public float CursorX { get; internal set; }
        public float CursorY { get; internal set; }

		public CCMouseButton MouseButton { get; internal set; }

        internal CCEventMouse(CCMouseEventType mouseEventType)
            : base (CCEventType.MOUSE)
        {
            MouseEventType = mouseEventType;
        }
    }
}
