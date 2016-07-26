using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace CocosSharp
{
    public class GameSwapChainPanel : SwapChainPanel
    {
        protected virtual void Dispose(bool disposing)
        {
        }
    }

    public partial class CCGameView : GameSwapChainPanel
    {
        #region Constructors

        public CCGameView()
        {

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                return;
            Initialise();
        }

        #endregion Constructors


        #region Initialisation

        void PlatformInitialise()
        {
            SizeChanged += ViewSizeChanged;
            Window.Current.Activated += ViewStateChanged;
        }

        void PlatformInitialiseGraphicsDevice(ref PresentationParameters presParams)
        {
            presParams.SwapChainBackgroundPanel = this;
        }

        void PlatformStartGame()
        {
            CompositionTarget.Rendering += OnUpdateFrame;
            viewportDirty = true;
        }

        void InitialiseInputHandling()
        {
            InitialiseMobileInputHandling();
        }

        #endregion Initialisation


        #region Cleaning up

        void PlatformDispose(bool disposing)
        {
            if (this == null)
                return;

            Window.Current.Activated -= ViewStateChanged;
        }

        bool PlatformCanDisposeGraphicsDevice()
        {
            return true;
        }

        #endregion Cleaning up


        #region Rendering

        void ViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewSize = new CCSizeI((int)e.NewSize.Width, (int)e.NewSize.Height);
            
            // We need to ask the graphics device to resize the underlying swapchain/buffers
            graphicsDevice.PresentationParameters.BackBufferWidth = ViewSize.Width;
            graphicsDevice.PresentationParameters.BackBufferHeight = ViewSize.Height;
            graphicsDevice.CreateSizeDependentResources();
            graphicsDevice.ApplyRenderTargets(null);

            UpdateViewport();

            platformInitialised = true;

            LoadGame();
        }

        #endregion Rendering


        #region Run loop

        void PlatformUpdatePaused()
        {
            if (Paused)
                CompositionTarget.Rendering -= OnUpdateFrame;
            else
                CompositionTarget.Rendering += OnUpdateFrame;

            MobilePlatformUpdatePaused();
        }

        void ViewStateChanged(object sender, WindowActivatedEventArgs e)
        {
            Paused = (e.WindowActivationState == CoreWindowActivationState.Deactivated);
        }

        void OnUpdateFrame(object sender, object e)
        {
            Tick();

            graphicsDevice.ResetRenderTargets();

            Draw();

            PlatformPresent();
        }

        void PlatformPresent()
        {
            if (graphicsDevice != null)
                graphicsDevice.Present();
        }

        void ProcessInput()
        {
            ProcessMobileInput();
        }
        #endregion Run loop


        #region Touch handling

        void PlatformUpdateTouchEnabled()
        {
            if (TouchEnabled)
            {
                PointerPressed += TouchesBegan;
                PointerReleased += TouchesEnded;
                PointerCanceled += TouchesEnded;
                PointerMoved += TouchesMoved;
            }
            else
            {
                PointerPressed -= TouchesBegan;
                PointerReleased -= TouchesEnded;
                PointerCanceled -= TouchesEnded;
                PointerMoved -= TouchesMoved;
            }
        }

        void TouchesBegan(object sender, PointerRoutedEventArgs args)
        {
            ((UIElement)sender).CapturePointer(args.Pointer);

            var pointerPoint = args.GetCurrentPoint(this);
            var pos = new CCPoint((float)pointerPoint.Position.X, (float)pointerPoint.Position.Y);

            AddIncomingNewTouch((int)pointerPoint.PointerId, ref pos);

            args.Handled = true;
        }

        void TouchesMoved(object sender, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(this);
            var pos = new CCPoint((float)pointerPoint.Position.X, (float)pointerPoint.Position.Y);

            UpdateIncomingMoveTouch((int)pointerPoint.PointerId, ref pos);

            args.Handled = true;
        }

        void TouchesEnded(object sender, PointerRoutedEventArgs args)
        {
            ((UIElement)sender).ReleasePointerCapture(args.Pointer);

            var pointerPoint = args.GetCurrentPoint(this);

            UpdateIncomingReleaseTouch((int)pointerPoint.PointerId);

            args.Handled = true;
        }

        #endregion Touch handling
    }
}