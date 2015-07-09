using System;
using UIKit;
using Foundation;
using ObjCRuntime;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.iPhoneOS;

using RectangleF=CoreGraphics.CGRect;
using SizeF=CoreGraphics.CGSize;
using OpenGLES;

namespace CocosSharp
{
    public partial class CCGameView : iPhoneOSGameView
    {
        #region Constructors

        public CCGameView(NSCoder coder) 
            : base(coder)
        {
            LayerRetainsBacking = true;
            LayerColorFormat = EAGLColorFormat.RGBA8;
            CreateFrameBuffer();

            Initialise();
        }

        public CCGameView(RectangleF frame)
            : base(frame)
        {
            LayerRetainsBacking = true;
            LayerColorFormat = EAGLColorFormat.RGBA8;
            CreateFrameBuffer();

            Initialise();
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
            ContextRenderingApi = EAGLRenderingAPI.OpenGLES2;

            // Fetch desired depth / stencil size
            // Need to more robustly handle

            try {
                base.CreateFrameBuffer();
                // Kick start the render loop
                // In particular, graphics context is lazily created, so we need to start this up 
                // here so that the view is initialised correctly
                Run();
                return;
            } catch (Exception ex) {      
            }
        }

        #endregion Initialisation


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            if (GraphicsContext == null || GraphicsContext.IsDisposed)
                return;


            if (!GraphicsContext.IsCurrent)
                MakeCurrent();

            Draw();

            PlatformPresent();
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

