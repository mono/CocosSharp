using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PageTransitionForward : CCTransitionPageTurn
    {
        public PageTransitionForward (float t, CCScene s) : base (t, s, false)
        {
            CCDirector.SharedDirector.SetDepthTest(true);
        }
    }
}
