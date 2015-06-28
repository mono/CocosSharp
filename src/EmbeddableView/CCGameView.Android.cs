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
    public partial class CCGameView : AndroidGameView
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
            Run();
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
                return;
            } catch (Exception ex) {
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            if (GraphicsContext == null || GraphicsContext.IsDisposed)
                return;

            Draw();

            PlatformPresent();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!GraphicsContext.IsCurrent)
                MakeCurrent();
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
    }
}

