using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCFontSpriteFont : CCFont
    {
        CCVector2 _imageOffset;
        public bool IsFontConfigValid { get; private set; }
        string fontName;
        float fontSize;

        public CCFontSpriteFont (string fntFilePath, float fontSize, CCVector2 imageOffset = default(CCVector2))
        { 
            fontName = fntFilePath;
            this.fontSize = fontSize;
        }

        /// <summary>
        /// Purges the cached data.
        /// Removes from memory the cached configurations and the atlas name dictionary.
        /// </summary>
        public void PurgeCachedData()
        {
            //            if (Configuration != null)
            //                Configuration.Clear();
        }

        internal override CCFontAtlas CreateFontAtlas()
        {
            var atlas = new CCFontAtlas(this);

            // Try to load the texture
            CCTexture2D atlasTexture = null;

            float loadedSize = fontSize;

            SpriteFont font = CCSpriteFontCache.SharedInstance.TryLoadFont(fontName, fontSize, out loadedSize);
            if (font == null)
            {
                atlasTexture = new CCTexture2D(); 
            }
            else
            {
#if XNA
                atlasTexture = new CCTexture2D();
#else
                atlasTexture = new CCTexture2D(font.Texture);
#endif
            }

            // add the texture (only one for now)
            atlas.AddTexture(atlasTexture, 0);

            // Set the atlas's common height
            atlas.CommonHeight = font.LineSpacing;
            // Set the default character to us if a character does not exist in the font
            atlas.DefaultCharacter = font.DefaultCharacter;

#if !XNA
            
            var glyphs = font.GetGlyphs();
            var reusedRect = Rectangle.Empty;

            foreach ( var character in font.Characters)
            {
                var glyphDefintion = new CCFontLetterDefinition();

                glyphDefintion.LetterChar = character;

                var glyphDef = glyphs[character];

                glyphDefintion.XOffset = glyphDef.LeftSideBearing + glyphDef.Cropping.X;

                glyphDefintion.YOffset = glyphDef.Cropping.Y;

                reusedRect = glyphDef.BoundsInTexture;
                glyphDefintion.Subrect = new CCRect(reusedRect.X, reusedRect.Y, reusedRect.Width, reusedRect.Height);

                reusedRect = glyphDef.Cropping;
                glyphDefintion.Cropping = new CCRect(reusedRect.X, reusedRect.Y, reusedRect.Width, reusedRect.Height);

                glyphDefintion.TextureID = 0;

                glyphDefintion.IsValidDefinition = true;
                glyphDefintion.XAdvance = (int)(font.Spacing + glyphDef.Width + glyphDef.RightSideBearing);//(int)glyphDef.WidthIncludingBearings;

                atlas.AddLetterDefinition(glyphDefintion);

            }
#endif
            return atlas;
        }

        public override int[] HorizontalKerningForText(string text, out int numLetters)
        {
            numLetters = text.Length;

            if (numLetters == 0)
                return null;

            var sizes = new int[numLetters];
            if (sizes.Length == 0)
                return null;

            for (int c = 0; c < numLetters; ++c)
            {
                if (c < (numLetters-1))
                    sizes[c] = GetHorizontalKerningForChars(text[c], text[c+1]);
                else
                    sizes[c] = 0;
            }

            return sizes;

        }

        private int GetHorizontalKerningForChars(char firstChar, char secondChar)
        {
            int ret = 0;
            int key = (firstChar << 16) | (secondChar & 0xffff);

            return ret;
        }

    }
}

