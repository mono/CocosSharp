using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class FlipYUpOver : CCTransitionFlipY
    {
        public FlipYUpOver(float t, CCScene s) : base(t, s, CCTransitionOrientation.UpOver)
        {
        }

    }
}
