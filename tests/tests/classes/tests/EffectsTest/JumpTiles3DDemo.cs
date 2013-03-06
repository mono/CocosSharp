using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class JumpTiles3DDemo : CCJumpTiles3D
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;
            return CCJumpTiles3D.Create(2, 30, new CCGridSize(15, 10), t);
        }
    }
}
