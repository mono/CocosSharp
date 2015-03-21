using System;
using System.Collections.Generic;

namespace CocosSharp
{

    internal struct CCFontLetterDefinition
    {
        public char LetterChar;
        private float u;
        private float v;
        private float width;
        private float height;
        private CCRect subRect;
        public float XOffset;
        public float YOffset;
        public int TextureID;
        public bool IsValidDefinition;
        public int XAdvance;
        public CCRect Cropping;

        public CCRect Subrect
        {
            get {return subRect; }
            set 
            {
                subRect = value;
                U = subRect.Origin.X;
                V = subRect.Origin.Y;
                width = subRect.Size.Width;
                height = subRect.Size.Height;
            }
        }

        public float U 
        {
            get { return u; }
            set 
            {
                u = value;
                subRect.Origin.X = value;
            }
        }

        public float V
        {
            get { return v; }
            set 
            {
                v = value;
                subRect.Origin.Y = value;
            }
        }

        public float Width 
        {
            get { return width; }
            set 
            {
                width = value;
                subRect.Size.Width = value;
            }
        }

        public float Height
        {
            get { return height; }
            set 
            {
                height = value;
                subRect.Size.Width = value;
            }
        }
    };

    internal class CCFontAtlas
    {
        const int CacheTextureWidth = 512;
        const int CacheTextureHeight = 512;

        public CCFont Font { get; set; }
        CCTexture2D Texture { get; set; }

        Dictionary<char, CCFontLetterDefinition> FontLetterDefinitions;
        Dictionary<int, CCTexture2D> AtlasTextures;

        public float CommonHeight { get; internal set;}
        public char? DefaultCharacter { get; set; }

        public CCFontAtlas (CCFont font)
        { 
            Font = font;
            FontLetterDefinitions = new Dictionary<char, CCFontLetterDefinition>();
            AtlasTextures = new Dictionary<int, CCTexture2D>();
        }

        public void AddLetterDefinition(CCFontLetterDefinition letterDefinition)
        {
            FontLetterDefinitions[letterDefinition.LetterChar] = letterDefinition;
        }

        public bool GetLetterDefinitionForChar(char letteCharUTF16, out CCFontLetterDefinition outDefinition)
        {
            var found = FontLetterDefinitions.TryGetValue(letteCharUTF16, out outDefinition);

            return (found) ? outDefinition.IsValidDefinition : false;
        }

        public void AddTexture(CCTexture2D texture, int slot)
        {
            AtlasTextures[slot] = texture;
        }

        public CCTexture2D GetTexture(int slot)
        {
            return AtlasTextures[slot];
        }

        public bool IsContainsGlyphs
        {
            get { return FontLetterDefinitions.Count > 0; }
        }

        public bool PrepareLetterDefinitions(string utf16String)
        {
            // Not implemented right now
            return false;
        }

        public void PurgeTexturesAtlas ()
        {

        }
    }
        

}
