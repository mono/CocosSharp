using System;
using Microsoft.Xna.Framework.Input;

namespace CocosSharp
{

	public class CCEventAccelerate : CCEvent
	{

		// Set the Acceleration data 
		public CCAcceleration Acceleration { get; internal set; }

		internal CCEventAccelerate()
			: base (CCEventType.ACCELERATION)
		{	}
	}
}
