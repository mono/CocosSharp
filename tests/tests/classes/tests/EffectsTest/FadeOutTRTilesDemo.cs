using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class FadeOutTRTilesDemo : CCFadeOutTRTiles
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCFadeOutTRTiles fadeout = new CCFadeOutTRTiles(t, new CCGridSize(16, 12));
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = new CCDelayTime (0.5f);

            return (CCActionInterval)(new CCSequence(fadeout, delay, back));
        }
    }
}
