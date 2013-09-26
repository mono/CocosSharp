using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Cocos2D
{
    public class CCSpriteFontCache
    {
        private struct FontMapEntry
        {
            public string FontName;
            public float FontSize;
        }

        private static ContentManager _contentManager;
        public static string FontRoot = "fonts";

        private static Dictionary<string, int[]> _registeredFonts = new Dictionary<string, int[]>(StringComparer.OrdinalIgnoreCase);
        private static Dictionary<string, FontMapEntry> _loadedFontsMap = new Dictionary<string, FontMapEntry>();
        private static float _fontScale = 1.0f;

        public static void RegisterFont(string fontName, params int[] sizes)
        {
            Array.Sort(sizes);
            _registeredFonts[fontName] = sizes;
        }

        public static float FontScale
        {
            get { return _fontScale; }
            set { _fontScale = value; }
        }

        private CCSpriteFontCache()
        {
            // You don't create this, we do.
        }

        static CCSpriteFontCache()
        {
            var cm = CCApplication.SharedApplication.Content;
            _contentManager = new ContentManager(cm.ServiceProvider, Path.Combine(cm.RootDirectory, FontRoot));
        }

        private string FontKey(string fontName, float fontSize)
        {
            if (fontSize == 0)
            {
                return fontName;
            }
            return String.Format("{0}-{1}", fontName, fontSize);
        }

        public SpriteFont GetFont(string fontName)
        {
            try
            {
                return _contentManager.Load<SpriteFont>(fontName);
            }
            catch (Exception)
            {
                CCLog.Log("Can't find font known as {0}. Please check your file name.", fontName);
                return null;
            }
        }

        public SpriteFont TryLoadFont(string fontName, float fontSize, out float loadedSize)
        {
            var key = FontKey(fontName, fontSize);
            
            if (_loadedFontsMap.ContainsKey(key))
            {
                //Already loaded
                var entry = _loadedFontsMap[key];
                loadedSize = entry.FontSize;
                return _contentManager.Load<SpriteFont>(FontKey(entry.FontName, entry.FontSize));
            }

            SpriteFont result = null;
            var loadedName = fontName;
            loadedSize = fontSize;
            try
            {
                result = InternalLoadFont(fontName, fontSize, out loadedSize);
            }
            catch (ContentLoadException)
            {
            }

            //Try Default Font
            if (result == null)
            {
                CCLog.Log("Can't find {0}, use system default ({1})", fontName, CCDrawManager.DefaultFont);
                try
                {
                    loadedName = CCDrawManager.DefaultFont;
                    result = InternalLoadFont(loadedName, fontSize, out loadedSize);
                }
                catch (ContentLoadException)
                {
                }
            }

            if (result != null)
            {
                _loadedFontsMap.Add(key, new FontMapEntry() {FontName = loadedName, FontSize = loadedSize});
            }

            return result;
        }

        private SpriteFont InternalLoadFont(string fontName, float fontSize, out float loadedSize)
        {
            loadedSize = fontSize;

            try
            {
                return _contentManager.Load<SpriteFont>(FontKey(fontName, fontSize));
            }
            catch (ContentLoadException)
            {
            }

            //Try nearest size
            if (_registeredFonts.ContainsKey(fontName))
            {
                var sizes = _registeredFonts[fontName];

                loadedSize = sizes[sizes.Length - 1];

                for (int i = 0; i < sizes.Length; i++)
                {
                    if (sizes[i] >= fontSize)
                    {
                        loadedSize = sizes[i];
                        break;
                    }
                }

                try
                {
                    return _contentManager.Load<SpriteFont>(FontKey(fontName, loadedSize));
                }
                catch (ContentLoadException)
                {
                }
            }

            loadedSize = 0;
            return _contentManager.Load<SpriteFont>(fontName);
        }

        public void Clear()
        {
            _contentManager.Unload();
            _loadedFontsMap.Clear();
        }

        #region Singleton
        
        public static CCSpriteFontCache SharedInstance
        {
            get
            {
                return (_Instance);
            }
        }
        private static CCSpriteFontCache _Instance = new CCSpriteFontCache();
        
        #endregion
    }
}
