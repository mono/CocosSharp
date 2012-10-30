using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class PageTransitionBackward : CCTransitionPageTurn
    {
        public new static CCTransitionScene Create(float t, CCScene s)
        {
            CCDirector.SharedDirector.SetDepthTest(true);
            return CCTransitionPageTurn.Create(t, s, true);
        }
    }
}
