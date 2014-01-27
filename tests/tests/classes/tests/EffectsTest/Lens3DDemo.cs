using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Lens3DDemo : CCLens3D
    {

        public Lens3DDemo(float t)
            : base(t, new CCGridSize(15, 10))
        {
            var size = TextLayer.BaseNode.ContentSize;
			Position = size.Center; 
			Radius = size.Height;
			//Concave = true;
        }

    }
}
