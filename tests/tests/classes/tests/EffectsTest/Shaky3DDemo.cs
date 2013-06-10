using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class Shaky3DDemo : CCShaky3D
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            return new CCShaky3D(t, new CCGridSize(15, 10), 5, true);
        }
    }
}
