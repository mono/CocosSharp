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
            public float FontSize;
        }

		#endregion Structs


		static CCSpriteFontCache instance = new CCSpriteFontCache();
		static ContentManager contentManager;
        public static string FontRoot = "fonts";

		static Dictionary<string, int[]> registeredFonts = new Dictionary<string, int[]>(StringComparer.OrdinalIgnoreCase);
		static Dictionary<string, FontMapEntry> loadedFontsMap = new Dictionary<string, FontMapEntry>();

 
		#region Properties

		public static CCSpriteFontCache SharedInstance
		{
			get { return instance; }
		}

		public static float FontScale { get; set; }

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

        static CCSpriteFontCache()
        {
            var cm = CCApplication.SharedApplication.Content;
            contentManager = new ContentManager(cm.ServiceProvider, Path.Combine(cm.RootDirectory, FontRoot));
			FontScale = 1.0f;
        }

		CCSpriteFontCache()
		{
			// You don't create this, we do.
		}

		#endregion Constructors

		public static void RegisterFont(string fontName, params int[] sizes)
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
                return contentManager.Load<SpriteFont>(FontKey(entry.FontName, entry.FontSize));
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
                loadedFontsMap.Add(key, new FontMapEntry() {FontName = loadedName, FontSize = loadedSize});
            }

            return result;
        }

        SpriteFont InternalLoadFont(string fontName, float fontSize, out float loadedSize)
        {
            loadedSize = fontSize;

            try
            {
                return contentManager.Load<SpriteFont>(FontKey(fontName, fontSize));
            }
            catch (ContentLoadException)
            {
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

                try
                {
                    return contentManager.Load<SpriteFont>(FontKey(fontName, loadedSize));
                }
                catch (ContentLoadException)
                {
                }
            }

            loadedSize = 0;
            return contentManager.Load<SpriteFont>(fontName);
        }
    }
}
