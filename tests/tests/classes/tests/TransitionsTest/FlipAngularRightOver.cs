using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipAngularRightOver : CCTransitionFlipAngular
    {
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return CCTransitionFlipAngular.Create(t, s, tOrientation.kOrientationRightOver);
        }
    }
}
