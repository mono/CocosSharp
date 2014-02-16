using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class FadeOutBLTilesDemo : CCSequence
    {
        public static FadeOutBLTilesDemo CreateWithDuration(float t)
        {
            var fadeout = new CCFadeOutBLTiles(t, new CCGridSize(16, 12));
            var back = fadeout.Reverse();
            var delay = new CCDelayTime (0.5f);
            var actions = new CCFiniteTimeAction[] {fadeout, delay, back};

            return new FadeOutBLTilesDemo(actions);
        }

        public FadeOutBLTilesDemo(params CCFiniteTimeAction[] actions) : base(actions)
        {
        }

    }
}
