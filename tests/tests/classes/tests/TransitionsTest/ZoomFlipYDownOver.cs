using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ZoomFlipYDownOver : CCTransitionZoomFlipY
    {

        public ZoomFlipYDownOver (float t, CCScene s) : base(t, s, CCTransitionOrientation.DownOver)
        { }

    }
}
