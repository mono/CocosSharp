using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class ShatteredTiles3DDemo : CCShatteredTiles3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            return new CCShatteredTiles3D(t, new CCGridSize(16, 12), 5, true);
        }
    }
}
