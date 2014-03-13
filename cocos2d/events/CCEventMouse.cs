using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    public class CCEventMouse : CCEvent
    {
        public CCMouseEventType MouseEventType { get; protected set; }

        // Set mouse scroll data 
        public float ScrollX { get; internal set; }
        public float ScrollY { get; internal set; }

        // Set mouse position
        public float CursorX { get; internal set; }
        public float CursorY { get; internal set; }

        public int MouseButton { get; internal set; }

        internal CCEventMouse(CCMouseEventType mouseEventType)
            : base (CCEventType.MOUSE)
        {
            MouseEventType = mouseEventType;

        }
    }
}
