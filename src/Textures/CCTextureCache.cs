using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CocosSharp
{
    public class CCTextureCache : IDisposable, ICCUpdatable
    {
        #region Structs

        struct AsyncStruct
        {
            public string  FileName { get; set; }
            public Action<CCTexture2D> Action { get; set; }
        }

        struct DataAsyncStruct
        {
            public byte[] Data { get; set; }
            public string AssetName { get; set; }
            public CCSurfaceFormat Format { get; set; }
            public Action<CCTexture2D> Action { get; set; }
        }

        #endregion Structs

        static CCTextureCache sharedTextureCache;

        List<AsyncStruct> asyncLoadedImages = new List<AsyncStruct>();
        List<DataAsyncStruct> dataAsyncLoadedImages = new List<DataAsyncStruct>();

        readonly object dictLock = new object();
        protected Dictionary<string, CCTexture2D> textures = new Dictionary<string, CCTexture2D>();


        #region Properties

        public static CCTextureCache SharedTextureCache
        {
            get 
            {
                if (sharedTextureCache == null) 
                {
                    sharedTextureCache = new CCTextureCache(new CCScheduler ());
                }

                return sharedTextureCache;
            }
        }

        Action ProcessingAction { get; set; }
        Action ProcessingDataAction { get; set; }
        object Task { get; set; }

        CCScheduler Scheduler { get; set; }

        public CCTexture2D this[string key]
        {
            get 
            {
                CCTexture2D texture = null;
                try
                {
                    if (Path.HasExtension(key))
                    {
                        key = CCFileUtils.RemoveExtension(key);
                    }

                    textures.TryGetValue(key, out texture);
                }
                catch (ArgumentNullException)
                {
                    CCLog.Log("Texture of key {0} is not exist.", key);
                }

                return texture;
            }
        }

        #endregion Properties


        #region Constructors

        public CCTextureCache() : this(CCScheduler.SharedScheduler)
        {
            sharedTextureCache = this;
        }

        CCTextureCache(CCScheduler scheduler)
        {
            Scheduler = scheduler;

            ProcessingAction = new Action(
                () =>
                {
                    while (true)
                    {

                        AsyncStruct image;

                        lock (asyncLoadedImages)
                        {
                            if (asyncLoadedImages.Count == 0)
                            {
                                Task = null;
                                return;
                            }
                            image = asyncLoadedImages[0];
                            asyncLoadedImages.RemoveAt(0);
                        }

                        try
                        {
                            var texture = AddImage(image.FileName);
                            CCLog.Log("Loaded texture: {0}", image.FileName);
                            if (image.Action != null)
                            {
                                Scheduler.Schedule (
                                    f => image.Action(texture), this, 0, 0, 0, false
                                );
                            }
                        }
                        catch (Exception ex)
                        {
                            CCLog.Log("Failed to load image {0}", image.FileName);
                            CCLog.Log(ex.ToString());
                        }
                    }
                }
            );
            ProcessingDataAction = new Action(
                () =>
                {
                    while (true)
                    {

                        DataAsyncStruct imageData;

                        lock (dataAsyncLoadedImages)
                        {
                            if (dataAsyncLoadedImages.Count == 0)
                            {
                                Task = null;
                                return;
                            }
                            imageData = dataAsyncLoadedImages[0];
                            dataAsyncLoadedImages.RemoveAt(0);
                        }

                        try
                        {
                            var texture = AddImage(imageData.Data, imageData.AssetName, imageData.Format);
                            CCLog.Log("Loaded texture: {0}", imageData.AssetName);
                            if (imageData.Action != null)
                            {
                                Scheduler.Schedule (
                                    f => imageData.Action(texture), this, 0, 0, 0, false
                                );
                            }
                        }
                        catch (Exception ex)
                        {
                            CCLog.Log("Failed to load image {0}", imageData.AssetName);
                            CCLog.Log(ex.ToString());
                        }
                    }
                }
            );
        }

        #endregion Constructors

        public void Update(float dt)
        {
        }

        public bool Contains(string assetFile)
        {
            return textures.ContainsKey(assetFile);
        }


        #region Cleaning up

        public static void PurgeSharedTextureCache()
        {
            if(sharedTextureCache != null) 
            {
                sharedTextureCache.Dispose();
                sharedTextureCache = null;
            }
        }

        // No unmanaged resources, so no need for finalizer
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && textures != null)
            {
                foreach (CCTexture2D t in textures.Values)
                {
                    t.Dispose();
                }

                textures = null;
            }
        }

        public void UnloadContent()
        {
            textures.Clear();
        }

        public void RemoveAllTextures()
        {
            textures.Clear();
        }

        public void RemoveUnusedTextures()
        {
            if (textures.Count > 0)
            {
                var tmp = new Dictionary<string, WeakReference>();

                foreach (var pair in textures)
                {
                    tmp.Add(pair.Key, new WeakReference(pair.Value));
                }

                textures.Clear();

                GC.Collect();

                foreach (var pair in tmp)
                {
                    if (pair.Value.IsAlive)
                    {
                        textures.Add(pair.Key, (CCTexture2D) pair.Value.Target);
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

            foreach (var pair in textures)
            {
                if (pair.Value == texture)
                {
                    key = pair.Key;
                    break;
                }
            }

            if (key != null)
            {
                textures.Remove(key);
            }
        }

        public void RemoveTextureForKey(string textureKeyName)
        {
            if (String.IsNullOrEmpty(textureKeyName))
            {
                return;
            }

            if (Path.HasExtension(textureKeyName))
            {
                textureKeyName = CCFileUtils.RemoveExtension(textureKeyName);
            }

            textures.Remove(textureKeyName);
        }

        public void DumpCachedTextureInfo()
        {
            int count = 0;
            int total = 0;

            foreach (var pair in textures)
            {
                var texture = pair.Value.XNATexture;

                if (texture != null)
                {
                    var bytes = texture.Width * texture.Height * 4;
                    CCLog.Log("{0} {1} x {2} => {3} KB.", pair.Key, texture.Width, texture.Height, bytes / 1024);
                    total += bytes;
                }

                count++;
            }
            CCLog.Log("{0} textures, for {1} KB ({2:00.00} MB)", count, total / 1024, total / (1024f * 1024f));
        }

        #endregion Cleaning up


        #region Managing texture images

        public void AddImageAsync(byte[] data, string assetName, CCSurfaceFormat format, Action<CCTexture2D> action)
        {
            Debug.Assert(data != null && data.Length != 0, "TextureCache: data MUST not be NULL and MUST contain data");

            lock (dataAsyncLoadedImages)
            {
                dataAsyncLoadedImages.Add(new DataAsyncStruct() { Data = data, AssetName = assetName, Format = format  , Action = action});
            }

            if (Task == null)
            {
                Task = CCTask.RunAsync(Scheduler, ProcessingDataAction);
            }
        }

        public void AddImageAsync(string fileimage, Action<CCTexture2D> action)
        {
            Debug.Assert(!String.IsNullOrEmpty(fileimage), "TextureCache: fileimage MUST not be NULL");

            lock (asyncLoadedImages)
            {
                asyncLoadedImages.Add(new AsyncStruct() {FileName = fileimage, Action = action});
            }

            if (Task == null)
            {
                Task = CCTask.RunAsync(Scheduler, ProcessingAction);
            }
        }

        public CCTexture2D AddImage(string fileimage)
        {
            Debug.Assert (!String.IsNullOrEmpty (fileimage), "TextureCache: fileimage MUST not be NULL");

            CCTexture2D texture = null;

            var assetName = fileimage;
            if (Path.HasExtension (assetName)) {
                assetName = CCFileUtils.RemoveExtension (assetName);
            }

            lock (dictLock) {
                textures.TryGetValue (assetName, out texture);
            }
            if (texture == null) {
                texture = new CCTexture2D(fileimage);

                lock(dictLock) 
                {
                    textures[assetName] = texture;
                }
            }

            return texture;
        }

        public CCTexture2D AddImage(byte[] data, string assetName, CCSurfaceFormat format)
        {
            lock (dictLock)
            {
                CCTexture2D texture;

                if (!textures.TryGetValue(assetName, out texture))
                {
                    texture = new CCTexture2D(data, format);
                    textures.Add(assetName, texture);
                }
                return texture;
            }
        }

        public CCTexture2D AddRawImage<T>(T[] data, int width, int height, string assetName, CCSurfaceFormat format, 
            bool premultiplied, bool mipMap=false) where T : struct
        {
            return AddRawImage(data, width, height, assetName, format, premultiplied, mipMap, new CCSize(width, height));
        }

        public CCTexture2D AddRawImage<T>(T[] data, int width, int height, string assetName, CCSurfaceFormat format,
            bool premultiplied, bool mipMap, CCSize contentSize) where T : struct
        {
            CCTexture2D texture;

            lock (dictLock)
            {
                if (!textures.TryGetValue(assetName, out texture))
                {
                    texture = new CCTexture2D();
                    texture.InitWithRawData(data, format, width, height, premultiplied, mipMap, contentSize);
                    textures.Add(assetName, texture);
                }
            }
            return texture;
        }

        #endregion Managing texture images
    }
}