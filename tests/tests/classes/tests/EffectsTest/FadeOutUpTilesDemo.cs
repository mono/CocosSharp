using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class FadeOutUpTilesDemo : CCFadeOutUpTiles
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            CCFadeOutUpTiles fadeout = new CCFadeOutUpTiles(t, new CCGridSize(16, 12));
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = new CCDelayTime (0.5f);

            return (CCActionInterval)(new CCSequence(fadeout, delay, back));
        }
    }
}
