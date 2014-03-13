using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCMouseDispatcher
    {

        public delegate void MouseDownEventHandler(int button, int x, int y);
        public delegate void MouseUpEventHandler(int button, int x, int y);
        public delegate void MouseMoveEventHandler(int x, int y);
        public delegate void MouseScrollEventHandler(int delta);

        public event MouseDownEventHandler MouseDown;

        public bool DispatchMouseState()
        {

            // Read the current keyboard state
//            MouseState currentMouseState = Mouse.GetState();

//            // Check for pressed/released keys.
//            // Loop for each possible pressed key (those that are pressed this update)
//            //Keys[] keys = currentKeyState.GetPressedKeys();

//            locked = true;
//            dispatchingEvents = false;

            if (MouseDown != null)
            {
                MouseDown (5, 30, 40);
            }
//            if (delegates.Count > 0)
//            {
//                for (int i = 0; i < delegates.Count; i++)
//                {
//                    CCMouseHandler handler = delegates[i];
//                    ICCMouseDelegate mouseDelegate = handler.Delegate;
//                    CCPoint pos;
//                    int posX = 0;
//                    int posY = 0;

//#if NETFX_CORE
//                    pos = TransformPoint(priorMouseState.X, priorMouseState.Y);
//                    pos = CCDrawManager.ScreenToWorld(pos.X, pos.Y);
//#else
//                    pos = CCDrawManager.ScreenToWorld(priorMouseState.X, priorMouseState.Y);
//#endif
//                    // We will only do the cast once.
//                    posX = (int)pos.X;
//                    posY = (int)pos.Y;

//                    if ((mouseDelegate.MouseMode & CCMouseMode.MouseMove) == CCMouseMode.MouseMove)
//                    {
//                        lastMouseId++;
//                        mouseDelegate.MouseMove(posX, posY);
//                    }

//                    if ((mouseDelegate.MouseMode & CCMouseMode.MouseDown) == CCMouseMode.MouseDown)
//                    {
//                        CCMouseButton mouseButton = 0;
//                        if (priorMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
//                        {
//                            mouseButton |= CCMouseButton.LeftButton;
//                        }
//                        if (priorMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed)
//                        {
//                            mouseButton |= CCMouseButton.RightButton;
//                        }
//                        if (priorMouseState.MiddleButton == ButtonState.Released && currentMouseState.MiddleButton == ButtonState.Pressed)
//                        {
//                            mouseButton |= CCMouseButton.MiddleButton;
//                        }
//                        if (priorMouseState.XButton1 == ButtonState.Released && currentMouseState.XButton1 == ButtonState.Pressed)
//                        {
//                            mouseButton |= CCMouseButton.ExtraButton1;
//                        }
//                        if (priorMouseState.XButton2 == ButtonState.Released && currentMouseState.XButton2 == ButtonState.Pressed)
//                        {
//                            mouseButton |= CCMouseButton.ExtraButton1;
//                        }

//                        if (mouseButton > 0)
//                            mouseDelegate.MouseDown(mouseButton, posX, posY);
//                    }

//                    if ((mouseDelegate.MouseMode & CCMouseMode.MouseUp) == CCMouseMode.MouseUp)
//                    {
//                        CCMouseButton mouseButton = 0;
//                        if (priorMouseState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
//                        {
//                            mouseButton |= CCMouseButton.LeftButton;
//                        }
//                        if (priorMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released)
//                        {
//                            mouseButton |= CCMouseButton.RightButton;
//                        }
//                        if (priorMouseState.MiddleButton == ButtonState.Pressed && currentMouseState.MiddleButton == ButtonState.Released)
//                        {
//                            mouseButton |= CCMouseButton.MiddleButton;
//                        }
//                        if (priorMouseState.XButton1 == ButtonState.Pressed && currentMouseState.XButton1 == ButtonState.Released)
//                        {
//                            mouseButton |= CCMouseButton.ExtraButton1;
//                        }
//                        if (priorMouseState.XButton2 == ButtonState.Pressed && currentMouseState.XButton2 == ButtonState.Released)
//                        {
//                            mouseButton |= CCMouseButton.ExtraButton1;
//                        }

//                        if (mouseButton > 0)
//                            mouseDelegate.MouseUp(mouseButton, posX, posY);
//                    }

//                    if ((mouseDelegate.MouseMode & CCMouseMode.ScrollWheel) == CCMouseMode.ScrollWheel)
//                    {
//                        var delta = priorMouseState.ScrollWheelValue - currentMouseState.ScrollWheelValue;
//                        if (delta != 0)
//                            mouseDelegate.MouseScroll(delta);
//                    }
//                }
//            }

            //dispatchingEvents = false;

            //if (mdeToAdd)
            //{
            //    for (int i = 0; i < mouseDownEventHandlersToAdd.Count; ++i)
            //    {
            //        MouseDown += mouseDownEventHandlersToAdd[i];
            //    }
            //    mouseDownEventHandlersToAdd.Clear();
            //    mdeToAdd = false;
            //}

            //if (mdeToRemove)
            //{
            //    for (int i = 0; i < mouseDownEventHandlersToRemove.Count; ++i)
            //    {
            //        MouseDown -= mouseDownEventHandlersToRemove[i];
            //    }
            //    mouseDownEventHandlersToRemove.Clear();
            //    mdeToRemove = false;
            //}

//            if (toRemove)
//            {
//                toRemove = false;
//                for (int i = 0; i < handlersToRemove.Count; ++i)
//                {
//                    ForceRemoveDelegate(handlersToRemove[i]);
//                }
//                handlersToRemove.Clear();
//            }

            
//            if (toAdd)
//            {
//                toAdd = false;
//                for (int i = 0; i < handlersToAdd.Count; ++i)
//                {
//                    ForceAddDelegate(handlersToAdd[i]);
//                }
//                handlersToAdd.Clear();
//            }

//            // Store the state for the next loop
//            priorMouseState = currentMouseState;

            return true;
        }

    }
}