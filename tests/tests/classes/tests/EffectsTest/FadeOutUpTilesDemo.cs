using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class FadeOutUpTilesDemo
    {
		public static CCActionInterval ActionWithDuration(float t)
        {
            var fadeout = new CCFadeOutUpTiles(t, new CCGridSize(16, 12));
            var back = fadeout.Reverse();
            var delay = new CCDelayTime (0.5f);

			return new CCSequence(fadeout, delay, back);
        }

    }
}
