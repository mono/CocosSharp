using System;
using System.Diagnostics;

namespace CocosSharp
{
	public class CCEventGamePadTrigger : CCEventGamePad
	{
		public float Left { get; internal set; }
		public float Right { get; internal set; }

        internal CCEventGamePadTrigger(int id, TimeSpan timeStamp)
            : base(CCGamePadEventType.GAMEPAD_TRIGGER, id, timeStamp)
		{ }

        public override string ToString()
        {
            return string.Concat("Left: ", Left, 
                " Right: ", Right
            );
        }
	}
}
