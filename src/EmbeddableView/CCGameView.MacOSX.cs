using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using MonoMac.CoreAnimation;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.OpenGL;
using MonoMac.AppKit;

using CGRect = System.Drawing.RectangleF;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace CocosSharp
{

    [Register("CCGameView"), DesignTimeVisible(true)]
    public partial class CCGameView  : MonoMac.OpenGL.MonoMacGameView
    {

        Dictionary<int, CCTouch> touchMap;
        List<CCTouch> incomingNewTouches;
        List<CCTouch> incomingMoveTouches;
        List<CCTouch> incomingReleaseTouches;

        bool touchEnabled;

        object touchLock = new object();

        private NSTrackingArea _trackingArea;


        #region Properties

        public bool TouchEnabled
        {
            get { return touchEnabled; }
            set
            {
                touchEnabled = value;
            }
        }

        #endregion Properties

        #region Constructors

        [Export("initWithFrame:")]
        public CCGameView (CGRect frame) : this(frame, null)
        {
        }

        public CCGameView (CGRect frame, NSOpenGLContext context) : base(frame, context)
        {
            ViewSize = new CCSizeI((int)frame.Width, (int)frame.Height);
            viewportDirty = true;

            BeginInitialise();
        }
 
        #endregion Constructors


        #region Initialisation

        void BeginInitialise()
        {
            RenderFrame += RenderScene;
        }

        public override bool AcceptsFirstResponder()
        {
            return true;
        }

        void PlatformInitialise()
        {

            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.DidBecomeActiveNotification, (n)=> Paused = false);
            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.DidResignActiveNotification, (n)=> Paused = true);

            // Exiting of application
            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.WillTerminateNotification, (n)=> { RenderFrame -= RenderScene; });
            NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.WillCloseNotification, (n)=> { RenderFrame -= RenderScene; });

            NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.WillEnterFullScreenNotification, (n)=> { RenderFrame -= RenderScene; });
            NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.DidEnterFullScreenNotification, (n)=> { RenderFrame += RenderScene; });

            NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.WillExitFullScreenNotification, (n)=> { RenderFrame -= RenderScene; });
            NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.DidExitFullScreenNotification, (n)=> { RenderFrame += RenderScene; });
        }

        void PlatformInitialiseGraphicsDevice(ref PresentationParameters presParams)
        {
        }

        void PlatformStartGame()
        {

            Run(1 / targetElapsedTime.TotalSeconds);
            // Reminder: We may need to do something like this later.
            //OpenGLContext.SwapInterval = graphicsDeviceManager.SynchronizeWithVerticalRetrace;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ViewSize = new CCSizeI((int)Size.Width, (int)Size.Height);
            viewportDirty = true;
        }

 
        public override void ViewDidMoveToWindow()
        {
            base.ViewDidMoveToWindow();

            Initialise();
            platformInitialised = true;
        }

        void InitialiseInputHandling ()
        {
            touchMap = new Dictionary<int, CCTouch>();
            incomingNewTouches = new List<CCTouch>();
            incomingMoveTouches = new List<CCTouch>();
            incomingReleaseTouches = new List<CCTouch>();

            TouchEnabled = CCDevice.IsMousePresent;

            InitialiseDesktopInputHandling();
        }

        #endregion Initialisation


        #region Cleaning up


        void PlatformDispose(bool disposing)
        {
            if (disposing)
            {
                RenderFrame -= RenderScene;
            }

            NSNotificationCenter.DefaultCenter.RemoveObserver(NSApplication.DidBecomeActiveNotification);
            NSNotificationCenter.DefaultCenter.RemoveObserver(NSApplication.DidResignActiveNotification);

            // TODO: Fix up exiting of application
            NSNotificationCenter.DefaultCenter.RemoveObserver(NSApplication.WillTerminateNotification);
            NSNotificationCenter.DefaultCenter.RemoveObserver(NSWindow.WillCloseNotification);
        }

        bool PlatformCanDisposeGraphicsDevice()
        {
            bool canDispose = true;

            try {
                MakeCurrent();
            } catch (Exception) {
                canDispose = false;
            }

            return canDispose;
        }

        #endregion Cleaning up 


        #region Run loop

        public void PlatformUpdatePaused()
        {
            if (Paused)
                RenderFrame -= RenderScene;
            else
                RenderFrame += RenderScene;
            
        }

        void RenderScene (object sender, FrameEventArgs e)
        {
            Tick();

            Draw();

            Present();
        }

        void PlatformPresent()
        {
            if (graphicsDevice != null)
                graphicsDevice.Present();
        }

        void ProcessInput()
        {
            lock (touchLock)
            {
                if (EventDispatcher.IsEventListenersFor(CCEventListenerTouchOneByOne.LISTENER_ID)
                    || EventDispatcher.IsEventListenersFor(CCEventListenerTouchAllAtOnce.LISTENER_ID))
                {
                    var touchEvent = new CCEventTouch(CCEventCode.BEGAN);

                    RemoveOldTouches();

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

            // The desktop implementation already takes care of the mouse input so just call it
            ProcessDesktopInput();

        }
        #endregion Run loop

 
        #region Touch handling

        void PlatformUpdateTouchEnabled()
        {
            
        }

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

        void UpdateIncomingReleaseTouch(int id)
        {
            lock (touchLock) 
            {
                CCTouch existingTouch;
                if (touchMap.TryGetValue (id, out existingTouch)) 
                {
                    incomingReleaseTouches.Add (existingTouch);
                    touchMap.Remove (id);
                }
            }
        }

        static readonly TimeSpan TouchTimeLimit = TimeSpan.FromMilliseconds(1000);
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


        #region Mouse handling

        void PlatformUpdateMouseEnabled()
        {
            UpdateTrackingAreas();
        }

        void PlatformUpdateMouseVisible()
        {
            ResetCursorRects();
        }

        public override void UpdateTrackingAreas()
        {
            base.UpdateTrackingAreas();
            if (_trackingArea != null)
            {
                this.RemoveTrackingArea(_trackingArea);

            }

            _trackingArea = new NSTrackingArea (RectangleF.Empty, NSTrackingAreaOptions.ActiveAlways
                | NSTrackingAreaOptions.InVisibleRect 
                | NSTrackingAreaOptions.MouseEnteredAndExited 
                | ((MouseEnabled) ? NSTrackingAreaOptions.MouseMoved : NSTrackingAreaOptions.ActiveAlways)
                , this, null);
            this.AddTrackingArea(_trackingArea);
        }

        public override void MouseEntered (NSEvent theEvent)
        {
            base.MouseEntered (theEvent);
        }

        public override void MouseExited (NSEvent theEvent)
        {
            base.MouseExited (theEvent);
        }

        public override void MouseMoved (NSEvent theEvent)
        {
            base.MouseMoved (theEvent);
            if (MouseEnabled)
            {
                var location = ConvertPointFromView(theEvent.LocationInWindow, null);

                if (!IsFlipped)
                    location.Y = Frame.Size.Height - location.Y;

                var position = new CCPoint((float)location.X, (float)location.Y);
                var id = theEvent.EventNumber;

                AddIncomingMouse(id, ref position);
            }

        }

        // These variables are to handle our custom cursor for when IsMouseVisible is false.
        // Hiding and unhiding the cursor was such a pain that I decided to let the system
        // take care of this with Cursor Rectangles
        NSImage cursorImage = null; // Will be set to our custom image
        NSCursor cursor = null;     // Our custom cursor
        public override void ResetCursorRects ()
        {

            // If we do not have a cursor then we create an image size 1 x 1
            // and then create our custom cursor with clear colors
            if (cursor == null) {
                cursorImage = new NSImage(new SizeF(1,1));
                cursor = new NSCursor(cursorImage, NSColor.Clear, NSColor.Clear, new PointF(0,0));
            }

            // if the cursor is not to be visible then we us our custom cursor.
            if (!IsMouseVisible)
                AddCursorRect(VisibleRect(), cursor);
            else
                AddCursorRect(VisibleRect(), NSCursor.ArrowCursor);

        }

        NSTouchPhase touchPhase = NSTouchPhase.Any;

        public override void MouseDown(NSEvent theEvent)
        {
            base.MouseDown(theEvent);
            touchPhase = NSTouchPhase.Began;
            IsLeftButtonPressed = true;
            UpdateMousePosition (theEvent);
        }

        public override void MouseDragged(NSEvent theEvent)
        {
            base.MouseDragged(theEvent);
            touchPhase = NSTouchPhase.Moved;
            UpdateMousePosition (theEvent);
        }

        public override void MouseUp(NSEvent theEvent)
        {
            base.MouseUp(theEvent);
            touchPhase = NSTouchPhase.Ended;
            UpdateMousePosition (theEvent);
            IsLeftButtonPressed = false;
            touchPhase = NSTouchPhase.Any;
        }


        public override void RightMouseDown (NSEvent theEvent)
        {
            base.RightMouseDown(theEvent);
            touchPhase = NSTouchPhase.Began;
            IsRightButtonPressed = true;
            UpdateMousePosition (theEvent);
        }

        public override void RightMouseUp (NSEvent theEvent)
        {
            base.RightMouseUp(theEvent);
            touchPhase = NSTouchPhase.Ended;
            UpdateMousePosition (theEvent);
            IsRightButtonPressed = false;
            touchPhase = NSTouchPhase.Any;
        }

        public override void RightMouseDragged (NSEvent theEvent)
        {
            base.RightMouseDragged(theEvent);
            touchPhase = NSTouchPhase.Moved;
            UpdateMousePosition (theEvent);
        }

        public override void ScrollWheel(NSEvent theEvent)
        {
            base.ScrollWheel(theEvent);
            var location = ConvertPointFromView(theEvent.LocationInWindow, null);

            if (!IsFlipped)
                location.Y = Frame.Size.Height - location.Y;

            var position = new CCPoint((float)location.X, (float)location.Y);

            switch (theEvent.Type) 
            { 
                case NSEventType.ScrollWheel: 
                    if (theEvent.ScrollingDeltaY != 0) 
                    { 

                        var id = theEvent.Handle.ToInt32();
                        var wheelDeltaY = 0f;
                        if (theEvent.ScrollingDeltaY > 0) 
                        { 
                            wheelDeltaY += (theEvent.ScrollingDeltaY * 0.1f + 0.09f) * 1200; 
                        } 
                        else 
                        { 
                            wheelDeltaY += (theEvent.ScrollingDeltaY * 0.1f - 0.09f) * 1200; 
                        } 
                        AddIncomingScrollMouse(id, ref position, wheelDeltaY);

                    } 
                    break; 
            } 

        }

        #endregion Mouse handling

        #region Common Input Handling

        CCMouseButton buttons = CCMouseButton.None;
        bool IsLeftButtonPressed;
        bool IsRightButtonPressed;

        void UpdateMousePosition (NSEvent theEvent)
        {
            var location = ConvertPointFromView(theEvent.LocationInWindow, null);

            if (!IsFlipped)
                location.Y = Frame.Size.Height - location.Y;

            var position = new CCPoint((float)location.X, (float)location.Y);
            var id = theEvent.EventNumber;

            if (TouchEnabled)
            {
                switch (touchPhase)
                {
                    case NSTouchPhase.Began:
                        AddIncomingNewTouch(id, ref position);
                        break;
                    case NSTouchPhase.Moved:
                        UpdateIncomingMoveTouch(id, ref position);
                        break;
                    case NSTouchPhase.Ended:
                    case NSTouchPhase.Cancelled:
                        UpdateIncomingReleaseTouch(id); 
                        break;
                    default:
                        break;

                }
            }

            if (MouseEnabled)
            {

                switch (touchPhase)
                {

                    case NSTouchPhase.Began:
                        buttons = CCMouseButton.None;
                        buttons |= IsLeftButtonPressed ? CCMouseButton.LeftButton : CCMouseButton.None;
                        buttons |= IsRightButtonPressed ? CCMouseButton.RightButton : CCMouseButton.None;
                        AddIncomingMouse(id, ref position, buttons);
                        break;
                    case NSTouchPhase.Moved:
                        UpdateIncomingMouse(id, ref position, buttons);
                        break;
                    case NSTouchPhase.Ended:
                    case NSTouchPhase.Cancelled:
                        var buttonsReleased = CCMouseButton.None;
                        if (buttons.HasFlag(CCMouseButton.LeftButton) && !IsLeftButtonPressed)
                            buttonsReleased = CCMouseButton.LeftButton;
                        if (buttons.HasFlag(CCMouseButton.RightButton) && !IsRightButtonPressed)
                            buttonsReleased |= CCMouseButton.RightButton;
                        UpdateIncomingReleaseMouse(id, buttons);
                        break;
                    default:
                        break;

                }
            }

        }
        #endregion Common Input Handling
    }
}

