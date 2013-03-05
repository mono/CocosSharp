using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace cocos2d
{
    //TODO: AsyncLoad
    public class CCTextureCache : CCObject, IDisposable
    {
        private readonly object m_pDictLock;

        protected Dictionary<string, CCTexture2D> m_pTextures = new Dictionary<string,CCTexture2D>();
        private static Dictionary<CCTexture2D, string> m_pTextureRefNames = new Dictionary<CCTexture2D, string>();

        #region Singleton

        private static CCTextureCache s_sharedTextureCache;

        ~CCTextureCache() 
        {
            Dispose();
        }

        private CCTextureCache()
        {
            Debug.Assert(s_sharedTextureCache == null, "Attempted to allocate a second instance of a singleton.");

            m_pTextures = new Dictionary<string, CCTexture2D>();

            m_pDictLock = new object();
        }

        public static CCTextureCache SharedTextureCache
        {
            get 
            {
                if (s_sharedTextureCache == null)
                {
                    s_sharedTextureCache = new CCTextureCache();
                }
                else
                {
                    s_sharedTextureCache.Restore();
                }
                return (s_sharedTextureCache);
            }
        }

        #endregion

        public static void PurgeSharedTextureCache()
        {
//            s_sharedTextureCache = null;
            if (s_sharedTextureCache != null)
            {
                s_sharedTextureCache.Dispose();
            }
        }

        public void UnloadContent()
        {
            m_pTextures.Clear();
            m_pTextureRefNames.Clear();
        }

        /// <summary>
        /// Restores all of the textures in the cache - used when the game is resurrected on Android.
        /// </summary>
        public void Restore()
        {
            if (m_pTextureRefNames.Count > 0)
            {
//                CCLog.Log("Restoring the texture cache!");
                List<string> l = new List<string>(m_pTextureRefNames.Values);
                m_pTextureRefNames.Clear();
                foreach (string name in l)
                {
//                    CCLog.Log("Restoring: " + name);
                    AddImage(name);
                }
            }
        }

        public void ReloadMyTexture(CCTexture2D t)
        {
            if (m_pTextureRefNames.ContainsKey(t))
            {
                string file = m_pTextureRefNames[t];
                AddImage(file);
            }
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

                bool bHasTexture = m_pTextures.TryGetValue(pathKey, out texture);
                if(!bHasTexture || texture == null) 
                {
                    // Create a new one only if the current one does not exist
                    texture = new CCTexture2D();
                }
                if(!texture.IsTextureDefined)
                {
                    // Either we are creating a new one or else we need to refresh the current one.
                    // CCLog.Log("Loading texture {0}", fileimage);

                    var textureXna = CCApplication.SharedApplication.Content.Load<Texture2D>(fileimage);

                    bool isInited = texture.InitWithTexture(textureXna);

                    if (isInited)
                    {
                        texture.IsManaged = true;
                        texture.ContentFile = fileimage;
                        m_pTextures[pathKey] = texture;
                        m_pTextureRefNames[texture] = fileimage;
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

        public CCTexture2D AddImage(Stream imageStream, string assetName)
        {
            Debug.Assert(imageStream == null, "TextureCache: imageStream MUST not be NULL");
            
            CCTexture2D texture;

            lock (m_pDictLock)
            {
                string pathKey = assetName;

                bool bHasTexture = m_pTextures.TryGetValue(pathKey, out texture);
                if(!bHasTexture || texture == null) 
                {
                    // Create a new one only if the current one does not exist
                    texture = new CCTexture2D();
                }
                if(!texture.IsTextureDefined)
                {
                    // Either we are creating a new one or else we need to refresh the current one.
                    // CCLog.Log("Loading texture {0}", fileimage);
                    
                    var graphicsDeviceService = CCApplication.SharedApplication.Content.ServiceProvider.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
                    if (graphicsDeviceService == null)
                    {
                        throw new InvalidOperationException("No Graphics Device Service");
                    }
                    var textureXna = Texture2D.FromStream(graphicsDeviceService.GraphicsDevice, imageStream);

                    bool isInited = texture.InitWithTexture(textureXna);
                    
                    if (isInited)
                    {
                        texture.IsManaged = true;
                        texture.ContentFile = assetName;
                        m_pTextures[pathKey] = texture;
                        m_pTextureRefNames[texture] = assetName;
                    }
                    else
                    {
                        Debug.Assert(false, "cocos2d: Couldn't add image:" + assetName + " in CCTextureCache");
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

        #region IDisposable Members

        public void Dispose()
        {
            if (m_pTextures != null)
            {
                foreach (CCTexture2D t in m_pTextures.Values)
                {
                    if (!t.IsManaged && (t is IDisposable))
                    {
                        // Only dispose objects that are not managed by the content manager.
                        ((IDisposable)t).Dispose();
        }
                    // Try to force GC collection of the texture because the ref name
                    // collection also holds on to this texture reference and prevents the texture2d from
                    // being collected.
                    t.texture2D = null;
                }
                // m_pTextures.Clear();
            }
        }

        #endregion
    }
}