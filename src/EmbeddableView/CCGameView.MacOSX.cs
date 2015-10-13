using System;
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

        private NSTrackingArea _trackingArea;

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

        #region Constructors

        void BeginInitialise()
        {
            RenderFrame += RenderScene;
        }

        #endregion Constructors


        #region Initialisation
        public override bool AcceptsFirstResponder()
        {
            return true;//return base.AcceptsFirstResponder();
        }
        void PlatformInitialise()
        {
            IsMouseVisible = true;



            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.DidBecomeActiveNotification, (n)=> Paused = false);
            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.DidResignActiveNotification, (n)=> Paused = true);

            // Exiting of application
            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.WillTerminateNotification, (n)=> { RenderFrame -= RenderScene; });
            NSNotificationCenter.DefaultCenter.AddObserver(NSWindow.WillCloseNotification, (n)=> { RenderFrame -= RenderScene; });

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

        #endregion Cleaning up 


        #region Run loop

        public void PlatformUpdatePaused()
        {
            if (Paused)
                RenderFrame -= RenderScene;
            else
                RenderFrame += RenderScene;
            
            MobilePlatformUpdatePaused();
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

        #endregion Run loop


        #region Touch handling

        private bool IsMouseVisible { get; set; }

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
                | NSTrackingAreaOptions.MouseMoved
                , this, null);
            this.AddTrackingArea(_trackingArea);
        }

        public override void MouseEntered (NSEvent theEvent)
        {
            base.MouseEntered (theEvent);
            OnDragChange ("Mouse Entered");
        }

        public override void MouseExited (NSEvent theEvent)
        {
            base.MouseExited (theEvent);
            OnDragChange ("Mouse Exited");
        }

        public override void MouseMoved (NSEvent theEvent)
        {
            base.MouseMoved (theEvent);
            OnDragChange ("Mouse Moved");

        }

        void OnDragChange (string description)
        {
            //Console.WriteLine(description);
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

        void PlatformUpdateTouchEnabled()
        {
            
        }

        public override void MouseDown(NSEvent theEvent)
        {
            base.MouseDown(theEvent);
            touchPhase = NSTouchPhase.Began;
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
            touchPhase = NSTouchPhase.Any;
        }

        void UpdateMousePosition (NSEvent theEvent)
        {
            var location = ConvertPointFromView(theEvent.LocationInWindow, null);

            if (!IsFlipped)
                location.Y = Frame.Size.Height - location.Y;

            var position = new CCPoint((float)location.X, (float)location.Y);
            var id = theEvent.EventNumber;

            //Console.WriteLine("mouse event: " + location + " view " + locationInView);
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

        #endregion Touch handling

        #region Mouse handling

        void PlatformUpdateMouseEnabled()
        {
        }


        #endregion Mouse handling
    }
}

