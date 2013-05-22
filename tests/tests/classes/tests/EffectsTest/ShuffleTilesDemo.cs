using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class ShuffleTilesDemo : CCShuffleTiles
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCShuffleTiles shuffle = new CCShuffleTiles(25, new CCGridSize(16, 12), t);
            CCFiniteTimeAction shuffle_back = shuffle.Reverse();
            CCDelayTime delay = new CCDelayTime (2);

            return (CCActionInterval)(CCSequence.FromActions(shuffle, delay, shuffle_back));
        }
    }
}
