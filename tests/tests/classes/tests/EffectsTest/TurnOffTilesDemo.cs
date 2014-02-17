using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class TurnOffTilesDemo
    {
		public static CCActionInterval ActionWithDuration(float t)
        {
            var fadeout = new CCTurnOffTiles(t, new CCGridSize(48, 32), 25);
            var back = fadeout.Reverse();
            var delay = new CCDelayTime (0.5f);

			return new CCSequence(fadeout, delay, back);
        }

    }
}
