using System;
using System.Collections.Generic;

namespace CocosSharp
{

    public class CCFont
    {

        const string GLYPH_ASCII = "\"!#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~¡¢£¤¥¦§¨©ª«¬­®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþ ";
        const string GLYPH_NEHE =  "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~ ";


        GlyphCollection usedGlyphs;
        List<char> customGlyphs;

        public CCFont()
        {     
            usedGlyphs = GlyphCollection.Ascii;
            customGlyphs = new List<char>();
        }

        internal virtual CCFontAtlas CreateFontAtlas() { return null; }

        public virtual int[] HorizontalKerningForText (string text, out int numLetters)
        {
            numLetters = 0;
            return null;
        }

        public string GetGlyphCollection ( GlyphCollection glyphs )
        {
            switch (glyphs)
            {
                case GlyphCollection.NEHE:
                    return GLYPH_NEHE;
                case GlyphCollection.Ascii:
                    return GLYPH_ASCII;
                default:
                    return string.Empty;

            }
        }

        public virtual List<char> CurrentGlyphCollection
        {
            get { return (customGlyphs.Count > 0) ? customGlyphs : new List<char>(GetGlyphCollection(usedGlyphs)); }
        }

        public virtual void SetCurrentGlyphCollection (GlyphCollection glyphs, string customGlyphs)
        {

            switch (glyphs)
            {
                case GlyphCollection.NEHE:
                case GlyphCollection.Ascii:
                    this.customGlyphs.Clear();
                    break;
                default:
                    this.customGlyphs.AddRange(customGlyphs);
                    break;

            }
            usedGlyphs = glyphs;
        }

        public virtual int FontMaxHeight
        {
            get { return 0; }
        }

    }
}

