using System;
using System.Collections.Generic;

namespace CocosSharp
{
    public class CCFontFNT : CCFont
    {
        internal CCBMFontConfiguration Configuration { get; set; }
        CCVector2 _imageOffset;
        public bool IsFontConfigValid { get; private set; }

        public CCFontFNT(string fntFilePath, CCVector2 imageOffset = default(CCVector2))
        { 
            Configuration = CCBMFontConfiguration.FontConfigurationWithFile(fntFilePath);
            _imageOffset = imageOffset;

            if (Configuration != null)
                IsFontConfigValid = true;
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

            // Check if any Bitmap Font configurations are loaded at all.
            if (Configuration.Glyphs.Count == 0)
                return null;

            if (Configuration.CommonHeight == 0)
                return null;


            // Set the atlas's common height
            atlas.CommonHeight = Configuration.CommonHeight;

            foreach (var glyph in Configuration.Glyphs)
            {
                var glyphDefintion = new CCFontLetterDefinition();

                glyphDefintion.LetterChar = (char)glyph.Key;

                var glyphDef = glyph.Value;

                glyphDefintion.XOffset = glyphDef.XOffset;
                glyphDefintion.YOffset = glyphDef.YOffset;

                glyphDefintion.Subrect = glyphDef.Subrect;

                glyphDefintion.TextureID = 0;

                glyphDefintion.IsValidDefinition = true;
                glyphDefintion.XAdvance = glyphDef.XAdvance;

                atlas.AddLetterDefinition(glyphDefintion);
            }

            // Try to load the texture
            CCTexture2D atlasTexture = null;

            try
            {
                atlasTexture = CCTextureCache.SharedTextureCache.AddImage(Configuration.AtlasName);
            }
            catch (Exception)
            {
                // Try the 'images' ref location just in case.
                try
                {
                    atlasTexture =
                        CCTextureCache.SharedTextureCache.AddImage(System.IO.Path.Combine("images",
                            Configuration.AtlasName));
                }
                catch (Exception)
                {
                    // Lastly, try <font_path>/images/<font_name>
                    string dir = System.IO.Path.GetDirectoryName(Configuration.AtlasName);
                    string fname = System.IO.Path.GetFileName(Configuration.AtlasName);
                    string newName = System.IO.Path.Combine(System.IO.Path.Combine(dir, "images"), fname);
                    atlasTexture = CCTextureCache.SharedTextureCache.AddImage(newName);
                }
            }

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

            if (Configuration.GlyphKernings.Count > 0)
            {
                CCBMFontConfiguration.CCKerningHashElement element;
                if (Configuration.GlyphKernings.TryGetValue(key, out element))
                {
                    ret = element.Amount;
                }
            }
            return ret;
        }

    }
}

