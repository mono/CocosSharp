using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Cocos2D
{
    public class CCSpriteFontCache
    {
        private static ContentManager _contentManager;
        public static string FontRoot = "fonts";

        private CCSpriteFontCache()
        {
            // You don't create this, we do.
        }

        static CCSpriteFontCache()
        {
            var cm = CCApplication.SharedApplication.Content;
            _contentManager = new ContentManager(cm.ServiceProvider, Path.Combine(cm.RootDirectory, FontRoot));
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

        public SpriteFont GetFont(string fontName, float size)
        {
            if (size <= 0f)
            {
                return GetFont(fontName);
            }

            try
            {
                return _contentManager.Load<SpriteFont>(String.Format("{0}-{1}", fontName, size));
            }
            catch (Exception)
            {
                CCLog.Log("Can't find {0}-{1}, going to try using {0} instead.", fontName, size);
                return GetFont(fontName);
            }
        }

        public void Clear()
        {
            _contentManager.Unload();
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
