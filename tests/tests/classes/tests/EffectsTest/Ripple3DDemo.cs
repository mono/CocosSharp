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
            CCSize size = CCDirector.SharedDirector.WinSize;
			return new CCRipple3D(t, new CCGridSize(32, 24), new CCPoint(size.Width / 2, size.Height / 2), size.Width / CCDirector.SharedDirector.ContentScaleFactor, 4, 160);
        }
    }
}
