using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cocos2d;

namespace tests
{
    public class SplitRowsDemo : CCSplitRows
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            return CCSplitRows.Create(9, t);
        }
    }
}
