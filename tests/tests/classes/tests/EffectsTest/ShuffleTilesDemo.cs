using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class ShuffleTilesDemo
    {
		public static CCActionInterval ActionWithDuration(float t)
        {
            var shuffle = new CCShuffleTiles(new CCGridSize(16, 12), t, 25);
            var shuffle_back = shuffle.Reverse();
            var delay = new CCDelayTime (2);

			return new CCSequence(shuffle, delay, shuffle_back);
        }

    }
}
