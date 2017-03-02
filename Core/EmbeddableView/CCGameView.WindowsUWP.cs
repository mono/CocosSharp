using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Microsoft.Xna.Framework.Graphics;
using Windows.Gaming.Input;


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

        private float ScaleFactor; //Variable to hold the device scale factor (use to determine phone screen resolution)

        #region Constructors

        public CCGameView() : base()
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
            ScaleFactor = CCDevice.ResolutionScaleFactor;
            currentPointerCursor = Window.Current.CoreWindow.PointerCursor;
            eventGamePadButton = new CCEventGamePadButton();
            eventGamePadDPad = new CCEventGamePadDPad();
        }

        void PlatformInitialiseGraphicsDevice(ref PresentationParameters presParams)
        {
#if WINDOWS_UWP
            presParams.SwapChainPanel = this;
#else
            presParams.SwapChainBackgroundPanel = this;
#endif
        }

        void PlatformStartGame()
        {
            CompositionTarget.Rendering += OnUpdateFrame;
            viewportDirty = true;
        }

        void InitialiseInputHandling()
        {
            InitialiseMobileInputHandling();
            InitialiseDesktopInputHandling();
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
            ViewSize = new CCSizeI((int)Math.Ceiling(e.NewSize.Width * ScaleFactor),
                (int)Math.Ceiling(e.NewSize.Height * ScaleFactor));

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
            ReadGamePadState();

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
            ProcessDesktopInput();
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
            var pos = new CCPoint((float)pointerPoint.Position.X * ScaleFactor, (float)pointerPoint.Position.Y * ScaleFactor);

            AddIncomingNewTouch((int)pointerPoint.PointerId, ref pos);

            args.Handled = true;
        }

        void TouchesMoved(object sender, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(this);
            var pos = new CCPoint((float)pointerPoint.Position.X * ScaleFactor, (float)pointerPoint.Position.Y * ScaleFactor);

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

        #region Mouse handling

        PointerEventHandler mousePressedHandler;
        PointerEventHandler mouseMovedHandler;
        PointerEventHandler mouseReleasedHandler;
        PointerEventHandler mouseWheelHandler;

        void PlatformUpdateMouseEnabled()
        {

            if (mousePressedHandler == null)
                mousePressedHandler = new PointerEventHandler(MousePressed);

            if (mouseMovedHandler == null)
                mouseMovedHandler = new PointerEventHandler(MouseMoved);

            if (mouseReleasedHandler == null)
                mouseReleasedHandler = new PointerEventHandler(MouseReleased);

            if (mouseWheelHandler == null)
                mouseWheelHandler = new PointerEventHandler(MouseWheelChanged);

            if (MouseEnabled)
            {
                AddHandler(UIElement.PointerPressedEvent, mousePressedHandler, true);
                AddHandler(UIElement.PointerMovedEvent, mouseMovedHandler, true);
                AddHandler(UIElement.PointerReleasedEvent, mouseReleasedHandler, true);
                AddHandler(UIElement.PointerWheelChangedEvent, mouseWheelHandler, true);
            }
            else
            {
                RemoveHandler(UIElement.PointerPressedEvent, mousePressedHandler);
                RemoveHandler(UIElement.PointerMovedEvent, mouseMovedHandler);
                RemoveHandler(UIElement.PointerReleasedEvent, mouseReleasedHandler);
                RemoveHandler(UIElement.PointerWheelChangedEvent, mouseWheelHandler);
            }
        }

        void PlatformUpdateMouseVisible()
        {
            Window.Current.CoreWindow.PointerCursor = mouseVisible ? currentPointerCursor : null;
        }

        CCMouseButton buttons = CCMouseButton.None;
        GamepadButtons previousGamepadButtons;
        CoreCursor currentPointerCursor;
        CCEventGamePadButton eventGamePadButton;
        CCEventGamePadDPad eventGamePadDPad;

        void MousePressed(object sender, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(this);
            if (pointerPoint.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {

                var pos = new CCPoint((float)pointerPoint.Position.X * ScaleFactor, (float)pointerPoint.Position.Y * ScaleFactor);

                var state = pointerPoint.Properties;

                buttons = CCMouseButton.None;
                buttons |= state.IsLeftButtonPressed ? CCMouseButton.LeftButton : CCMouseButton.None;
                buttons |= state.IsRightButtonPressed ? CCMouseButton.RightButton : CCMouseButton.None;
                buttons |= state.IsMiddleButtonPressed ? CCMouseButton.MiddleButton : CCMouseButton.None;

                AddIncomingMouse((int)pointerPoint.PointerId, ref pos, buttons);
                args.Handled = true;
            }
        }

        void MouseMoved(object sender, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(this);
            if (pointerPoint.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {

                var pos = new CCPoint((float)pointerPoint.Position.X * ScaleFactor, (float)pointerPoint.Position.Y * ScaleFactor);

                var state = pointerPoint.Properties;

                if (mouseMap.ContainsKey((int)pointerPoint.PointerId))
                    UpdateIncomingMouse((int)pointerPoint.PointerId, ref pos, buttons);
                else
                {
                    buttons = CCMouseButton.None;
                    AddIncomingMouse((int)pointerPoint.PointerId, ref pos);
                }

                args.Handled = true;
            }
        }

        void MouseReleased(object sender, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(this);
            if (pointerPoint.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {

                var pos = new CCPoint((float)pointerPoint.Position.X * ScaleFactor, (float)pointerPoint.Position.Y * ScaleFactor);

                var state = pointerPoint.Properties;

                var buttonsReleased = CCMouseButton.None;
                if (buttons.HasFlag(CCMouseButton.LeftButton) && !state.IsLeftButtonPressed)
                    buttonsReleased = CCMouseButton.LeftButton;
                if (buttons.HasFlag(CCMouseButton.RightButton) && !state.IsRightButtonPressed)
                    buttonsReleased |= CCMouseButton.RightButton;
                if (buttons.HasFlag(CCMouseButton.MiddleButton) && !state.IsMiddleButtonPressed)
                    buttonsReleased |= CCMouseButton.MiddleButton;
                UpdateIncomingReleaseMouse((int)pointerPoint.PointerId, buttonsReleased);

                args.Handled = true;
            }

        }

        void MouseWheelChanged(object sender, PointerRoutedEventArgs args)
        {
            var pointerPoint = args.GetCurrentPoint(this);
            if (pointerPoint.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {

                var pos = new CCPoint((float)pointerPoint.Position.X * ScaleFactor, (float)pointerPoint.Position.Y * ScaleFactor);

                var state = pointerPoint.Properties;

                AddIncomingScrollMouse((int)pointerPoint.PointerId, ref pos, state.MouseWheelDelta);

                args.Handled = true;
            }
        }
        #endregion Mouse handling

        #region Keyboard handling

        void PlatformUpdateKeyboardEnabled()
        {
            if (KeyboardEnabled)
            {
                Window.Current.CoreWindow.KeyDown += OnKeyDown;
                Window.Current.CoreWindow.KeyUp += OnKeyUp;
            }
            else
            {
                Window.Current.CoreWindow.KeyDown -= OnKeyDown;
                Window.Current.CoreWindow.KeyUp -= OnKeyUp;
            }
        }

        void OnKeyDown(object sender, KeyEventArgs args)
        {
            AddIncomingKeyDown((CCKeys)(int)args.VirtualKey);
            args.Handled = true;
        }

        void OnKeyUp(object sender, KeyEventArgs args)
        {
            AddIncomingKeyUp((CCKeys)(int)args.VirtualKey);
            args.Handled = true;
        }

        #endregion Keyboard handling

        #region Gamepad handling

        void ReadGamePadState()
        {
            if (Gamepad.Gamepads.Count > 0)
            {
                var gamepad = Gamepad.Gamepads[0];
                var gamepadReading = gamepad.GetCurrentReading();
                var gamepadButtons = gamepadReading.Buttons;

                var buttonStateChanged = false;
                eventGamePadButton.A = CCGamePadButtonStatus.None;
                eventGamePadButton.B = CCGamePadButtonStatus.None;
                eventGamePadButton.X = CCGamePadButtonStatus.None;
                eventGamePadButton.Y = CCGamePadButtonStatus.None;
                eventGamePadButton.LeftShoulder = CCGamePadButtonStatus.None;
                eventGamePadButton.RightShoulder = CCGamePadButtonStatus.None;
                eventGamePadButton.Start = CCGamePadButtonStatus.None;
                eventGamePadButton.Back = CCGamePadButtonStatus.None;
                if (previousGamepadButtons.HasFlag(GamepadButtons.A) != gamepadButtons.HasFlag(GamepadButtons.A))
                {
                    eventGamePadButton.A = gamepadButtons.HasFlag(GamepadButtons.A) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    buttonStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.B) != gamepadButtons.HasFlag(GamepadButtons.B))
                {
                    eventGamePadButton.B = gamepadButtons.HasFlag(GamepadButtons.B) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    buttonStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.X) != gamepadButtons.HasFlag(GamepadButtons.X))
                {
                    eventGamePadButton.X = gamepadButtons.HasFlag(GamepadButtons.X) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    buttonStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.Y) != gamepadButtons.HasFlag(GamepadButtons.Y))
                {
                    eventGamePadButton.Y = gamepadButtons.HasFlag(GamepadButtons.Y) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    buttonStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.LeftShoulder) != gamepadButtons.HasFlag(GamepadButtons.LeftShoulder))
                {
                    eventGamePadButton.LeftShoulder = gamepadButtons.HasFlag(GamepadButtons.LeftShoulder) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    buttonStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.RightShoulder) != gamepadButtons.HasFlag(GamepadButtons.RightShoulder))
                {
                    eventGamePadButton.RightShoulder = gamepadButtons.HasFlag(GamepadButtons.RightShoulder) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    buttonStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.Menu) != gamepadButtons.HasFlag(GamepadButtons.Menu))
                {
                    eventGamePadButton.Start = gamepadButtons.HasFlag(GamepadButtons.Menu) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    buttonStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.View) != gamepadButtons.HasFlag(GamepadButtons.View))
                {
                    eventGamePadButton.Back = gamepadButtons.HasFlag(GamepadButtons.View) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    buttonStateChanged = true;
                }
                if (buttonStateChanged)
                {
                    AddIncomingGamePadButton(eventGamePadButton);
                }

                var dPadStateChanged = false;
                eventGamePadDPad.Left = CCGamePadButtonStatus.None;
                eventGamePadDPad.Right = CCGamePadButtonStatus.None;
                eventGamePadDPad.Up = CCGamePadButtonStatus.None;
                eventGamePadDPad.Down = CCGamePadButtonStatus.None;
                if (previousGamepadButtons.HasFlag(GamepadButtons.DPadLeft) != gamepadButtons.HasFlag(GamepadButtons.DPadLeft))
                {
                    eventGamePadDPad.Left = gamepadButtons.HasFlag(GamepadButtons.DPadLeft) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    dPadStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.DPadRight) != gamepadButtons.HasFlag(GamepadButtons.DPadRight))
                {
                    eventGamePadDPad.Right = gamepadButtons.HasFlag(GamepadButtons.DPadRight) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    dPadStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.DPadUp) != gamepadButtons.HasFlag(GamepadButtons.DPadUp))
                {
                    eventGamePadDPad.Up = gamepadButtons.HasFlag(GamepadButtons.DPadUp) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    dPadStateChanged = true;
                }
                if (previousGamepadButtons.HasFlag(GamepadButtons.DPadDown) != gamepadButtons.HasFlag(GamepadButtons.DPadDown))
                {
                    eventGamePadDPad.Down = gamepadButtons.HasFlag(GamepadButtons.DPadDown) ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
                    dPadStateChanged = true;
                }
                if (dPadStateChanged)
                {
                    AddIncomingGamePadDPad(eventGamePadDPad);
                }

                previousGamepadButtons = gamepadButtons;
            }
        }

        #endregion Gamepad
    }
}