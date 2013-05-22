using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class FlipYDownOver : CCTransitionFlipY
    {
        public FlipYDownOver(float t, CCScene s)
            : base(t, s, CCTransitionOrientation.DownOver)
        {
        }
    }
}
