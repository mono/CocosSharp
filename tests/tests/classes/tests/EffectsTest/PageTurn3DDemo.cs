using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class PageTurn3DDemo : CCPageTurn3D
    {
		public PageTurn3DDemo(float t)
			: base (t, new CCGridSize(15, 10))
        {
            //Director.IsUseDepthTesting = true;
        }
    }
}
