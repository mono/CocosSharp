using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class Lens3DDemo : CCLens3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;
            return CCLens3D.Create(new CCPoint(size.Width / 2, size.Height / 2), 240, new ccGridSize(15, 10), t);
        }
    }
}
