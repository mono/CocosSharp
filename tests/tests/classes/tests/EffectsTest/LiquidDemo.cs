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
            return new CCLiquid (4, 20, new CCGridSize(16, 12), t);
        }
    }
}
