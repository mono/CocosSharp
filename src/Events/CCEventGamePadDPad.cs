using System;
using System.Diagnostics;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
	public class CCEventGamePadDPad : CCEventGamePad
	{
		public CCGamePadButtonStatus Left { get; internal set; }
		public CCGamePadButtonStatus Up { get; internal set; }
		public CCGamePadButtonStatus Right { get; internal set; }
		public CCGamePadButtonStatus Down { get; internal set; }
        /// <summary>
        /// The magnitude of the dpad movement, used to control soft or hard movements using
        /// the dpad.
        /// </summary>
        public float Magnitude { get; internal set; }

        internal CCEventGamePadDPad(int id, TimeSpan timeStamp)
            : base(CCGamePadEventType.GAMEPAD_DPAD, id, timeStamp)
		{ }

        public override string ToString()
        {
            return string.Concat("Left: ", Left, 
                " Up: ", Up,
                " Right: ", Right,
                " Down: ", Down,
                " Magnitude: ", Magnitude
            );
        }
	}
}
