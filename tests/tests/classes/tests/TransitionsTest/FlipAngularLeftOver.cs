using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class FlipAngularLeftOver : CCTransitionFlipAngular
    {
        public FlipAngularLeftOver(float t, CCScene s) : base(t, s, CCTransitionOrientation.LeftOver)
        {
        }
    }
}
