using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class TwirlDemo : CCTwirl
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            CCSize size = CCDirector.SharedDirector.WinSize;
            return CCTwirl.Create(new CCPoint(size.Width / 2, size.Height / 2), 1, 2.5f, new CCGridSize(12, 8), t);
        }
    }
}
