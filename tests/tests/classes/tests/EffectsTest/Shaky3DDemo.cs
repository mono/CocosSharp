using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Shaky3DDemo : CCShaky3D
    {
        public static CCActionInterval actionWithDuration(float t)
        {
            return CCShaky3D.Create(5, true, new CCGridSize(15, 10), t);
        }
    }
}
