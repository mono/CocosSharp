using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class WavesDemo : CCWaves
    {
		public WavesDemo (float t)
			: base (t, new CCGridSize(16, 12), 4, 20, true, true)
        {
			// Testing properties
			//Waves = 4;
			//Amplitude = 20;
        }
    }
}
