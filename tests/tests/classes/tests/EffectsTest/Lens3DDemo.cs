using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Lens3DDemo : CCLens3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
			var size = TextLayer.BaseNode[EffectTestScene.kTagBackground].ContentSize;
			return new CCLens3D(t, new CCGridSize(15, 10), size.Center, size.Width);
        }
    }
}
