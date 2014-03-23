using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{

	public class CCEventListenerGamePad : CCEventListener
	{

		public static string LISTENER_ID = "__cc_gamepad";

		/// <summary>
		/// The event delegate to handle game pad button state changes. This delegate handles all discrete button
		/// devices on the gamepad. See the CCGamePadTriggerDelegate and CCGamePadStickDelegate and CCGamePadDPadDelegate for the analog
		/// controls.
		/// </summary>
		/// <param name="backButton">State of the back button</param>
		/// <param name="startButton">State of the start button</param>
		/// <param name="systemButton">State of the system (Xbox, PS3, Ouya) button</param>
		/// <param name="aButton">State of the A (bottom) button</param>
		/// <param name="bButton">State of the B (right) button</param>
		/// <param name="xButton">State of the X (left) button</param>
		/// <param name="yButton">State of the Y (top) button</param>
		/// <param name="leftShoulder">State of the left shoulder button</param>
		/// <param name="rightShoulder">State of the right shoulder button</param>
		public Action<CCEventGamePadButton> OnButtonStatus { get; set; }


		// Event callback function for GamePad Player Connect/Disconnect events
		public Action<CCEventGamePadConnection> OnConnectionStatus { get; set; }

		/// <summary>
		/// Called with an update on the current D-Pad status. 
		/// </summary>
		/// <param name="leftButton">The left d-pad button status</param>
		/// <param name="upButton">The up d-pad button status</param>
		/// <param name="rightButton">The right d-pad button status</param>
		/// <param name="downButton">The down d-pad button status</param>
		public Action<CCEventGamePadDPad> OnDPadStatus { get; set; }

		/// <summary>
		/// each time the game pad status is queried, this method will get triggered.
		/// </summary>
		/// <param name="leftStick">The status of the left stick</param>
		/// <param name="rightStick">The status of the right stick</param>
		/// <param name="player">The player to which this pertains</param>
		public Action<CCEventGamePadStick> OnStickStatus { get; set; }

		/// <summary>
		/// Passes the left and right trigger depression value (strength) for the given player.
		/// </summary>
		/// <param name="leftTriggerStrength">Left trigger value</param>
		/// <param name="rightTriggerStrength">Right trigger value</param>
		/// <param name="player">The player to which it pertains</param>
		public Action<CCEventGamePadTrigger> OnTriggerStatus { get; set; }

		public override bool IsAvailable {
			get {
				return true;
			}
		}

		public CCEventListenerGamePad() : base(CCEventListenerType.GAMEPAD, LISTENER_ID)
		{
			// Set our call back action to be called on mouse events so they can be 
			// propagated to the listener.
			Action<CCEvent> listener = gpEvent =>
			{
				var gamePadEvent = (CCEventGamePad)gpEvent;
				switch (gamePadEvent.GamePadEventType)
				{
				case CCGamePadEventType.GAMEPAD_BUTTON:
					if (OnButtonStatus != null)
						OnButtonStatus((CCEventGamePadButton)gamePadEvent);
					break;
				case CCGamePadEventType.GAMEPAD_DPAD:
					if (OnDPadStatus != null)
						OnDPadStatus((CCEventGamePadDPad)gamePadEvent);
					break;
				case CCGamePadEventType.GAMEPAD_STICK:
					if (OnStickStatus != null)
						OnStickStatus((CCEventGamePadStick)gamePadEvent);
					break;
				case CCGamePadEventType.GAMEPAD_TRIGGER:
					if (OnTriggerStatus != null)
						OnTriggerStatus((CCEventGamePadTrigger)gamePadEvent);
					break;
				case CCGamePadEventType.GAMEPAD_CONNECTION:
					if (OnConnectionStatus != null)
						OnConnectionStatus((CCEventGamePadConnection)gamePadEvent);
					break;
				default:
					break;
				}

			};
			OnEvent = listener;
		}

		internal CCEventListenerGamePad(CCEventListenerGamePad gamePad)
			: this()
		{
			OnButtonStatus = gamePad.OnButtonStatus;
			OnConnectionStatus = gamePad.OnConnectionStatus;
			OnDPadStatus = gamePad.OnDPadStatus;
			OnStickStatus = gamePad.OnStickStatus;
			OnTriggerStatus = gamePad.OnTriggerStatus;
		}

		public override CCEventListener Copy()
		{
			return new CCEventListenerGamePad (this);
		}
	}
}
