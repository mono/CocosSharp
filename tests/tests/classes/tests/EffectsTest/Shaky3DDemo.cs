using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Shaky3DDemo : CCShaky3D
    {
		public Shaky3DDemo (float t) : base (t, new CCGridSize(15, 10), 10, true)
		{
			// Testing of Properties
//			Range = 5;
//			Shake = true;
        }
    }
}
