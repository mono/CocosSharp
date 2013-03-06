using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ShuffleTilesDemo : CCShuffleTiles
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCShuffleTiles shuffle = CCShuffleTiles.Create(25, new CCGridSize(16, 12), t);
            CCFiniteTimeAction shuffle_back = shuffle.Reverse();
            CCDelayTime delay = CCDelayTime.Create(2);

            return (CCActionInterval)(CCSequence.Create(shuffle, delay, shuffle_back));
        }
    }
}
