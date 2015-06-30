using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Util;

using OpenTK;
using OpenTK.Platform.Android;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;


namespace CocosSharp
{
    public partial class CCGameView : AndroidGameView, ISurfaceHolderCallback
    {
        public CCGameView(Context context) 
            : base(context)
        {
            RenderOnUIThread = false;
            FocusableInTouchMode = true;
        }

        public CCGameView(Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {
            RenderOnUIThread = false;
            FocusableInTouchMode = true;
        }

        void PlatformInitialise()
        {
            RequestFocus();
        }

        void PlatformStartGame()
        {
            Resume();
        }

        protected override void OnContextSet(EventArgs e)
        {
            base.OnContextSet(e);

            Initialise();
        }

        protected override void CreateFrameBuffer()
        {
            ContextRenderingApi = GLVersion.ES2;

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

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Context is already set for us in AndroidGameView, so no need to set it again

            base.OnRenderFrame(e);

            if (GraphicsContext == null || GraphicsContext.IsDisposed)
                return;

            Draw();

            PlatformPresent();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        void PlatformPresent()
        {
            try
            {
                if (graphicsDevice != null)
                    graphicsDevice.Present();

                SwapBuffers();
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("Error in swap buffers", ex.ToString());
            }
        }

        void ISurfaceHolderCallback.SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            ViewSize = new CCSizeI(width, height);
            viewportDirty = true;
        }
    }
}

