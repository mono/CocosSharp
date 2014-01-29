using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Waves3DDemo : CCWaves3D
    {
		public Waves3DDemo(float t) : base(t, new CCGridSize(15, 10), 5, 40) 
		{ 
			// Testing Properties
			//Waves = 5;
			//Amplitude = 40;
		}
    }
}
