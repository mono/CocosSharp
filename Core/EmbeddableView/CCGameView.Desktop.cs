using System;
using System.Collections.Generic;
using System.Threading;

namespace CocosSharp
{

    public partial class CCGameView
    {

        bool mouseEnabled;

        Dictionary<int, CCEventMouse> mouseMap;
        List<CCEventMouse> incomingNewMouse;
        List<CCEventMouse> incomingMoveMouse;
        List<CCEventMouse> incomingReleaseMouse;
        List<CCEventMouse> incomingScrollMouse;

        object mouseLock = new object();

        bool mouseVisible;

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

        public bool IsMouseVisible
        {
            get { return mouseVisible; }
            set
            {
                mouseVisible = value;
                PlatformUpdateMouseVisible();
            }
        }

        #endregion Properties


        #region Initialisation

        void InitialiseDesktopInputHandling()
        {

            mouseMap = new Dictionary<int, CCEventMouse>();
            incomingNewMouse = new List<CCEventMouse>();
            incomingMoveMouse = new List<CCEventMouse>();
            incomingReleaseMouse = new List<CCEventMouse>();
            incomingScrollMouse = new List<CCEventMouse>();

            IsMouseVisible = MouseEnabled = CCDevice.IsMousePresent;

        }

        #endregion Initialisation

        #region Mouse handling

        void AddIncomingMouse(int id, ref CCPoint position, CCMouseButton buttons = CCMouseButton.None)
        {
            lock (mouseLock)
            {
                if (!mouseMap.ContainsKey(id))
                {
                    var mouse = new CCEventMouse(((buttons == CCMouseButton.None) ? CCMouseEventType.MOUSE_MOVE : CCMouseEventType.MOUSE_DOWN),
                        id, position, gameTime.ElapsedGameTime);
                    mouse.MouseButton = buttons;
                    incomingNewMouse.Add(mouse);
                    mouseMap.Add(id, mouse);
                }
            }
        }

        void UpdateIncomingMouse(int id, ref CCPoint position, CCMouseButton buttons = CCMouseButton.None)
        {
            lock (mouseLock)
            {
                CCEventMouse existingMouse;
                if (mouseMap.TryGetValue(id, out existingMouse))
                {
                    existingMouse.MouseEventType = CCMouseEventType.MOUSE_MOVE;
                    existingMouse.MouseButton = buttons;
                    existingMouse.CursorOnScreen = position;
                    incomingMoveMouse.Add(existingMouse);
                }
            }
        }

        void UpdateIncomingReleaseMouse(int id, CCMouseButton buttons = CCMouseButton.None)
        {
            lock (mouseLock)
            {
                CCEventMouse existingMouse;
                if (mouseMap.TryGetValue(id, out existingMouse))
                {
                    existingMouse.MouseEventType = CCMouseEventType.MOUSE_UP;
                    existingMouse.MouseButton = buttons;

                    incomingReleaseMouse.Add(existingMouse);
                    mouseMap.Remove(id);
                }
            }
        }

        void AddIncomingScrollMouse(int id, ref CCPoint position, float wheelDelta)
        {
            lock (mouseLock)
            {
                if (!mouseMap.ContainsKey(id))
                {
                    var mouse = new CCEventMouse(CCMouseEventType.MOUSE_SCROLL,
                        id, position, gameTime.ElapsedGameTime);
                    mouse.ScrollY += wheelDelta;
                    incomingScrollMouse.Add(mouse);
                    mouseMap.Add(id, mouse);
                }
            }
        }

        void ProcessDesktopInput()
        {
            lock (mouseLock)
            {

                if (EventDispatcher.IsEventListenersFor(CCEventListenerMouse.LISTENER_ID))
                {

                    foreach (var mouseEvent in incomingNewMouse)
                    {
                        EventDispatcher.DispatchEvent(mouseEvent);

                        // Mouse Move events do not have a corresponding release event so they may
                        // not be removed from processing so after processing one we will just
                        // remove it so the next mouse move event can be added again.
                        if (mouseEvent.MouseEventType == CCMouseEventType.MOUSE_MOVE)
                            mouseMap.Remove(mouseEvent.Id);
                    }

                    foreach (var mouseEvent in incomingMoveMouse)
                    {
                        EventDispatcher.DispatchEvent(mouseEvent);
                    }

                    foreach (var mouseEvent in incomingReleaseMouse)
                    {
                        EventDispatcher.DispatchEvent(mouseEvent);
                    }

                    foreach (var mouseEvent in incomingScrollMouse)
                    {
                        EventDispatcher.DispatchEvent(mouseEvent);
                        mouseMap.Remove(mouseEvent.Id);
                    }

                    incomingNewMouse.Clear();
                    incomingMoveMouse.Clear();
                    incomingReleaseMouse.Clear();
                    incomingScrollMouse.Clear();
                }

            }
        }

        #endregion Mouse handling
    }
}

