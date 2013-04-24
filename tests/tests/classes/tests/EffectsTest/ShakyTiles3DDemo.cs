using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ShakyTiles3DDemo : CCShakyTiles3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            return new CCShakyTiles3D(5, true, new CCGridSize(16, 12), t);
        }
    }
}
