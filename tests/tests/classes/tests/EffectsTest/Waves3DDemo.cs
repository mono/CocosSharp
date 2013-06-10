using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class Waves3DDemo : CCWaves3D
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            return new CCWaves3D(t, new CCGridSize(15, 10), 5, 40);
        }
    }
}
