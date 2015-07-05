using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public partial class CCGameView
    {
        bool touchEnabled;

        Dictionary<int, CCTouch> touchMap;
        List<CCTouch> incomingNewTouches;
        List<CCTouch> incomingMoveTouches;
        List<CCTouch> incomingReleaseTouches;


        #region Properties

        public bool TouchEnabled
        {
            get { return touchEnabled; }
            set
            {
                touchEnabled = value;
                PlatformUpdateTouchEnabled();
            }
        }

        #endregion Properties


        #region Initialisation

        void InitialiseInputHandling()
        {
            touchMap = new Dictionary<int, CCTouch>();
            incomingNewTouches = new List<CCTouch>();
            incomingMoveTouches = new List<CCTouch>();
            incomingReleaseTouches = new List<CCTouch>();

            TouchEnabled = true;
        }

        #endregion Initialisation


        #region Touch handling

        void AddIncomingNewTouch(int touchId, ref CCPoint position)
        {
            if (!touchMap.ContainsKey(touchId))
            {
                var touch = new CCTouch(touchId, position, gameTime.TotalGameTime);
                touchMap.Add(touchId, touch);
                incomingNewTouches.Add(touch);
            }
        }

        void UpdateIncomingMoveTouch(int touchId, ref CCPoint position)
        {
            CCTouch existingTouch;
            if (touchMap.TryGetValue(touchId, out existingTouch))
            {
                var delta = existingTouch.LocationOnScreen - position;
                if (delta.LengthSquared > 1.0f)
                {
                    incomingMoveTouches.Add(existingTouch);
                    existingTouch.UpdateTouchInfo(touchId, position.X, position.Y, gameTime.TotalGameTime);
                }
            }
        }

        void UpdateIncomingReleaseTouch(int touchId)
        {
            CCTouch existingTouch;
            if (touchMap.TryGetValue(touchId, out existingTouch))
            {
                incomingReleaseTouches.Add(existingTouch);
                touchMap.Remove(touchId);
            }
        }

        void ProcessInput()
        {
            if (EventDispatcher.IsEventListenersFor(CCEventListenerTouchOneByOne.LISTENER_ID)
                || EventDispatcher.IsEventListenersFor(CCEventListenerTouchAllAtOnce.LISTENER_ID))
            {
                var touchEvent = new CCEventTouch(CCEventCode.BEGAN);

                if (incomingNewTouches.Count > 0)
                {
                    touchEvent.EventCode = CCEventCode.BEGAN;
                    touchEvent.Touches = incomingNewTouches;
                    EventDispatcher.DispatchEvent(touchEvent);
                }

                if (incomingMoveTouches.Count > 0)
                {
                    touchEvent.EventCode = CCEventCode.MOVED;
                    touchEvent.Touches = incomingMoveTouches;
                    EventDispatcher.DispatchEvent(touchEvent);
                }

                if (incomingReleaseTouches.Count > 0)
                {
                    touchEvent.EventCode = CCEventCode.ENDED;
                    touchEvent.Touches = incomingReleaseTouches;
                    EventDispatcher.DispatchEvent(touchEvent);
                }

                incomingNewTouches.Clear();
                incomingMoveTouches.Clear();
                incomingReleaseTouches.Clear();
            }
        }

        void RemoveOldTouches()
        {

        }

        #endregion Touch handling
    }
}

