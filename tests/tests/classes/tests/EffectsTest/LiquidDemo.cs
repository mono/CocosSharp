using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class LiquidDemo : CCLiquid
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            return CCLiquid.Create(4, 20, new ccGridSize(16, 12), t);
        }
    }
}
