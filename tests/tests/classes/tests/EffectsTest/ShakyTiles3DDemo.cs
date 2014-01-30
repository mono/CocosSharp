using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class ShakyTiles3DDemo : CCShakyTiles3D
    {
		public ShakyTiles3DDemo(float t) 
			: base (t, new CCGridSize(16, 12))//, 5, true)
        {
			// Testing properties
			Range = 5;
			ShakeZ = true;
        }
    }
}
