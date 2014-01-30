using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
	public class ShatteredTiles3DDemo : CCShatteredTiles3D 
    {
		public ShatteredTiles3DDemo(float t) 
			: base(t, new CCGridSize(16, 12), 5, true)
        {
			// Testing Properties
			//Range = 5;
			//ShatterZ = true;
        }
    }
}
