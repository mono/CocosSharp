using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCSizeConverter
    {
        public static CCSize CCSizeFromString(string content)
        {
            CCSize ret = new CCSize();

            do
            {
                List<string> strs = new List<string>();
                if (!CCUtils.SplitWithForm(content, strs)) break;

                float width = CCUtils.CCParseFloat(strs[0]);
                float height = CCUtils.CCParseFloat(strs[1]);

                ret = new CCSize(width, height);
            } while (false);

            return ret;
        }

    }
}

