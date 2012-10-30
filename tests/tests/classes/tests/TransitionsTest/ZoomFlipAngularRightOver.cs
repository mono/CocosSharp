using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZoomFlipAngularRightOver : CCTransitionZoomFlipAngular
    {
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return CCTransitionZoomFlipAngular.Create(t, s, tOrientation.kOrientationRightOver);
        }
    }
}
