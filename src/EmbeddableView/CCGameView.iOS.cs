using System;
using System.ComponentModel;
using UIKit;
using Foundation;
using ObjCRuntime;
using CoreAnimation;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;
using OpenTK.Platform;
using OpenTK.Platform.iPhoneOS;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RectangleF=CoreGraphics.CGRect;
using SizeF=CoreGraphics.CGSize;
using OpenGLES;

namespace CocosSharp
{
    class GameViewTimeSource  
    {
        TimeSpan timeout;
        NSTimer timer;

        CCGameView view;

        public GameViewTimeSource(CCGameView view, double updatesPerSecond)
        {
            this.view = view;

            // Can't use TimeSpan.FromSeconds() as that only has 1ms
            // resolution, and we need better (e.g. 60fps doesn't fit nicely
            // in 1ms resolution, but does in ticks).
            timeout = new TimeSpan((long) (((1.0 * TimeSpan.TicksPerSecond) / updatesPerSecond) + 0.5));
        }

        public void Suspend()
        {
            if (timer != null) {
                timer.Invalidate();
                timer = null;
            }
        }

        public void Resume()
        {
            if (timeout != new TimeSpan (-1)) 
            {
                timer = NSTimer.CreateRepeatingTimer(timeout, view.RunIteration);
                NSRunLoop.Main.AddTimer(timer, NSRunLoopMode.Common);
            }
        }

        public void Invalidate()
        {
            if (timer != null) 
            {
                timer.Invalidate();
                timer = null;
            }
        }
    }


    [Register("CCGameView"), DesignTimeVisible(true)]
    public partial class CCGameView : iPhoneOSGameView
    {
        bool bufferCreated;
        uint depthbuffer;

        GameViewTimeSource timeSource;


        #region Constructors

        [Export("initWithCoder:")]
        public CCGameView(NSCoder coder) 
            : base(coder)
        {
            BeginInitiliase();
        }

        public CCGameView(RectangleF frame)
            : base(frame)
        {
            BeginInitiliase();
        }

        void BeginInitiliase()
        {
            LayerRetainsBacking = true;
            LayerColorFormat = EAGLColorFormat.RGBA8;
            ContextRenderingApi = EAGLRenderingAPI.OpenGLES2;

            CreateFrameBuffer();
        }

        #endregion Constructors


        #region Initialisation

        void PlatformInitialise()
        {
            AutoResize = true;
            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidEnterBackgroundNotification, (n)=> Paused = true);
            NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillEnterForegroundNotification, (n)=> Paused = false);
        }

        void PlatformInitialiseGraphicsDevice(ref PresentationParameters presParams)
        {
        }

        void PlatformStartGame()
        {
            if (timeSource !=null)
                timeSource.Invalidate();

            timeSource = new GameViewTimeSource(this, 60.0f);

            CreateFrameBuffer();

            timeSource.Resume();
        }

        protected override void CreateFrameBuffer()
        {
            // Fetch desired depth / stencil size
            // Need to more robustly handle

            if (bufferCreated)
                return;

            base.CreateFrameBuffer();

            MakeCurrent();

            CAEAGLLayer eaglLayer = (CAEAGLLayer) Layer;

            var newSize = new System.Drawing.Size(
                (int) Math.Round(eaglLayer.Bounds.Size.Width), 
                (int) Math.Round(eaglLayer.Bounds.Size.Height));

            GL.GenRenderbuffers(1, out depthbuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthbuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, newSize.Width, newSize.Height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferSlot.DepthAttachment, RenderbufferTarget.Renderbuffer, depthbuffer);

            Threading.BackgroundContext = new OpenGLES.EAGLContext(EAGLContext.API, EAGLContext.ShareGroup);

            Size = newSize;

            Initialise();

            // For iOS, MonoGame's GraphicsDevice needs to maintain reference to default framebuffer
            graphicsDevice.glFramebuffer = Framebuffer;

            bufferCreated = true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ViewSize = new CCSizeI(Size.Width, Size.Height);
            viewportDirty = true;
        }

        public override void LayoutSubviews()
        {
            // Called when the dimensions of our view change
            // E.g. When rotating the device and autoresizing

            if((Framebuffer + depthbuffer + Renderbuffer == 0) || EAGLContext == null)
                return;

            var newSize = new System.Drawing.Size(
                (int) Math.Round(Layer.Bounds.Size.Width), 
                (int) Math.Round(Layer.Bounds.Size.Height));

            Size = newSize;

            var eaglLayer = Layer as CAEAGLLayer;

            // Do not call base because iPhoneOSGameView:LayoutSubviews
            // destroys our graphics context
            // Instead we will manually rejig our buffer storage

            MakeCurrent();

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Renderbuffer);
            EAGLContext.RenderBufferStorage((uint) All.Renderbuffer, eaglLayer);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferSlot.ColorAttachment0, RenderbufferTarget.Renderbuffer, Renderbuffer);

            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthbuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferInternalFormat.DepthComponent16, newSize.Width, newSize.Height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferSlot.DepthAttachment, RenderbufferTarget.Renderbuffer, depthbuffer);

            platformInitialised = true;
            LoadGame();
        }

        #endregion Initialisation


        #region Cleaning up

        void PlatformDispose(bool disposing)
        {
            if (disposing)
            {
                if (timeSource != null)
                {
                    timeSource.Invalidate();
                    timeSource = null;
                }
            }

            NSNotificationCenter.DefaultCenter.RemoveObserver(this, UIApplication.DidEnterBackgroundNotification);
            NSNotificationCenter.DefaultCenter.RemoveObserver(this, UIApplication.WillEnterForegroundNotification);
        }

        protected override void DestroyFrameBuffer()
        {
            MakeCurrent();

            GL.DeleteRenderbuffers (1, ref depthbuffer);
            depthbuffer = 0;

            base.DestroyFrameBuffer();

            bufferCreated = false;
        }

        #endregion Cleaning up 


        #region Run loop

        public void PlatformUpdatePaused()
        {
            if (Paused)
                timeSource.Suspend();
            else
                timeSource.Resume();
        }

        internal void RunIteration(NSTimer timer)
        {
            OnUpdateFrame(null);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Framebuffer);

            OnRenderFrame(null);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            if (GraphicsContext == null || GraphicsContext.IsDisposed)
                return;


            if (!GraphicsContext.IsCurrent)
                MakeCurrent();

            Draw();

            Present();
        }

        void PlatformPresent()
        {
            if (graphicsDevice != null)
                graphicsDevice.Present();

            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Tick();
        }

        #endregion Run loop


        #region Touch handling

        void PlatformUpdateTouchEnabled()
        {
            UserInteractionEnabled = TouchEnabled;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan (touches, evt);
            FillTouchCollection (touches);
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded (touches, evt);
            FillTouchCollection (touches);
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved (touches, evt);
            FillTouchCollection (touches);
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled (touches, evt);
            FillTouchCollection (touches);
        }

        void FillTouchCollection(NSSet touches)
        {
            if (touches.Count == 0)
                return;

            foreach (UITouch touch in touches) 
            {
                var location = touch.LocationInView(touch.View);
                var position = new CCPoint((float)location.X, (float)location.Y);

                var id = touch.Handle.ToInt32();

                switch (touch.Phase) 
                {
                    case UITouchPhase.Moved:
                        UpdateIncomingMoveTouch(id, ref position);                   
                        break;
                    case UITouchPhase.Began:
                        AddIncomingNewTouch(id, ref position);
                        break;
                    case UITouchPhase.Ended :
                    case UITouchPhase.Cancelled :
                        UpdateIncomingReleaseTouch(id);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion Touch handling
    }
}

