using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using GameController;

namespace CocosSharp
{

    internal class GamePadControllerHandler
    {
        public int Id { get; set; }
        protected GCController controller { get; set; }
        public Action<CCEventGamePad> ButtonPressedHandler { get; set; }
        public Action<CCEventGamePad> ButtonReleasedHandler { get; set; }
        public Action<CCEventGamePad> DispatchGamePadEventHandler { get; set; }

        public GamePadControllerHandler(GCController controller)
        {
            this.controller = controller;
        }

        protected void DpadUp(GCControllerButtonInput dpadButton, float buttonValue, bool pressed )
        {
            var id = dpadButton.Handle.ToInt32 ();
            var dpadEvent = new CCEventGamePadDPad (id, new TimeSpan());
            dpadEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            dpadEvent.Up = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            DispatchGamePadEventHandler (dpadEvent);
        }

        protected void DpadDown(GCControllerButtonInput dpadButton, float buttonValue, bool pressed )
        {
            var id = dpadButton.Handle.ToInt32 ();
            var dpadEvent = new CCEventGamePadDPad (id, new TimeSpan());
            dpadEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            dpadEvent.Down = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            DispatchGamePadEventHandler (dpadEvent);
        }

        protected void DpadLeft(GCControllerButtonInput dpadButton, float buttonValue, bool pressed )
        {
            var id = dpadButton.Handle.ToInt32 ();
            var dpadEvent = new CCEventGamePadDPad (id, new TimeSpan());
            dpadEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            dpadEvent.Left = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            DispatchGamePadEventHandler (dpadEvent);
        }

        protected void DpadRight(GCControllerButtonInput dpadButton, float buttonValue, bool pressed )
        {
            var id = dpadButton.Handle.ToInt32 ();
            var dpadEvent = new CCEventGamePadDPad (id, new TimeSpan());
            dpadEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            dpadEvent.Right = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            DispatchGamePadEventHandler (dpadEvent);

        }

        protected void DPadValueChangeHandler(GCControllerDirectionPad dpad, float xValue, float yValue)
        {

            var id = dpad.Handle.ToInt32 ();

            var gpEvent = new CCEventGamePadStick (id, new TimeSpan());
            gpEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            var direction = CCPoint.Zero;
            direction.X = xValue;
            direction.Y = yValue;

            // tvos game pad distiguishes from left and right axis
            if (dpad.Left.IsPressed) {
                var left = gpEvent.Left;
                left.IsDown = true;
                left.Direction = direction;
                left.Magnitude = dpad.Left.Value;
                gpEvent.Left = left;
                // We will dispatch this event immediately
                DispatchGamePadEventHandler (gpEvent);
            } 
            if (dpad.Right.IsPressed) {
                var right = gpEvent.Right;
                right.IsDown = true;
                right.Direction = direction;
                right.Magnitude = dpad.Right.Value;
                gpEvent.Right = right;
                // We will dispatch this event immediately
                DispatchGamePadEventHandler (gpEvent);
            } 

        }

        protected void ButtonA(GCControllerButtonInput button, float buttonValue, bool pressed)
        {
            var buttonEvent = new CCEventGamePadButton (button.Handle.ToInt32(), new TimeSpan());
            buttonEvent.A = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);
        }

        protected void ButtonX(GCControllerButtonInput button, float buttonValue, bool pressed)
        {
            var buttonEvent = new CCEventGamePadButton (button.Handle.ToInt32(), new TimeSpan());
            buttonEvent.X = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);
        }

        protected void ButtonB(GCControllerButtonInput button, float buttonValue, bool pressed)
        {
            var buttonEvent = new CCEventGamePadButton (button.Handle.ToInt32(), new TimeSpan());
            buttonEvent.B = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);
        }

        protected void ButtonY(GCControllerButtonInput button, float buttonValue, bool pressed)
        {
            var buttonEvent = new CCEventGamePadButton (button.Handle.ToInt32(), new TimeSpan());
            buttonEvent.Y = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);
        }

        protected void LeftStick(GCControllerDirectionPad directionPad, float xValue, float yValue)
        {
            var id = directionPad.Handle.ToInt32 ();
            var buttonEvent = new CCEventGamePadStick (id, new TimeSpan());
            var left = buttonEvent.Left;
            left.Direction.X = xValue;
            left.Direction.Y = yValue;

            buttonEvent.Left = left;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);
        }

        protected void LeftShoulder(GCControllerButtonInput button, float buttonValue, bool pressed)
        {
            var buttonEvent = new CCEventGamePadButton (button.Handle.ToInt32(), new TimeSpan());
            buttonEvent.LeftShoulder = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);           
        }

        protected void RightShoulder(GCControllerButtonInput button, float buttonValue, bool pressed)
        {
            var buttonEvent = new CCEventGamePadButton (button.Handle.ToInt32(), new TimeSpan());
            buttonEvent.RightShoulder = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);           
        }

        protected void RightStick(GCControllerDirectionPad directionPad, float xValue, float yValue)
        {
            var id = directionPad.Handle.ToInt32 ();
            var buttonEvent = new CCEventGamePadStick (id, new TimeSpan());
            var right = buttonEvent.Right;
            right.Direction.X = xValue;
            right.Direction.Y = yValue;

            buttonEvent.Right = right;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);
        }

        protected void LeftTrigger(GCControllerButtonInput button, float buttonValue, bool pressed)
        {
            var buttonEvent = new CCEventGamePadTrigger (button.Handle.ToInt32(), new TimeSpan());
            var left = buttonEvent.Left;
            left.IsDown = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            left.Magnitude = buttonValue;
            buttonEvent.Left = left;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);           
        }

        protected void RightTrigger(GCControllerButtonInput button, float buttonValue, bool pressed)
        {
            var buttonEvent = new CCEventGamePadTrigger (button.Handle.ToInt32(), new TimeSpan());
            var right = buttonEvent.Right;
            right.IsDown = pressed ? CCGamePadButtonStatus.Pressed : CCGamePadButtonStatus.Released;
            right.Magnitude = buttonValue;
            buttonEvent.Right = right;
            buttonEvent.Player = CCGameView.ConvertToPlayerIndex (controller.PlayerIndex);
            DispatchGamePadEventHandler (buttonEvent);           
        }

    }

    internal class MicroGamePadControllerHandler : GamePadControllerHandler
    {
    
        GCMicroGamepad microGamePad;

        public MicroGamePadControllerHandler(GCController controller) : base (controller)
        {
            microGamePad = controller.MicroGamepad;
            Id = microGamePad.Handle.ToInt32 ();
            microGamePad.ButtonA.SetValueChangedHandler (ButtonA);
            microGamePad.ButtonX.SetValueChangedHandler (ButtonX);
            microGamePad.Dpad.ValueChangedHandler = DPadValueChangeHandler;
            microGamePad.Dpad.Up.SetValueChangedHandler (DpadUp);
            microGamePad.Dpad.Down.SetValueChangedHandler (DpadDown);
            microGamePad.Dpad.Left.SetValueChangedHandler (DpadLeft);
            microGamePad.Dpad.Right.SetValueChangedHandler (DpadRight);

            microGamePad.ValueChangedHandler = ValueElementHandler;

        }


        void ValueElementHandler(GCMicroGamepad microMe, GCControllerElement element)
        {
            CCLog.Log ("Value changed: " + element);
        }
    }

    internal class ExtendedGamePadControllerHandler : GamePadControllerHandler
    {

        GCExtendedGamepad extendedGamePad;

        public ExtendedGamePadControllerHandler(GCController controller) : base(controller)
        {
            extendedGamePad = controller.ExtendedGamepad;
            Id = extendedGamePad.Handle.ToInt32 ();
            extendedGamePad.ButtonA.SetValueChangedHandler (ButtonA);
            extendedGamePad.ButtonX.SetValueChangedHandler (ButtonX);
            extendedGamePad.ButtonB.SetValueChangedHandler (ButtonB);
            extendedGamePad.ButtonY.SetValueChangedHandler (ButtonY);
            extendedGamePad.DPad.ValueChangedHandler = DPadValueChangeHandler;
            extendedGamePad.DPad.Up.SetValueChangedHandler (DpadUp);
            extendedGamePad.DPad.Down.SetValueChangedHandler (DpadDown);
            extendedGamePad.DPad.Left.SetValueChangedHandler (DpadLeft);
            extendedGamePad.DPad.Right.SetValueChangedHandler (DpadRight);
            extendedGamePad.LeftShoulder.SetValueChangedHandler (LeftShoulder);
            extendedGamePad.RightShoulder.SetValueChangedHandler (RightShoulder);
            extendedGamePad.LeftThumbstick.ValueChangedHandler = LeftStick;
            extendedGamePad.RightThumbstick.ValueChangedHandler = RightStick;
            extendedGamePad.LeftTrigger.SetValueChangedHandler (LeftTrigger);
            extendedGamePad.RightTrigger.SetValueChangedHandler (RightTrigger);

            extendedGamePad.ValueChangedHandler = ValueElementHandler;

            controller.PlayerIndex = GCControllerPlayerIndex.Index2;
        }

        void ValueElementHandler(GCExtendedGamepad microMe, GCControllerElement element)
        {
            CCLog.Log ("Value changed: " + element);
        }
    }

    public partial class CCGameView
    {


        Dictionary<int, GCController> controllerMap;  // may be used in the future when actual equipment and controllers are attached.

        #region GamePad connection handling

        internal static CCPlayerIndex ConvertToPlayerIndex(GCControllerPlayerIndex playerIndex)
        {
            switch (playerIndex)
            {
            case GCControllerPlayerIndex.Index1:
                return CCPlayerIndex.One;
            case GCControllerPlayerIndex.Index2:
                return CCPlayerIndex.Two;
            case GCControllerPlayerIndex.Index3:
                return CCPlayerIndex.Three;
            case GCControllerPlayerIndex.Index4:
                return CCPlayerIndex.Four;
            default:
                return CCPlayerIndex.Unset;
            }
        }

        void UpdateGameControllers(GCController gameController)
        {
            var controllers = GCController.Controllers;

            foreach (var controller in GCController.Controllers) 
            {
                // AttachedToDevice means physically attached or not via cable.  A bluetooth controller is not attached.
                if (controller.ExtendedGamepad != null) 
                {
                    CCLog.Log ("Extended Gamepad connected: " + controller.PlayerIndex + " Physically Attached: " + controller.AttachedToDevice + " id: " + controller.ExtendedGamepad.Handle.ToInt32());
                    if (controller.ExtendedGamepad != null) 
                    {
                        var handler = new ExtendedGamePadControllerHandler (controller);
                        handler.ButtonPressedHandler = ButtonPressed;
                        handler.ButtonReleasedHandler = ButtonReleased;
                        handler.DispatchGamePadEventHandler = DispatchGamePadEvent;

                        AddIncomingConnection (handler.Id, true, ConvertToPlayerIndex (controller.PlayerIndex));
                    }
                }
                else if (controller.Gamepad != null) 
                {
                    CCLog.Log ("Normal Gamepad connected: Physically Attached: " + controller.AttachedToDevice + " id: " + controller.Gamepad.Handle.ToInt32());
                }
                else if (controller.MicroGamepad != null) {

                    CCLog.Log ("Micro Gamepad connected: " + controller.PlayerIndex + " Physically Attached: " + controller.AttachedToDevice + " id: " + controller.MicroGamepad.Handle.ToInt32());

                    if (controller.MicroGamepad != null) 
                    {

                        var handler = new MicroGamePadControllerHandler (controller);
                        handler.ButtonPressedHandler = ButtonPressed;
                        handler.ButtonReleasedHandler = ButtonReleased;
                        handler.DispatchGamePadEventHandler = DispatchGamePadEvent;

                        AddIncomingConnection (handler.Id, true, ConvertToPlayerIndex (controller.PlayerIndex));
                    }
                }
            }

        }

        void GameControllerConnect(NSNotification note)
        {

            var gcController = note.Object as GCController;

            if (gcController != null) 
            {
                UpdateGameControllers (gcController);   
            }
        }

        void GameControllerDisconnect(NSNotification note)
        {

            var gcController = note.Object as GCController;

            foreach (var controller in GCController.Controllers)
            {
                if (controller.VendorName == "Remote")
                {
                }
            }

        }

        #endregion

        #region GamePad handling

        NSObject DidConnectObserver;
        NSObject DidDisconnectObserver;

        void PlatformUpdateGamePadEnabled()
        {
            if (GamePadEnabled)
            {
                controllerMap = new Dictionary<int, GCController> ();

                GCController.StartWirelessControllerDiscovery (() => {

                    CCLog.Log ("Wireless Controller Discover finished");
                }
                );
                DidConnectObserver = NSNotificationCenter.DefaultCenter.AddObserver (GCController.DidConnectNotification, 

                    GameControllerConnect
                );  

                DidDisconnectObserver = NSNotificationCenter.DefaultCenter.AddObserver (GCController.DidDisconnectNotification, 

                    GameControllerDisconnect
                );  


            }
            else
            {
                if (DidConnectObserver != null)
                    NSNotificationCenter.DefaultCenter.RemoveObserver (DidConnectObserver);

                if (DidDisconnectObserver != null)
                    NSNotificationCenter.DefaultCenter.RemoveObserver (DidDisconnectObserver);


                GCController.StopWirelessControllerDiscovery();

            }

        }

        // Not sure what to do with these right now.
        public override void PressesBegan (NSSet<UIPress> presses, UIPressesEvent evt)
        {

            FillPressesCollection (presses);
            //base.PressesBegan (presses, evt);

        }
//
//        public override void PressesChanged (NSSet<UIPress> presses, UIPressesEvent evt)
//        {
////            CCLog.Log ("Presses Changed: " + evt.Type);
////            foreach (UIPress press in presses)
////            {
////                CCLog.Log ("Press : " + press.Type);
////            }
////            base.PressesChanged (presses, evt);
//        }
//
        public override void PressesEnded (NSSet<UIPress> presses, UIPressesEvent evt)
        {
            FillPressesCollection (presses);
//            base.PressesEnded (presses, evt);
        }

        public override void PressesCancelled (NSSet<UIPress> presses, UIPressesEvent evt)
        {
            FillPressesCollection (presses);
//            base.PressesCancelled (presses, evt);
        }

        void ButtonPressed(CCEventGamePad gamePadEvent)
        {
            gamePadEvent.TimeStamp = gameTime.TotalGameTime;
            AddIncomingNewPress (gamePadEvent.Id, gamePadEvent);
        }

        void ButtonReleased(CCEventGamePad gamePadEvent)
        {
            gamePadEvent.TimeStamp = gameTime.TotalGameTime;
            UpdateIncomingReleasePress (gamePadEvent.Id, gamePadEvent);
        }

        void DispatchGamePadEvent(CCEventGamePad gamePadEvent)
        {
            gamePadEvent.TimeStamp = gameTime.TotalGameTime;
            EventDispatcher.DispatchEvent (gamePadEvent);
        }

        // https://developer.apple.com/library/tvos/documentation/ServicesDiscovery/Conceptual/GameControllerPG/ControllingInputontvOS/ControllingInputontvOS.html#//apple_ref/doc/uid/TP40013276-CH7-DontLinkElementID_13
        void FillPressesCollection(NSSet<UIPress> presses)
        {
            if (presses.Count == 0)
                return;

            CCEventGamePad pressEvent = null;

            foreach (UIPress press in presses) 
            {
                var id = press.Handle.ToInt32();
                Console.WriteLine ("Pressed: " + press.ClassHandle.ToInt32 ());
                switch (press.Phase) 
                {
                case UIPressPhase.Changed:
                    //UpdateIncomingMoveTouch(id, ref position);                   
                    break;
                case UIPressPhase.Began:

                    switch(press.Type)
                    {
                    case UIPressType.Menu:
                        pressEvent = new CCEventGamePadButton (id, gameTime.TotalGameTime);
                        ((CCEventGamePadButton)pressEvent).Back = CCGamePadButtonStatus.Pressed;
                        break;
                    }

                    if (pressEvent != null)
                        AddIncomingNewPress (id, pressEvent);
                    break;
                case UIPressPhase.Ended:
                case UIPressPhase.Cancelled:

                    if (gamePadMap.TryGetValue (id, out pressEvent)) {
                        switch (press.Type) 
                        {
                        case UIPressType.Menu:
                            pressEvent = new CCEventGamePadButton (id, gameTime.TotalGameTime);
                            ((CCEventGamePadButton)pressEvent).Back = CCGamePadButtonStatus.Released;
                            break;

                        }
                        UpdateIncomingReleasePress (id, pressEvent);
                    }
                    break;
                default:
                    break;
                }
            }
        }

        #endregion GamePad handling
    }
}

