using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class ZoomFlipAngularLeftOver : CCTransitionZoomFlipAngular
    {
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return CCTransitionZoomFlipAngular.Create(t, s, tOrientation.kOrientationLeftOver);
        }
    }
}
