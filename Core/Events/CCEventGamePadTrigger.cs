using System;

namespace CocosSharp
{
	public class CCEventGamePadTrigger : CCEventGamePad
	{
		public float Left { get; internal set; }
		public float Right { get; internal set; }

		public CCPlayerIndex Player { get; internal set; }

		internal CCEventGamePadTrigger()
			: base(CCGamePadEventType.GAMEPAD_TRIGGER)
		{ }
	}
}
