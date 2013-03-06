using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Waves3DDemo : CCWaves3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            return CCWaves3D.Create(5, 40, new CCGridSize(15, 10), t);
        }
    }
}
