using System;
using System.Collections.Generic;

namespace CocosSharp
{
    internal class CCPointConverter
    {
        public static CCPoint CCPointFromString(string content)
        {
            CCPoint ret = CCPoint.Zero;

            do
            {
                List<string> strs = new List<string>();
                if (!CCUtils.SplitWithForm(content, strs)) break;

                float x = CCUtils.CCParseFloat(strs[0]);
                float y = CCUtils.CCParseFloat(strs[1]);

                ret.X = x;
                ret.Y = y;

            } while (false);

            return ret;
        }

    }
}

