using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipXRightOver : CCTransitionFlipX
    {
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return CCTransitionFlipX.Create(t, s, tOrientation.kOrientationRightOver);
        }
    }
}
