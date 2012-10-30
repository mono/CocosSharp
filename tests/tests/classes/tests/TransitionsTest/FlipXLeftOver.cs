using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipXLeftOver : CCTransitionFlipX
    {
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return CCTransitionFlipX.Create(t, s, tOrientation.kOrientationLeftOver);
        }
    }
}
