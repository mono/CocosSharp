using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Ripple3DDemo : CCRipple3D
    {

		public Ripple3DDemo(float t) : base (t, new CCGridSize(32, 24))
		{
			var size = TextLayer.BaseNode.ContentSize;
			Position = size.Center;
			Radius = size.Width;
			Waves = 4;
			Amplitude = 160;
		}

    }
}
