using System;
using System.Runtime.InteropServices;

namespace Cocos2D
{
    public partial class CCLabel
    {

        private void CreateFont(string fontName, float fontSize, CCRawList<char> charset)
        {
			throw(new NotImplementedException("PSM support for CCLabel is not implemented yet."));
        }

        private void CreateBitmap(int width, int height)
        {
			throw(new NotImplementedException("PSM support for CCLabel is not implemented yet."));
        }

        private float GetFontHeight()
        {
			throw(new NotImplementedException("PSM support for CCLabel is not implemented yet."));
        }

        private CCSize GetMeasureString(string text)
        {
			throw(new NotImplementedException("PSM support for CCLabel is not implemented yet."));
        }

        private KerningInfo GetKerningInfo(char ch)
        {
			throw(new NotImplementedException("PSM support for CCLabel is not implemented yet."));
        }

        private unsafe byte* GetBitmapData(string text, out int stride)
        {
			throw(new NotImplementedException("PSM support for CCLabel is not implemented yet."));
        }
    }
}
