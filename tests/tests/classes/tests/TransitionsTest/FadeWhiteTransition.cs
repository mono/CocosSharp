using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FadeWhiteTransition : CCTransitionFade
    {
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            return CCTransitionFade.Create(t, s, ccTypes.ccWHITE);
        }
    }
}
