using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class TurnOffTilesDemo : CCSequence
    {
        public static TurnOffTilesDemo CreateWithDuration(float t)
        {
            var fadeout = new CCTurnOffTiles(t, new CCGridSize(48, 32), 25);
            var back = fadeout.Reverse();
            var delay = new CCDelayTime (0.5f);
            var actions = new CCFiniteTimeAction[] {fadeout, delay, back};

            return new TurnOffTilesDemo(actions);
        }

        public TurnOffTilesDemo(params CCFiniteTimeAction[] actions) : base(actions)
        {
        }
    }
}
