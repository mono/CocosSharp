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
            CCSize size = CCDirector.SharedDirector.WinSize;
            return new CCLens3D(t, new CCGridSize(15, 10), new CCPoint(size.Width / 2, size.Height / 2), 240);
        }
    }
}
