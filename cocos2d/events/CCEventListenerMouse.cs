using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCEventListenerMouse : CCEventListener
    {

        public static string LISTENER_ID = "__cc_mouse";

        // Event callback function for Mouse Down events
        public Action<CCEventMouse> OnMouseDown { get; set; }
        // Event callback function for Mouse Up events
        public Action<CCEventMouse> OnMouseUp { get; set; }
        // Event callback function for Mouse Move events
        public Action<CCEventMouse> OnMouseMove { get; set; }
        // Event callback function for Mouse Scroll events
        public Action<CCEventMouse> OnMouseScroll { get; set; }

        public CCEventListenerMouse() : base(CCEventListenerType.MOUSE, LISTENER_ID)
        {

            Action<CCEvent> listener = mEvent =>
                {
                    var mouseEvent = (CCEventMouse)mEvent;
                    switch (mouseEvent.MouseEventType)
                    {
                        case CCMouseEventType.MOUSE_DOWN:
                            if (OnMouseDown != null)
                                OnMouseDown(mouseEvent);
                            break;
                        case CCMouseEventType.MOUSE_UP:
                            if (OnMouseUp != null)
                                OnMouseUp(mouseEvent);
                            break;
                        case CCMouseEventType.MOUSE_MOVE:
                            if(OnMouseMove != null)
                                OnMouseMove(mouseEvent);
                            break;
                        case CCMouseEventType.MOUSE_SCROLL:
                            if(OnMouseScroll != null)
                                OnMouseScroll(mouseEvent);
                            break;
                        default:
                            break;
                    }

                };
            OnEvent = listener;
        }
    }
}
