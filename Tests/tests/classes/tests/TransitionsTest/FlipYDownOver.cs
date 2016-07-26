using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

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
