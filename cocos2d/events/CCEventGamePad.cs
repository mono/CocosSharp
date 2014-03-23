using System;

namespace CocosSharp
{

	public enum CCGamePadEventType
	{
		GAMEPAD_NONE,
		GAMEPAD_BUTTON,
		GAMEPAD_DPAD,
		GAMEPAD_STICK,
		GAMEPAD_TRIGGER,
		GAMEPAD_CONNECTION
	}

	/// <summary>
	/// How the button was engaged. You will get a Pressed notification unless the director
	/// is configuerd to consume button presses and create the Tapped button status.
	/// </summary>
	public enum CCGamePadButtonStatus
	{
		Pressed,
		Released,
		/// <summary>
		/// A pressed and released action was merged
		/// </summary>
		Tapped,
		/// <summary>
		/// Used when one of the buttons reported does not exist on the game pad
		/// </summary>
		NotApplicable
	}

	public enum CCPlayerIndex
	{
		One = 0,
		Two = 1,
		Three = 2,
		Four = 3
	}

	/// <summary>
	/// Mapped from the gamepad game stick status, will tell yoyu when the game
	/// stick is down or up and the direction and magnitude of the stick movement
	/// in [u,v] coordinates.
	/// </summary>
	public struct CCGameStickStatus
	{
		/// <summary>
		/// When true, the stick is down, otherwise it is up.
		/// </summary>
		public bool IsDown;
		/// <summary>
		/// The direction of the stick movement as a unit vector.
		/// </summary>
		public CCPoint Direction;
		/// <summary>
		/// The magnitude of the stick movement, used to control soft or hard movements using
		/// the stick.
		/// </summary>
		public float Magnitude;
	}

	public class CCEventGamePad : CCEvent
	{

		public CCGamePadEventType GamePadEventType { get; internal set; }

		internal CCEventGamePad(CCGamePadEventType gamePadEventType)
			: base (CCEventType.GAMEPAD)
		{
			GamePadEventType = gamePadEventType;
		}
	}
}

