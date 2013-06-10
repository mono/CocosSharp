using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class LiquidDemo : CCLiquid
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            return new CCLiquid (t, new CCGridSize(16, 12), 4, 20);
        }
    }
}
