using System;
using Microsoft.Xna.Framework.Content;


namespace Cocos2D
{
    internal class CCBMFontPaddingtReader : ContentTypeReader<CCBMFontConfiguration.CCBMFontPadding>
    {
        public CCBMFontPaddingtReader()
        {
        }
        
        protected override CCBMFontConfiguration.CCBMFontPadding Read (ContentReader input, CCBMFontConfiguration.CCBMFontPadding existingInstance)
        {
            var bottom = input.ReadInt32 ();
            var left = input.ReadInt32 ();
            var right = input.ReadInt32 ();
            var top = input.ReadInt32 ();

            var objectSize = new CCBMFontConfiguration.CCBMFontPadding();
            objectSize.bottom = bottom;
            objectSize.left = left;
            objectSize.right = right;
            objectSize.top = top;

            return objectSize;
        }
        
    }
}
