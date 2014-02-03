using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class WavesTiles3DDemo : CCWavesTiles3D
    {
		public WavesTiles3DDemo(float t)
			: base (t, new CCGridSize(15, 10), 4, 120)
        {
			// Testing properties
			//Waves = 4;
			//Amplitude = 120;
        }
    }
}
