using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    //TODO: AsyncLoad
    public class CCTextureCache : CCObject
    {
        private readonly object m_pDictLock;
        protected Dictionary<string, CCTexture2D> m_pTextures;

        #region Singleton

        private static CCTextureCache s_sharedTextureCache;

        private CCTextureCache()
        {
            Debug.Assert(s_sharedTextureCache == null, "Attempted to allocate a second instance of a singleton.");

            m_pTextures = new Dictionary<string, CCTexture2D>();

            m_pDictLock = new object();
        }

        public static CCTextureCache SharedTextureCache
        {
            get { return s_sharedTextureCache ?? (s_sharedTextureCache = new CCTextureCache()); }
        }

        #endregion

        public static void PurgeSharedTextureCache()
        {
            s_sharedTextureCache = null;
        }

        public CCTexture2D AddImage(string fileimage)
        {
            Debug.Assert(!String.IsNullOrEmpty(fileimage), "TextureCache: fileimage MUST not be NULL");

            CCTexture2D texture;

            lock (m_pDictLock)
            {
                //remove possible -HD suffix to prevent caching the same image twice (issue #1040)
                string pathKey = fileimage;
                //CCFileUtils.ccRemoveHDSuffixFromFile(pathKey);

                if (!m_pTextures.TryGetValue(pathKey, out texture))
                {
                    var textureXna = CCApplication.SharedApplication.Content.Load<Texture2D>(fileimage);

                    texture = new CCTexture2D();
                    bool isInited = texture.InitWithTexture(textureXna);

                    if (isInited)
                    {
                        m_pTextures.Add(pathKey, texture);
                    }
                    else
                    {
                        Debug.Assert(false, "cocos2d: Couldn't add image:" + fileimage + " in CCTextureCache");
                        return null;
                    }
                }
            }
            return texture;
        }

        public CCTexture2D TextureForKey(string key)
        {
            CCTexture2D texture = null;

            try
            {
                m_pTextures.TryGetValue(key, out texture);
            }
            catch (ArgumentNullException)
            {
                CCLog.Log("Texture of key {0} is not exist.", key);
            }

            return texture;
        }

        public void RemoveAllTextures()
        {
            m_pTextures.Clear();
        }

        public void RemoveUnusedTextures()
        {
            if (m_pTextures.Count > 0)
            {
                var tmp = new Dictionary<string, WeakReference>();

                foreach (var pair in m_pTextures)
                {
                    tmp.Add(pair.Key, new WeakReference(pair.Value));
                }

                m_pTextures.Clear();

                GC.Collect();

                foreach (var pair in tmp)
                {
                    if (pair.Value.IsAlive)
                    {
                        m_pTextures.Add(pair.Key, (CCTexture2D) pair.Value.Target);
                    }
                }
            }
        }

        public void RemoveTexture(CCTexture2D texture)
        {
            if (texture == null)
            {
                return;
            }

            string key = null;

            foreach (var pair in m_pTextures)
            {
                if (pair.Value == texture)
                {
                    key = pair.Key;
                    break;
                }
            }

            if (key != null)
            {
                m_pTextures.Remove(key);
            }
        }

        public void RemoveTextureForKey(string textureKeyName)
        {
            if (textureKeyName == null)
            {
                return;
            }
            m_pTextures.Remove(textureKeyName);
        }

        public void DumpCachedTextureInfo()
        {
            int count = 0;
            int total = 0;
            foreach(var pair in m_pTextures) // invalid state exception
            {
                var texture = pair.Value.Texture2D;
                var bytes = texture.Width * texture.Height * 4;
                
                count++;
                total += bytes;

                CCLog.Log("{0} {1} x {2} => {3} KB.",
                    pair.Key,
                    pair.Value.Texture2D.Width,
                    pair.Value.Texture2D.Height,
                    bytes / 1024
                    );
            }

            CCLog.Log("{0} textures, for {1} KB ({2:00.00} MB)", count, total / 1024, total / (1024f * 1024f));
        }
    }
}