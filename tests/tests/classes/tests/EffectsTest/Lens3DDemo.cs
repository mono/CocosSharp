using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Lens3DDemo : CCLens3D
    {
        static readonly CCSize contentSize = TextLayer.BaseNode.ContentSize;

        public Lens3DDemo(float t)
            : base(t, new CCGridSize(15, 10), contentSize.Center, contentSize.Height)
        {
			//Concave = true;
        }

    }
}
