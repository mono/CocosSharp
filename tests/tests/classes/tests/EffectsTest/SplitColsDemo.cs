using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocosSharp;

namespace tests
{
    public class SplitColsDemo : CCSplitCols
    {
        public new static CCActionInterval actionWithDuration(float t)
        {
            return new CCSplitCols(t, 9);
        }
    }
}
