using System;
using Microsoft.Xna.Framework.Content;


namespace CocosSharp
{
    internal class CCBMFontPaddingtReader : ContentTypeReader<CCBMFontConfiguration.CCBMGlyphPadding>
    {
        public CCBMFontPaddingtReader()
        {
        }
        
        protected override CCBMFontConfiguration.CCBMGlyphPadding Read (ContentReader input, CCBMFontConfiguration.CCBMGlyphPadding existingInstance)
        {
            var bottom = input.ReadInt32 ();
            var left = input.ReadInt32 ();
            var right = input.ReadInt32 ();
            var top = input.ReadInt32 ();

            var objectSize = new CCBMFontConfiguration.CCBMGlyphPadding();
            objectSize.Bottom = bottom;
            objectSize.Left = left;
            objectSize.Right = right;
            objectSize.Top = top;

            return objectSize;
        }
        
    }
}
