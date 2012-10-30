using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZoomFlipYDownOver : CCTransitionZoomFlipY
    {
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return CCTransitionZoomFlipY.Create(t, s, tOrientation.kOrientationDownOver);
        }
    }
}
