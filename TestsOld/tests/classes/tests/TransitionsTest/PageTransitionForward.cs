using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PageTransitionForward : CCTransitionPageTurn
    {
        public PageTransitionForward (float t, CCScene s) : base (t, s, false)
        {
            GameView.DepthTesting = true;
        }

        public override void OnExit()
        {
            GameView.DepthTesting = false;

            base.OnExit();
        }
    }
}
