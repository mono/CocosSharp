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
            CCFadeOutTRTiles fadeout = CCFadeOutTRTiles.Create(new ccGridSize(16, 12), t);
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = CCDelayTime.Create(0.5f);

            return (CCActionInterval)(CCSequence.Create(fadeout, delay, back));
        }
    }
}
