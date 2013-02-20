using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FlipAngularLeftOver : CCTransitionFlipAngular
    {
        public FlipAngularLeftOver(float t, CCScene s) : base(t, s, tOrientation.kOrientationLeftOver)
        {
        }
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return (new FlipAngularLeftOver(t, s));
//            return CCTransitionFlipAngular.Create(t, s, tOrientation.kOrientationLeftOver);
        }
    }
}
