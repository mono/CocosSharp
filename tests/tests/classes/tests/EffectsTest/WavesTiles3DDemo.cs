using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class WavesTiles3DDemo : CCWavesTiles3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            return CCWavesTiles3D.Create(4, 120, new ccGridSize(15, 10), t);
        }
    }
}
