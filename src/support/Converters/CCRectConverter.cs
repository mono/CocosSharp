using System;
using System.Collections.Generic;

namespace CocosSharp
{
    internal class CCRectConverter
    {
        public static CCRect CCRectFromString(string rectSpec)
        {
            CCRect result = CCRect.Zero;

            do
            {
                if (rectSpec == null)
                {
                    break;
                }

                string content = rectSpec;

                // find the first '{' and the third '}'
                int nPosLeft = content.IndexOf('{');
                int nPosRight = content.IndexOf('}');
                for (int i = 1; i < 3; ++i)
                {
                    if (nPosRight == -1)
                    {
                        break;
                    }
                    nPosRight = content.IndexOf('}', nPosRight + 1);
                }
                if (nPosLeft == -1 || nPosRight == -1)
                {
                    break;
                }
                content = content.Substring(nPosLeft + 1, nPosRight - nPosLeft - 1);
                int nPointEnd = content.IndexOf('}');
                if (nPointEnd == -1)
                {
                    break;
                }
                nPointEnd = content.IndexOf(',', nPointEnd);
                if (nPointEnd == -1)
                {
                    break;
                }

                // get the point string and size string
                string pointStr = content.Substring(0, nPointEnd);
                string sizeStr = content.Substring(nPointEnd + 1);
                //, content.Length - nPointEnd
                // split the string with ','
                List<string> pointInfo = new List<string>();

                if (!CCUtils.SplitWithForm(pointStr, pointInfo))
                {
                    break;
                }
                List<string> sizeInfo = new List<string>();
                if (!CCUtils.SplitWithForm(sizeStr, sizeInfo))
                {
                    break;
                }

                float x = CCUtils.CCParseFloat(pointInfo[0]);
                float y = CCUtils.CCParseFloat(pointInfo[1]);
                float width = CCUtils.CCParseFloat(sizeInfo[0]);
                float height = CCUtils.CCParseFloat(sizeInfo[1]);

                result = new CCRect(x, y, width, height);
            } while (false);

            return result;
        }
    }
}

