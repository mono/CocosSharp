using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class FadeOutTRTilesDemo : CCSequence
    {
        public static FadeOutTRTilesDemo CreateWithDuration(float t)
        {
            CCFadeOutTRTiles fadeout = new CCFadeOutTRTiles(t, new CCGridSize(16, 12));
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = new CCDelayTime (0.5f);
            var actions = new CCFiniteTimeAction[] {fadeout, delay, back};

            return new FadeOutTRTilesDemo(actions);
        }

        public FadeOutTRTilesDemo(params CCFiniteTimeAction[] actions) : base(actions)
        {
        }
    }
}
