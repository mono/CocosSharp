using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipAngularRightOver : CCTransitionFlipAngular
    {
        public FlipAngularRightOver(float t, CCScene s)
            : base(t, s, tOrientation.kOrientationRightOver)
        {
        }
        public static CCTransitionScene Create(float t, CCScene s)
        {
            return (new FlipAngularRightOver(t, s));
//            return CCTransitionFlipAngular.Create(t, s, tOrientation.kOrientationRightOver);
        }
    }
}
