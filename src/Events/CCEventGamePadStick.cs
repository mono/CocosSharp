using System;
using System.Diagnostics;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
	public class CCEventGamePadStick : CCEventGamePad
	{
		public CCGameStickStatus Left { get; internal set; }
		public CCGameStickStatus Right { get; internal set; }

        internal CCEventGamePadStick(int id, TimeSpan timeStamp)
            : base(CCGamePadEventType.GAMEPAD_STICK, id, timeStamp)
		{ }

        public override string ToString()
        {
            return string.Concat("Left: ", Left, 
                " Right: ", Right
            );
        }
	}
}
