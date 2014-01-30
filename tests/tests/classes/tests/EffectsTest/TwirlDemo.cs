using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class TwirlDemo : CCTwirl
    {
		public TwirlDemo (float t) : base (t, new CCGridSize(12, 8))  
        {
            CCSize size = CCDirector.SharedDirector.WinSize;
			Position = size.Center;
			Twirls = 1;
			Amplitude = 2.5f;
        }
    }
}
