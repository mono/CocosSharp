using System;
using System.Collections.Generic;
using System.Text;

namespace CocosSharp
{
    internal class CCFontAtlasCache
    {
        Dictionary<string, CCFontAtlas> atlasMap;

        #region Properties

        public static CCFontAtlasCache SharedFontAtlasCache { get; internal set; }

        #endregion Properties


        #region Constructors

        public CCFontAtlasCache()
        {
            atlasMap = new Dictionary<string, CCFontAtlas>();
        }

        #endregion Constructors


        public void PurgeCachedData() 
        {
            foreach (var atlas in atlasMap.Values)
            {
                atlas.PurgeTexturesAtlas();
            }

            atlasMap.Clear();
        }

        string GenerateFontName(string fontFileName, int size, GlyphCollection type, bool useDistanceField)
        {
            var tempName = new StringBuilder(fontFileName);

            tempName.Append("_");
            tempName.Append(type.ToString());
            tempName.Append("_");
            if (useDistanceField)
                tempName.Append("_df");
            tempName.Append("_");
            tempName.Append(size);

            return tempName.ToString();
        }

        public CCFontAtlas GetFontAtlasFNT (string fontFileName, CCVector2 imageOffset = default(CCVector2))
        {
            string atlasName = GenerateFontName(fontFileName, 0, GlyphCollection.Custom,false);
            var atlasAlreadyExists = atlasMap.ContainsKey(atlasName);

            if (!atlasAlreadyExists)
            {
                var font = new CCFontFNT(fontFileName, imageOffset);
                if (font != null)
                {
                    var tempAtlas = font.CreateFontAtlas();
                    if (tempAtlas != null)
                    {
                        atlasMap[atlasName] = tempAtlas;
                        return atlasMap[atlasName];
                    }
                }
            }
            else
            {
                return atlasMap[atlasName];
            }
           

            return null;
        }

        public CCFontAtlas GetFontAtlasFNT(CCFontFNT font, CCVector2 imageOffset = default(CCVector2))
        {
            if (font != null)
            {
                var atlasName = font.Configuration.AtlasName;
                var tempAtlas = font.CreateFontAtlas();
                if (tempAtlas != null)
                {
                    atlasMap[atlasName] = tempAtlas;
                    return atlasMap[atlasName];
                }
            }


            return null;
        }

        public CCFontAtlas GetFontAtlasSpriteFont (string fontFileName, float fontSize, CCVector2 imageOffset = default(CCVector2))
        {
            string atlasName = GenerateFontName(fontFileName, (int)fontSize, GlyphCollection.Custom,false);
            var atlasAlreadyExists = atlasMap.ContainsKey(atlasName);

            if (!atlasAlreadyExists)
            {
                var font = new CCFontSpriteFont(fontFileName, fontSize, imageOffset);
                if (font != null)
                {
                    var tempAtlas = font.CreateFontAtlas();
                    if (tempAtlas != null)
                    {
                        atlasMap[atlasName] = tempAtlas;
                        return atlasMap[atlasName];
                    }
                }
            }
            else
            {
                return atlasMap[atlasName];
            }

            return null;
        }

    }
}

