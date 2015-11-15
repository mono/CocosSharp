using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
	public class CCEventGamePadButton : CCEventGamePad
	{
		public CCGamePadButtonStatus Back { get; internal set; }
		public CCGamePadButtonStatus Start { get; internal set; } 
		public CCGamePadButtonStatus System { get; internal set; }
		public CCGamePadButtonStatus A { get; internal set; }
		public CCGamePadButtonStatus B { get; internal set; }
		public CCGamePadButtonStatus X { get; internal set; }
		public CCGamePadButtonStatus Y { get; internal set; }
		public CCGamePadButtonStatus LeftShoulder { get; internal set; }
		public CCGamePadButtonStatus RightShoulder { get; internal set; }

        internal CCEventGamePadButton(int id, TimeSpan timeStamp)
            : base(CCGamePadEventType.GAMEPAD_BUTTON, id, timeStamp)
		{ }

        public override string ToString()
        {
            return string.Concat("Back: ", Back, 
                " Start: ", Start,
                " System: ", System,
                " A: ", A,
                " B: ", B,
                " X: ", X,
                " Y: ", Y,
                " LeftShoulder: ", LeftShoulder,
                " RightShoulder: ", RightShoulder
            );
        }
	}
}
