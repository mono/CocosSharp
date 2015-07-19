using System;
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

using RectangleF=CoreGraphics.CGRect;
using SizeF=CoreGraphics.CGSize;
using OpenGLES;

namespace CocosSharp
{
    public partial class CCGameView : iPhoneOSGameView
    {
        bool bufferCreated;
        uint depthbuffer;

        #region Constructors

        public CCGameView(NSCoder coder) 
            : base(coder)
        {
            LayerRetainsBacking = true;
            LayerColorFormat = EAGLColorFormat.RGBA8;
            ContextRenderingApi = EAGLRenderingAPI.OpenGLES2;
            CreateFrameBuffer();
        }

        public CCGameView(RectangleF frame)
            : base(frame)
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
        }

        void PlatformStartGame()
        {
            Run();
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
        }

        #endregion Initialisation


        #region Cleaning up

        protected override void DestroyFrameBuffer()
        {
            MakeCurrent();

            GL.DeleteRenderbuffers (1, ref depthbuffer);
            depthbuffer = 0;

            base.DestroyFrameBuffer();

            bufferCreated = false;
        }

        #endregion Cleaning up 


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

