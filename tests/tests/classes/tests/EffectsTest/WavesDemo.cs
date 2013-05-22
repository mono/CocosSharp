using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class WavesDemo : CCWaves
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            return new CCWaves(4, 20, true, true, new CCGridSize(16, 12), t);
        }
    }
}
