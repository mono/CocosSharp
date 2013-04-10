using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class FadeWhiteTransition : CCTransitionFade
    {
        public FadeWhiteTransition (float t, CCScene s) : base (t, s, CCTypes.CCWhite)
        {  }

    }
}
