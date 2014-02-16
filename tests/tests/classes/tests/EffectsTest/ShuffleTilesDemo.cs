using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class ShuffleTilesDemo : CCSequence
    {
        public static ShuffleTilesDemo CreateWithDuration(float t)
        {
            var shuffle = new CCShuffleTiles(new CCGridSize(16, 12), t, 25);
            var shuffle_back = shuffle.Reverse();
            var delay = new CCDelayTime (2);
            var actions = new CCFiniteTimeAction[]{shuffle, delay, shuffle_back};

            return new ShuffleTilesDemo(actions);
        }

        public ShuffleTilesDemo(params CCFiniteTimeAction[] actions) : base(actions) 
        {
        }
    }
}
