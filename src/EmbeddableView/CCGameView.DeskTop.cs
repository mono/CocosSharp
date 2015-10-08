using System;
using System.Collections.Generic;
using System.Threading;

namespace CocosSharp
{
    public partial class CCGameView
    {
        static readonly TimeSpan TouchTimeLimit = TimeSpan.FromMilliseconds(1000);

        bool touchEnabled;
        bool prevAccelerometerEnabled;

        Dictionary<int, CCTouch> touchMap;
        List<CCTouch> incomingNewTouches;
        List<CCTouch> incomingMoveTouches;
        List<CCTouch> incomingReleaseTouches;

        object touchLock = new object();

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

        public CCAccelerometer Accelerometer { get; private set; }

        #endregion Properties


        #region Initialisation

        void InitialiseInputHandling()
        {
            touchMap = new Dictionary<int, CCTouch>();
            incomingNewTouches = new List<CCTouch>();
            incomingMoveTouches = new List<CCTouch>();
            incomingReleaseTouches = new List<CCTouch>();

            TouchEnabled = true;

            Accelerometer = new CCAccelerometer(this);
        }

        #endregion Initialisation


        #region Accelerometer handling

        public void MobilePlatformUpdatePaused()
        {
            if (Paused) 
            {
                prevAccelerometerEnabled = Accelerometer.Enabled;
                Accelerometer.Enabled = false;
            } 
            else
                Accelerometer.Enabled = prevAccelerometerEnabled;
        }

        #endregion Accelerometer handling


        #region Touch handling

        void AddIncomingNewTouch(int touchId, ref CCPoint position)
        {
            lock (touchLock) 
            {
                if (!touchMap.ContainsKey (touchId)) 
                {
                    var touch = new CCTouch (touchId, position, gameTime.TotalGameTime);
                    touchMap.Add (touchId, touch);
                    incomingNewTouches.Add (touch);
                }
            }
        }

        void UpdateIncomingMoveTouch(int touchId, ref CCPoint position)
        {
            lock (touchLock) 
            {
                CCTouch existingTouch;
                if (touchMap.TryGetValue (touchId, out existingTouch)) 
                {
                    var delta = existingTouch.LocationOnScreen - position;
                    if (delta.LengthSquared > 1.0f) 
                    {
                        incomingMoveTouches.Add (existingTouch);
                        existingTouch.UpdateTouchInfo (touchId, position.X, position.Y, gameTime.TotalGameTime);
                    }
                }
            }
        }

        void UpdateIncomingReleaseTouch(int touchId)
        {
            lock (touchLock) 
            {
                CCTouch existingTouch;
                if (touchMap.TryGetValue (touchId, out existingTouch)) 
                {
                    incomingReleaseTouches.Add (existingTouch);
                    touchMap.Remove (touchId);
                }
            }
        }

        void ProcessInput()
        {
            lock (touchLock) 
            {
                if (EventDispatcher.IsEventListenersFor (CCEventListenerTouchOneByOne.LISTENER_ID)
                || EventDispatcher.IsEventListenersFor (CCEventListenerTouchAllAtOnce.LISTENER_ID)) 
                {
                    var touchEvent = new CCEventTouch (CCEventCode.BEGAN);

                    RemoveOldTouches ();

                    if (incomingNewTouches.Count > 0) {
                        touchEvent.EventCode = CCEventCode.BEGAN;
                        touchEvent.Touches = incomingNewTouches;
                        EventDispatcher.DispatchEvent (touchEvent);
                    }

                    if (incomingMoveTouches.Count > 0) {
                        touchEvent.EventCode = CCEventCode.MOVED;
                        touchEvent.Touches = incomingMoveTouches;
                        EventDispatcher.DispatchEvent (touchEvent);
                    }

                    if (incomingReleaseTouches.Count > 0) {
                        touchEvent.EventCode = CCEventCode.ENDED;
                        touchEvent.Touches = incomingReleaseTouches;
                        EventDispatcher.DispatchEvent (touchEvent);
                    }

                    incomingNewTouches.Clear ();
                    incomingMoveTouches.Clear ();
                    incomingReleaseTouches.Clear ();
                }
            }
        }

        // Prevent memory leaks by removing stale touches
        // In particular, in the case of the game entering the background
        // a release touch event may not have been triggered within the view
        void RemoveOldTouches()
        {
            lock (touchLock) 
            {
                var currentTime = gameTime.ElapsedGameTime;

                foreach (CCTouch touch in touchMap.Values) 
                {
                    if (!incomingReleaseTouches.Contains (touch)
                    && (currentTime - touch.TimeStamp) > TouchTimeLimit) 
                    {
                        incomingReleaseTouches.Add (touch);
                        touchMap.Remove (touch.Id);
                    }
                }
            }
        }

        #endregion Touch handling
    }
}

