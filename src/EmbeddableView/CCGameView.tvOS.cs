using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using ObjCRuntime;
using CoreAnimation;
using GameController;

namespace CocosSharp
{
    public partial class CCGameView
    {


        Dictionary<int, GCController> controllerMap;  // may be used in the future when actual equipment and controllers are attached.
        GCController gameController;
        GCMicroGamepad microGamePad;

        #region GamePad connection handling


        CCPlayerIndex ConvertToPlayerIndex(GCControllerPlayerIndex playerIndex)
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
            this.gameController = gameController;

            foreach (var controller in GCController.Controllers) 
            {
                // AttachedToDevice means physically attached or not via cable.  A bluetooth controller is not attached.
                if (controller.ExtendedGamepad != null) 
                {
                    CCLog.Log ("Extended Gamepad connected: Physically Attached: " + gameController.AttachedToDevice + " id: " + controller.ExtendedGamepad.Handle.ToInt32());

                }
                else if (controller.Gamepad != null) 
                {
                    CCLog.Log ("Normal Gamepad connected: Physically Attached: " + gameController.AttachedToDevice + " id: " + controller.Gamepad.Handle.ToInt32());
                }
                else if (controller.MicroGamepad != null) {

                    CCLog.Log ("Micro Gamepad connected: Physically Attached: " + gameController.AttachedToDevice + " id: " + controller.MicroGamepad.Handle.ToInt32());

                    if (controller.MicroGamepad != null) 
                    {
                        
                        microGamePad = controller.MicroGamepad;
                        //microGamePad.ValueChangedHandler = MicroValueChangeHandler; // This does not seem reliable at all, instead we will use Presses overrides
                        microGamePad.Dpad.ValueChangedHandler = MicroDPadValueChangeHandler;
                        microGamePad.ReportsAbsoluteDpadValues = true;

                        var id = microGamePad.ClassHandle.ToInt32 ();

                        AddIncomingConnection (id, true, ConvertToPlayerIndex (controller.PlayerIndex));
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

        // This does not seem to be called right now may depend on the ViewController that
        // is controlling.  UIViewController or GCEventViewController.
//        void MicroValueChangeHandler(GCMicroGamepad gamepad, GCControllerElement element)
//        {
//            CCLog.Log ("Value Changed: " + element);// + " value: " +  + " pressed: " + pressed);
//
////            if (element == gamepad.ButtonA)
////            {
////                CCLog.Log ("button A pressed");
////            }
////
////            if (element == gamepad.ButtonX)
////            {
////                CCLog.Log ("button X pressed");
////            }
//
//        }

        void MicroDPadValueChangeHandler(GCControllerDirectionPad dpad, float xValue, float yValue)
        {

            var id = dpad.Handle.ToInt32 ();

            var gpEvent = new CCEventGamePadStick (id, gameTime.TotalGameTime);
            gpEvent.Player = ConvertToPlayerIndex (microGamePad.Controller.PlayerIndex);
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
                EventDispatcher.DispatchEvent (gpEvent);
            } 
            if (dpad.Right.IsPressed) {
                var right = gpEvent.Right;
                right.IsDown = true;
                right.Direction = direction;
                right.Magnitude = dpad.Right.Value;
                gpEvent.Right = right;
                // We will dispatch this event immediately
                EventDispatcher.DispatchEvent (gpEvent);
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
//            CCLog.Log ("Presses Began: " + evt.Type);
//            foreach (UIPress press in presses)
//            {
//                CCLog.Log ("Press : " + press.Type);
//            }
//            base.PressesBegan (presses, evt);

            FillPressesCollection (presses);

        }

        public override void PressesChanged (NSSet<UIPress> presses, UIPressesEvent evt)
        {
//            CCLog.Log ("Presses Changed: " + evt.Type);
//            foreach (UIPress press in presses)
//            {
//                CCLog.Log ("Press : " + press.Type);
//            }
//            base.PressesChanged (presses, evt);
        }

        public override void PressesEnded (NSSet<UIPress> presses, UIPressesEvent evt)
        {
            //CCLog.Log ("Presses Ended: " + evt.Type);
            //foreach (UIPress press in presses)
            //{
            //    CCLog.Log ("Press : " + press.Type);
            //}
            FillPressesCollection (presses);
//            base.PressesEnded (presses, evt);
        }

        public override void PressesCancelled (NSSet<UIPress> presses, UIPressesEvent evt)
        {
//            CCLog.Log ("Presses Cancelled: " + evt.Type);
//            foreach (UIPress press in presses)
//            {
//                CCLog.Log ("Press : " + press.Type);
//            }
            FillPressesCollection (presses);
//            base.PressesCancelled (presses, evt);
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

                switch (press.Phase) 
                {
                case UIPressPhase.Changed:
                    //UpdateIncomingMoveTouch(id, ref position);                   
                    break;
                case UIPressPhase.Began:

                    switch(press.Type)
                    {
                    case UIPressType.LeftArrow:
                        pressEvent = new CCEventGamePadDPad (id, gameTime.TotalGameTime);
                        ((CCEventGamePadDPad)pressEvent).Left = CCGamePadButtonStatus.Pressed;
                        break;
                    case UIPressType.UpArrow:
                        pressEvent = new CCEventGamePadDPad (id, gameTime.TotalGameTime);
                        ((CCEventGamePadDPad)pressEvent).Up = CCGamePadButtonStatus.Pressed;
                        break;
                    case UIPressType.RightArrow:
                        pressEvent = new CCEventGamePadDPad (id, gameTime.TotalGameTime);
                        ((CCEventGamePadDPad)pressEvent).Right = CCGamePadButtonStatus.Pressed;
                        break;
                    case UIPressType.DownArrow:
                        pressEvent = new CCEventGamePadDPad (id, gameTime.TotalGameTime);
                        ((CCEventGamePadDPad)pressEvent).Down = CCGamePadButtonStatus.Pressed;
                        break;
                    case UIPressType.Select:
                        pressEvent = new CCEventGamePadButton (id, gameTime.TotalGameTime);
                        ((CCEventGamePadButton)pressEvent).A = CCGamePadButtonStatus.Pressed;
                        break;
                    case UIPressType.PlayPause:
                        pressEvent = new CCEventGamePadButton (id, gameTime.TotalGameTime);
                        ((CCEventGamePadButton)pressEvent).X = CCGamePadButtonStatus.Pressed;
                        break;
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
                        switch (press.Type) {
                        case UIPressType.LeftArrow:
                            pressEvent = new CCEventGamePadDPad (id, gameTime.TotalGameTime);
                            ((CCEventGamePadDPad)pressEvent).Left = CCGamePadButtonStatus.Released;
                            break;
                        case UIPressType.UpArrow:
                            pressEvent = new CCEventGamePadDPad (id, gameTime.TotalGameTime);
                            ((CCEventGamePadDPad)pressEvent).Up = CCGamePadButtonStatus.Released;
                            break;
                        case UIPressType.RightArrow:
                            pressEvent = new CCEventGamePadDPad (id, gameTime.TotalGameTime);
                            ((CCEventGamePadDPad)pressEvent).Right = CCGamePadButtonStatus.Released;
                            break;
                        case UIPressType.DownArrow:
                            pressEvent = new CCEventGamePadDPad (id, gameTime.TotalGameTime);
                            ((CCEventGamePadDPad)pressEvent).Down = CCGamePadButtonStatus.Released;
                            break;
                        case UIPressType.Select:
                            pressEvent = new CCEventGamePadButton (id, gameTime.TotalGameTime);
                            ((CCEventGamePadButton)pressEvent).A = CCGamePadButtonStatus.Released;
                            break;
                        case UIPressType.PlayPause:
                            pressEvent = new CCEventGamePadButton (id, gameTime.TotalGameTime);
                            ((CCEventGamePadButton)pressEvent).X = CCGamePadButtonStatus.Released;
                            break;
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

