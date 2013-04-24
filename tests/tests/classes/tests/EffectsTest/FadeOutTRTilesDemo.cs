using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FadeOutTRTilesDemo : CCFadeOutTRTiles
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCFadeOutTRTiles fadeout = new CCFadeOutTRTiles(new CCGridSize(16, 12), t);
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = new CCDelayTime (0.5f);

            return (CCActionInterval)(CCSequence.FromActions(fadeout, delay, back));
        }
    }
}
