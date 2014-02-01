using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class FadeOutBLTilesDemo : CCSequence
    {
		public FadeOutBLTilesDemo(float t)
        {
			var fadeout = new CCFadeOutBLTiles(t, new CCGridSize(16, 12));
			var back = fadeout.Reverse();
			var delay = new CCDelayTime (0.5f);

			Actions = new CCFiniteTimeAction[] {fadeout, delay, back};
        }
    }
}
