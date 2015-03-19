using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCFontSpriteFont : CCFont
    {
        CCVector2 _imageOffset;
        public bool IsFontConfigValid { get; private set; }

        public CCFontSpriteFont (string fntFilePath, CCVector2 imageOffset = default(CCVector2))
        { 

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

            if (atlasTexture == null)
                atlasTexture = new CCTexture2D();

            // add the texture (only one for now)
            atlas.AddTexture(atlasTexture, 0);

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

