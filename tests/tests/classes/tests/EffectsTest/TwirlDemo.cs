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
            return CCTwirl.Create(new CCPoint(size.width / 2, size.height / 2), 1, 2.5f, new ccGridSize(12, 8), t);
        }
    }
}
