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
            var size = TextLayer.BaseNode[EffectTestScene.kTagBackground].ContentSize;
            var sizeinpixels = TextLayer.BaseNode[EffectTestScene.kTagBackground].ContentSizeInPixels;
			Position = size.Center; // CCDirector.SharedDirector.ContentScaleFactor;
			Radius = size.Height;// / CCDirector.SharedDirector.ContentScaleFactor;
			//Concave = true;
        }

    }
}
