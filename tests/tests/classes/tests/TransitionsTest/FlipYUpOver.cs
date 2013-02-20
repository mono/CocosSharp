using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipYUpOver : CCTransitionFlipY
    {
        public FlipYUpOver(float t, CCScene s) : base(t, s, tOrientation.kOrientationUpOver)
        {
        }

        [Obsolete("Use the parameter ctor instead.")]
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return (new FlipYUpOver(t, s));
//            return CCTransitionFlipY.Create(t, s, tOrientation.kOrientationUpOver);
        }
    }
}
