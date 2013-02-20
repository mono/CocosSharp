using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipYDownOver : CCTransitionFlipY
    {
        public FlipYDownOver(float t, CCScene s)
            : base(t, s, tOrientation.kOrientationDownOver)
        {
        }
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return (new FlipYDownOver(t, s));
//            return CCTransitionFlipY.Create(t, s, tOrientation.kOrientationDownOver);
        }
    }
}
