using System;
using System.Collections.Generic;
using System.Threading;

namespace CocosSharp
{

    public partial class CCGameView
    {

        bool mouseEnabled;

        Dictionary<int, CCEventMouse> mouseMap;
        List<CCEventMouse> incomingMoveMouse;

        object mouseLock = new object();

        #region Properties

        public bool MouseEnabled
        {
            get { return mouseEnabled; }
            set
            {
                mouseEnabled = value;
                PlatformUpdateMouseEnabled();
            }
        }

        #endregion Properties


        #region Initialisation

        void InitialiseDesktopInputHandling()
        {

            mouseMap = new Dictionary<int, CCEventMouse>();
            incomingMoveMouse = new List<CCEventMouse>();

            MouseEnabled = CCDevice.IsMousePresent;

        }

        #endregion Initialisation

        #region Mouse handling

        void UpdateIncomingMoveMouse(int touchId, ref CCPoint position)
        {
            lock (mouseLock)
            {
                var mouse = new CCEventMouse(CCMouseEventType.MOUSE_MOVE, position);
                incomingMoveMouse.Add(mouse);

            }
        }

        void ProcessMouseInput()
        {
            lock (mouseLock) 
            {
                if (EventDispatcher.IsEventListenersFor(CCEventListenerMouse.LISTENER_ID))
                {
                    if (incomingMoveMouse.Count > 0)
                    {
                        EventDispatcher.DispatchEvent(incomingMoveMouse[0]);
                    }

                    incomingMoveMouse.Clear();
                }
            }
        }

        #endregion Mouse handling
    }
}

