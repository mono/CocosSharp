using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cocos2D;

namespace tests
{
    public class ZoomFlipYUpOver : CCTransitionZoomFlipY
    {
        public ZoomFlipYUpOver (float t, CCScene s) : base(t, s, CCTransitionOrientation.UpOver)
        { }

    }
}
