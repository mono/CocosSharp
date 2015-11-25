using System;
using System.Diagnostics;

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
		Released,
		Pressed,
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
        Unset = 0,
		One = 1,
		Two = 2,
		Three = 3,
		Four = 4,
	}

	/// <summary>
	/// Mapped from the gamepad game stick status, will tell yoyu when the game
	/// stick is down or up and the direction and magnitude of the stick movement
	/// in [u,v] coordinates.
	/// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
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

        internal string DebugDisplayString
        {
            get
            {
                return ToString ();
            }
        }

        public override string ToString ()
        {
            return string.Concat("IsDown: ", IsDown,
                " Direction: ", Direction,
                " Magnitude: ", Magnitude
            );
        }
	}

    /// <summary>
    /// Mapped from the gamepad game trigger status, will tell you when the game
    /// trigger is down or up and the magnitude of the stick movement
    /// </summary>
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct CCGameTriggerStatus
    {
        /// <summary>
        /// When true, the stick is down, otherwise it is up.
        /// </summary>
        public CCGamePadButtonStatus IsDown;
        /// <summary>
        /// The magnitude of the trigger movement, used to control soft or hard movements using
        /// the trigger.
        /// </summary>
        public float Magnitude;

        internal string DebugDisplayString
        {
            get
            {
                return ToString ();
            }
        }

        public override string ToString ()
        {
            return string.Concat("IsDown: ", IsDown,
                " Magnitude: ", Magnitude
            );
        }
    }

    [DebuggerDisplay("{DebugDisplayString,nq}")]
	public class CCEventGamePad : CCEvent
	{

		public CCGamePadEventType GamePadEventType { get; internal set; }

        internal TimeSpan TimeStamp { get; set; }
        public int Id { get; private set; }
        public CCPlayerIndex Player { get; internal set; }

        internal CCEventGamePad(CCGamePadEventType gamePadEventType, int id, TimeSpan timeStamp)
			: base (CCEventType.GAMEPAD)
		{
            Id = id;
            TimeStamp = timeStamp;
			GamePadEventType = gamePadEventType;
            Player = CCPlayerIndex.Unset;
		}

        internal string DebugDisplayString
        {
            get
            {
                return ToString ();
            }
        }

        public override string ToString()
        {
            return string.Concat("Player: ", 
                Player
            );
        }

	}
}

