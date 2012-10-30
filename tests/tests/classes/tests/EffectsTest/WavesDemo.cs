using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class WavesDemo : CCWaves
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            return CCWaves.Create(4, 20, true, true, new ccGridSize(16, 12), t);
        }
    }
}
