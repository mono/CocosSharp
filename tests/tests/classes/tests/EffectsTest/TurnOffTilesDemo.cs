using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TurnOffTilesDemo : CCTurnOffTiles
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCTurnOffTiles fadeout = new CCTurnOffTiles(t, new CCGridSize(48, 32), 25);
            CCFiniteTimeAction back = fadeout.Reverse();
            CCDelayTime delay = new CCDelayTime (0.5f);

            return (CCActionInterval)(new CCSequence(fadeout, delay, back));
        }
    }
}
