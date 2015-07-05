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
    public partial class CCGameView : AndroidGameView, View.IOnTouchListener, ISurfaceHolderCallback
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

            Tick();
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

        void PlatformUpdateTouchEnabled()
        {
            SetOnTouchListener(touchEnabled ? this : null);
        }

        bool IOnTouchListener.OnTouch(View v, MotionEvent e)
        {
            if (!TouchEnabled)
                return true;

            CCPoint position = new CCPoint(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
            int id = e.GetPointerId(e.ActionIndex);
            switch (e.ActionMasked)
            {              
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    AddIncomingNewTouch(id, ref position);
                    break;              
                case MotionEventActions.Up:
                case MotionEventActions.PointerUp:
                    UpdateIncomingReleaseTouch(id);
                    break;             
                case MotionEventActions.Move:
                    for (int i = 0; i < e.PointerCount; i++)
                    {
                        id = e.GetPointerId(i);
                        position.X = e.GetX(i);
                        position.Y = e.GetY(i);
                        UpdateIncomingMoveTouch(id, ref position);
                    }
                    break;               
                case MotionEventActions.Cancel:
                case MotionEventActions.Outside:
                    for (int i = 0; i < e.PointerCount; i++)
                    {
                        id = e.GetPointerId(i);
                        UpdateIncomingReleaseTouch(id);
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
    }
}

