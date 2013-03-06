using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TurnOffTilesDemo : CCTurnOffTiles
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCTurnOffTiles fadeout = CCTurnOffTiles.Create(25, new CCGridSize(48, 32), t);
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = CCDelayTime.Create(0.5f);

            return (CCActionInterval)(CCSequence.Create(fadeout, delay, back));
        }
    }
}
