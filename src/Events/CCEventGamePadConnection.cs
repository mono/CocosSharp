using System;

namespace CocosSharp
{
	public class CCEventGamePadConnection : CCEventGamePad
	{

		public bool IsConnected { get; internal set; }

        internal CCEventGamePadConnection(int id, TimeSpan timeStamp)
            : base(CCGamePadEventType.GAMEPAD_CONNECTION, id, timeStamp)
		{ }
	}
}

