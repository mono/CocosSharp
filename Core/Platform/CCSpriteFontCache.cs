using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace CocosSharp
{
    public class CCSpriteFontCache
    {
        #region Structs

        struct FontMapEntry
        {
            public string FontName;
            public string FontPath;
            public float FontSize;
        }

        #endregion Structs

        public static string DefaultFont = "arial";

        CCContentManager contentManager;
        Dictionary<string, int[]> registeredFonts;
        Dictionary<string, FontMapEntry> loadedFontsMap;


        #region Properties

        public static CCSpriteFontCache SharedSpriteFontCache { get; internal set; }

        public float FontScale { get; set; }

        public SpriteFont this[string fontName]
        {
            get 
            {
                try 
                {
                    return contentManager.Load<SpriteFont> (fontName);
                } 
                catch (Exception) 
                {
                    CCLog.Log ("Can't find font known as {0}. Please check your file name.", fontName);
                    return null;
                }
            }
        }

        #endregion Properties


        #region Constructors

        internal CCSpriteFontCache(CCContentManager cm)
        {
            FontScale = 1.0f;

            registeredFonts = new Dictionary<string, int[]>(StringComparer.OrdinalIgnoreCase);
            loadedFontsMap = new Dictionary<string, FontMapEntry>();
            contentManager = cm;
        }

        #endregion Constructors


        public void RegisterFont(string fontName, params int[] sizes)
        {
            Array.Sort(sizes);
            registeredFonts[fontName] = sizes;
        }

        public void Clear()
        {
            contentManager.Unload();
            loadedFontsMap.Clear();
        }

        string FontKey(string fontName, float fontSize)
        {
            if (fontSize == 0)
            {
                return fontName;
            }
            return String.Format("{0}-{1}", fontName, fontSize);
        }

        internal SpriteFont TryLoadFont(string fontName, float fontSize, out float loadedSize)
        {
            var key = FontKey(fontName, fontSize);

            if (loadedFontsMap.ContainsKey(key))
            {
                //Already loaded
                var entry = loadedFontsMap[key];
                loadedSize = entry.FontSize;
                return contentManager.Load<SpriteFont>(entry.FontPath);
            }

            SpriteFont result = null;
            var loadedName = fontName;
            loadedSize = fontSize;
            var fontPath = fontName;

            try
            {
                result = InternalLoadFont(fontName, fontSize, out loadedSize, out fontPath);
            }
            catch (ContentLoadException)
            {
            }

            //Try Default Font
            if (result == null)
            {
                CCLog.Log("Can't find {0}, use system default ({1})", fontName, DefaultFont);
                try
                {
                    loadedName = DefaultFont;
                    result = InternalLoadFont(loadedName, fontSize, out loadedSize, out fontPath);
                }
                catch (ContentLoadException)
                {
                }
            }
            else 
            {
                loadedFontsMap.Add(key, new FontMapEntry() {FontName = loadedName, FontSize = loadedSize, FontPath = fontPath});
            }

            return result;
        }

        SpriteFont InternalLoadFont(string fontName, float fontSize, out float loadedSize, out string fontPath)
        {
            loadedSize = fontSize;
            fontPath = fontName;

            var searchPaths = CCContentManager.SharedContentManager.SearchPaths;
            var searchResolutionsOrder = CCContentManager.SharedContentManager.SearchResolutionsOrder;
            foreach (var searchPath in searchPaths)
            {
                foreach (string resolutionOrder in searchResolutionsOrder)
                {
                    fontPath = Path.Combine(Path.Combine(searchPath, resolutionOrder), FontKey(fontName, fontSize));

                    try
                    {
                        //TODO: for platforms with access to the file system, first check for the existence of the file 
                        return contentManager.Load<SpriteFont>(fontPath);
                    }
                    catch (ContentLoadException)
                    {
                        // try other path
                    }
                }
            }

            //Try nearest size
            if (registeredFonts.ContainsKey(fontName))
            {
                var sizes = registeredFonts[fontName];

                loadedSize = sizes[sizes.Length - 1];

                for (int i = 0; i < sizes.Length; i++)
                {
                    if (sizes[i] >= fontSize)
                    {
                        loadedSize = sizes[i];
                        break;
                    }
                }

                foreach (var searchPath in searchPaths)
                {
                    foreach (string resolutionOrder in searchResolutionsOrder)
                    {
                        fontPath = Path.Combine(Path.Combine(searchPath, resolutionOrder), FontKey(fontName, loadedSize));

                        try
                        {
                            //TODO: for platforms with access to the file system, first check for the existence of the file 
                            return contentManager.Load<SpriteFont>(fontPath);
                        }
                        catch (ContentLoadException)
                        {
                            // try other path
                        }
                    }
                }
            }

            loadedSize = 0;
            foreach (var searchPath in searchPaths)
            {
                foreach (string resolutionOrder in searchResolutionsOrder)
                {
                    fontPath = Path.Combine(Path.Combine(searchPath, resolutionOrder), fontName);

                    try
                    {
                        //TODO: for platforms with access to the file system, first check for the existence of the file 
                        return contentManager.Load<SpriteFont>(fontPath);
                    }
                    catch (ContentLoadException)
                    {
                        // try other path
                    }
                }
            }

            return null;
        }
    }
}
