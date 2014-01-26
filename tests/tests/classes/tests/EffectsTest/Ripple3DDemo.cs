using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class Ripple3DDemo : CCRipple3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
			var size = TextLayer.BaseNode[EffectTestScene.kTagBackground].ContentSize;
			return new CCRipple3D(t, new CCGridSize(32, 24), size.Center, size.Width, 4, 160);
        }
    }
}
