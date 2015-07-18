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

            if (Threading.BackgroundContext == null)
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

        #endregion Initialisation


        #region Cleaning up

        protected override void DestroyFrameBuffer()
        {
            base.DestroyFrameBuffer();

            MakeCurrent();

            GL.DeleteRenderbuffers (1, ref depthbuffer);
            depthbuffer = 0;

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


        void PlatformUpdateTouchEnabled()
        {
            
        }
    }
}

