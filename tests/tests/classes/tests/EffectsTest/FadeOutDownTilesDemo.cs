using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class FadeOutDownTilesDemo : CCSequence
    {
        public static FadeOutDownTilesDemo CreateWithDuration(float t)
        {
            var fadeout = new CCFadeOutDownTiles(t, new CCGridSize(16, 12));
            var back = fadeout.Reverse();
            var delay = new CCDelayTime (0.5f);
            var actions = new CCFiniteTimeAction[] {fadeout, delay, back};

            return new FadeOutDownTilesDemo(actions);
        }
            
        public FadeOutDownTilesDemo(params CCFiniteTimeAction[] actions) : base(actions)
        {
        }
    }
}
