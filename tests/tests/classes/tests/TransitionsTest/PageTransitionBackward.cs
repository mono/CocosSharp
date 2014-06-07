using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PageTransitionBackward : CCTransitionPageTurn
    {
        public PageTransitionBackward (float t, CCScene s) : base (t, s, true)
        {
            CCApplication.SharedApplication.MainWindowDirector.IsUseDepthTesting = true;
        }
    }
}
