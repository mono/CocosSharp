using System;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{

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
		public CCPlayerIndex Player { get; internal set; }

		internal CCEventGamePadButton()
			: base(CCGamePadEventType.GAMEPAD_BUTTON)
		{ }
	}
}
