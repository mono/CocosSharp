using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    public static class CCNS
    {
        public static CCRect CCRectFromString(string pszContent)
        {
            CCRect result = new CCRect(0, 0, 0, 0);

            do
            {
                if (pszContent == null)
                {
                    break;
                }

                string content = pszContent;

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

                if (!SplitWithForm(pointStr, pointInfo))
                {
                    break;
                }
                List<string> sizeInfo = new List<string>();
                if (!SplitWithForm(sizeStr, sizeInfo))
                {
                    break;
                }

                float x = CCUtils.ccParseFloat(pointInfo[0]);
                float y = CCUtils.ccParseFloat(pointInfo[1]);
                float width = CCUtils.ccParseFloat(sizeInfo[0]);
                float height = CCUtils.ccParseFloat(sizeInfo[1]);

                result = new CCRect(x, y, width, height);
            } while (false);

            return result;
        }

        public static CCPoint CCPointFromString(string pszContent)
        {
            CCPoint ret = new CCPoint();

            do
            {
                List<string> strs = new List<string>();
                if (!SplitWithForm(pszContent, strs)) break;

                float x = CCUtils.ccParseFloat(strs[0]);
                float y = CCUtils.ccParseFloat(strs[1]);

                ret = new CCPoint(x, y);
            } while (false);

            return ret;
        }

        public static CCSize CCSizeFromString(string pszContent)
        {
            CCSize ret = new CCSize();

            do
            {
                List<string> strs = new List<string>();
                if (!SplitWithForm(pszContent, strs)) break;

                float width = CCUtils.ccParseFloat(strs[0]);
                float height = CCUtils.ccParseFloat(strs[1]);

                ret = new CCSize(width, height);
            } while (false);

            return ret;
        }

        public static void Split(string src, string token, List<string> vect)
        {
            int nend = 0;
            int nbegin = 0;
            while (nend != -1)
            {
                nend = src.IndexOf(token, nbegin);
                if (nend == -1)
                    vect.Add(src.Substring(nbegin, src.Length - nbegin));
                else
                    vect.Add(src.Substring(nbegin, nend - nbegin));
                nbegin = nend + token.Length;
            }
        }

        // first, judge whether the form of the string like this: {x,y}
        // if the form is right,the string will be splited into the parameter strs;
        // or the parameter strs will be empty.
        // if the form is right return true,else return false.
        public static bool SplitWithForm(string pStr, List<string> strs)
        {
            bool bRet = false;

            do
            {
                if (pStr == null)
                {
                    break;
                }

                // string is empty
                string content = pStr;
                if (content.Length == 0)
                {
                    break;
                }

                int nPosLeft = content.IndexOf('{');
                int nPosRight = content.IndexOf('}');

                // don't have '{' and '}'
                if (nPosLeft == -1 || nPosRight == -1)
                {
                    break;
                }
                // '}' is before '{'
                if (nPosLeft > nPosRight)
                {
                    break;
                }

                string pointStr = content.Substring(nPosLeft + 1, nPosRight - nPosLeft - 1);
                // nothing between '{' and '}'
                if (pointStr.Length == 0)
                {
                    break;
                }

                int nPos1 = pointStr.IndexOf('{');
                int nPos2 = pointStr.IndexOf('}');
                // contain '{' or '}' 
                if (nPos1 != -1 || nPos2 != -1) break;

                Split(pointStr, ",", strs);
                if (strs.Count != 2 || strs[0].Length == 0 || strs[1].Length == 0)
                {
                    strs.Clear();
                    break;
                }

                bRet = true;
            } while (false);

            return bRet;
        }
    }
}
