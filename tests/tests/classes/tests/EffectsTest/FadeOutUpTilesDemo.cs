using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FadeOutUpTilesDemo : CCFadeOutUpTiles
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCFadeOutUpTiles fadeout = CCFadeOutUpTiles.Create(new CCGridSize(16, 12), t);
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = new CCDelayTime (0.5f);

            return (CCActionInterval)(CCSequence.FromActions(fadeout, delay, back));
        }
    }
}
