using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class JumpTiles3DDemo : CCJumpTiles3D
    {
		public JumpTiles3DDemo(float t)
			: base (t, new CCGridSize(15, 10), 2, 30)
        {
			//Testing properities
			//NumberOfJumps = 2;
			//Amplitude = 30;
        }
    }
}
