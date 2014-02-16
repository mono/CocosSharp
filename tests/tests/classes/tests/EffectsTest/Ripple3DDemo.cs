using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Ripple3DDemo : CCRipple3D
    {
        static readonly CCSize contentSize = TextLayer.BaseNode.ContentSize;

        public Ripple3DDemo(float t) 
            : base (t, new CCGridSize(32, 24), contentSize.Center, contentSize.Width, 4, 160)
		{
		}

    }
}
