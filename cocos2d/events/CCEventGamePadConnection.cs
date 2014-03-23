using System;

namespace CocosSharp
{
	public class CCEventGamePadConnection : CCEventGamePad
	{

		public bool IsConnected { get; internal set; }
		public CCPlayerIndex Player { get; internal set; }

		internal CCEventGamePadConnection()
			: base(CCGamePadEventType.GAMEPAD_CONNECTION)
		{ }
	}
}

