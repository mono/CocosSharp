using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class LiquidDemo : CCLiquid
    {
		public LiquidDemo (float t)
			: base (t, new CCGridSize(16, 12), 4, 20)
        {
			// Testing properties
			//Waves = 4;
			//Amplitude = 20;
        }
    }
}
