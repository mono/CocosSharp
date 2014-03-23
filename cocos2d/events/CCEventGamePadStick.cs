using System;

namespace CocosSharp
{
	public class CCEventGamePadStick : CCEventGamePad
	{
		public CCGameStickStatus Left { get; internal set; }
		public CCGameStickStatus Right { get; internal set; }

		public CCPlayerIndex Player { get; internal set; }

		internal CCEventGamePadStick()
			: base(CCGamePadEventType.GAMEPAD_STICK)
		{ }
	}
}
