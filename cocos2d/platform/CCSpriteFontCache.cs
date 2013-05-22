using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Cocos2D
{
    public class CCSpriteFontCache
    {
        private Dictionary<string, SpriteFont> _Cache = new Dictionary<string, SpriteFont>();
        public static string FontRoot = "fonts";

        private CCSpriteFontCache()
        {
            // You don't create this, we do.
        }

        public SpriteFont GetFont(string fontName)
        {
            string key = string.Format("{0}/{1}", fontName.ToLower(), "ANY");
            if (_Cache.ContainsKey(key))
            {
                return (_Cache[key]);
            }
            SpriteFont font = null;
            try
            {
                font = CCApplication.SharedApplication.Content.Load<SpriteFont>(Path.Combine("fonts", fontName));
            }
            catch (Exception)
            {
                CCLog.Log("Can't find font known as {0}. Please check your file name.", fontName);
            }
            if (font != null)
            {
                _Cache[key] = font;
            }
            return (font);
        }

        public SpriteFont GetFont(string fontName, float size)
        {
            if (size <= 0f)
            {
                return (GetFont(fontName));
            }
            string key = string.Format("{0}/{1}", fontName.ToLower(), size);
            if (_Cache.ContainsKey(key))
            {
                return (_Cache[key]);
            }
            SpriteFont font = null;
            try
            {
                font = CCApplication.SharedApplication.Content.Load<SpriteFont>(Path.Combine("fonts", string.Format("{0}-{1}", fontName, size)));
            }
            catch (Exception)
            {
                CCLog.Log("Can't find {0}-{1}, going to try using {0} instead.", fontName, size);
                return (GetFont(fontName));
            }
            if (font != null)
            {
                _Cache[key] = font;
            }
            return (font);
        }

        public void Clear()
        {
            _Cache.Clear();
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
