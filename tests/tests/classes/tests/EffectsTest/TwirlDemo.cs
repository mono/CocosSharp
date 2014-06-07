using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TwirlDemo : CCTwirl
    {
        static readonly CCSize winSize = CCApplication.SharedApplication.MainWindowDirector.WinSize;

        public TwirlDemo (float t) : base (t, new CCGridSize(12, 8), winSize.Center, 1, 2.5f)  
        {
        }
    }
}
