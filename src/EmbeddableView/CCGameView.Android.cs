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

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;


namespace CocosSharp
{
    public partial class CCGameView : AndroidGameView, View.IOnTouchListener, ISurfaceHolderCallback
    {
        bool startedRunning;
        CCAndroidScreenReceiver screenLockHandler;
        object androidViewLock = new object();


        #region Android screen lock handling inner class

        class CCAndroidScreenReceiver : BroadcastReceiver
        {   
            bool previousPausedState;
            CCGameView gameView;

            public CCAndroidScreenReceiver(CCGameView view)
            {
                gameView = view;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                if(intent.Action == Intent.ActionScreenOff)
                    OnLocked();
                else if(intent.Action == Intent.ActionScreenOn)
                {
                    KeyguardManager keyguard = (KeyguardManager)context.GetSystemService(Context.KeyguardService);
                    if (!keyguard.InKeyguardRestrictedInputMode())
                        OnUnlocked();
                }
                else if(intent.Action == Intent.ActionUserPresent)
                    OnUnlocked();
            }

            void OnLocked()
            {
                previousPausedState = gameView.Paused;
                gameView.Paused = true;
            }

            void OnUnlocked()
            {
                gameView.Paused = previousPausedState;
            }
        }

        #endregion Android screen lock handling inner class


        #region Constructors

        public CCGameView(Context context) 
            : base(context)
        {
            ViewInit();
        }

        public CCGameView(Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {
            ViewInit();
        }

        void ViewInit()
        {
            RenderOnUIThread = false;
            AutoSetContextOnRenderFrame = true;
            RenderThreadRestartRetries = 100;
            FocusableInTouchMode = true;
            ContextRenderingApi = GLVersion.ES2;
        }

        #endregion Constructors


        #region Initialisation

        void PlatformInitialise()
        {
            var context = Android.App.Application.Context;
            MediaLibrary.Context = context;

            IntentFilter filter = new IntentFilter();
            filter.AddAction(Intent.ActionScreenOff);
            filter.AddAction(Intent.ActionScreenOn);
            filter.AddAction(Intent.ActionUserPresent);

            screenLockHandler = new CCAndroidScreenReceiver(this);
            context.RegisterReceiver(screenLockHandler, filter);

            Microsoft.Xna.Framework.Threading.GameView = this;
        }

        void PlatformInitialiseGraphicsDevice(ref PresentationParameters presParams)
        {
        }

        void PlatformStartGame()
        {
            lock (androidViewLock) 
            {
                Resume();
            }
        }

        protected override void OnContextSet(EventArgs e)
        {
            lock (androidViewLock) 
            {
                base.OnContextSet (e);

                Initialise ();

                platformInitialised = true;
                LoadGame ();
            }
        }

        protected override void CreateFrameBuffer()
        {
            // Fetch desired depth / stencil size
            // Need to more robustly handle

            lock (androidViewLock) 
            {
                try 
                {
                    base.CreateFrameBuffer();
                    // Kick start the render loop
                    // In particular, graphics context is lazily created, so we need to start this up 
                    // here so that the view is initialised correctly
                    if (!startedRunning) 
                    {
                        Run();
                        startedRunning = true;
                    }
                    return;
                } catch (Exception) {}
            }
        }

        void InitialiseInputHandling()
        {
            InitialiseMobileInputHandling();
        }

        #endregion Initialisation


        #region Cleaning up

        protected override void OnContextLost(EventArgs e)
        {
            lock (androidViewLock) 
            {
                base.OnContextLost (e);

                if (graphicsDevice != null)
                    graphicsDevice.OnDeviceResetting ();
            }
        }
            
        void PlatformDispose(bool disposing)
        {
            var context = Android.App.Application.Context;
            context.UnregisterReceiver(screenLockHandler);
        }

        #endregion Cleaning up


        #region Run loop

        void PlatformUpdatePaused()
        {
            if (Paused)
            {
                Pause();
                ClearFocus();
            }
            else
            {
                Resume();

                if (!IsFocused)
                    RequestFocus();
            }

            MobilePlatformUpdatePaused();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // Context is already set for us in AndroidGameView, so no need to set it again

            base.OnRenderFrame(e);

            if (Paused || GraphicsContext == null || GraphicsContext.IsDisposed)
                return;

            Draw();

            PlatformPresent();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Tick();
        }

        void ProcessInput()
        {
            ProcessMobileInput();
        }

        #endregion Run loop


        #region Rendering

        void PlatformPresent()
        {
            if (Paused)
                return;

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

        void ISurfaceHolderCallback.SurfaceDestroyed(ISurfaceHolder holder)
        {
            lock (androidViewLock) 
            {
                Paused = true;
                SurfaceDestroyed (holder);
            }
        }

        void ISurfaceHolderCallback.SurfaceCreated(ISurfaceHolder holder)
        {
            lock (androidViewLock) 
            {
                SurfaceCreated (holder);
                Paused = false;
            }
        }

        void ISurfaceHolderCallback.SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            lock (androidViewLock) 
            {
                SurfaceChanged (holder, format, width, height);
                ViewSize = new CCSizeI (width, height);
                viewportDirty = true;
            }
        }

        #endregion Rendering


        #region Touch handling

        void PlatformUpdateTouchEnabled()
        {
            SetOnTouchListener(touchEnabled ? this : null);
        }

        bool IOnTouchListener.OnTouch(View v, MotionEvent e)
        {
            if (!TouchEnabled || Paused)
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

        #endregion Touch handling
    }
}

