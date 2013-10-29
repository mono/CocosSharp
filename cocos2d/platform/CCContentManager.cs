using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Cocos2D
{
    public class CCContentManager : ContentManager
    {
        public static CCContentManager SharedContentManager;

        internal static void Initialize(IServiceProvider serviceProvider, string rootDirectory)
        {
            SharedContentManager = new CCContentManager(serviceProvider, rootDirectory);
#if IOS || WINDOWS_PHONE8
            InitializeContentTypeReaders();
#endif
        }

#if IOS || WINDOWS_PHONE8
        private static bool s_readersInited;

        private static void InitializeContentTypeReaders()
        {
            // Please read the following discussions for the reasons of this.
            // http://monogame.codeplex.com/discussions/393775
            // http://monogame.codeplex.com/discussions/396792
            // 
            // https://github.com/mono/MonoGame/pull/726
            //
            // Also search Google for -> ContentTypeReaderManager.AddTypeCreator

            if (s_readersInited)
            {
                return;
            }

            // .FNT Reader
            ContentTypeReaderManager.AddTypeCreator(
                "Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[Cocos2D.CCBMFontConfiguration+CCBMFontDef, cocos2d-xna, Version=2.0.3.0, Culture=neutral, PublicKeyToken=null]]",
                () => new DictionaryReader<Int32, CCBMFontConfiguration.CCBMFontDef>()

                );

            ContentTypeReaderManager.AddTypeCreator(
                "Microsoft.Xna.Framework.Content.DictionaryReader`2[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[Cocos2D.CCBMFontConfiguration+CCKerningHashElement, cocos2d-xna, Version=2.0.3.0, Culture=neutral, PublicKeyToken=null]]",
                () => new DictionaryReader<Int32, CCBMFontConfiguration.CCKerningHashElement>()

                );
            ContentTypeReaderManager.AddTypeCreator(
                "Microsoft.Xna.Framework.Content.ReflectiveReader`1[[Cocos2D.CCRect, cocos2d-xna, Version=2.0.3.0, Culture=neutral, PublicKeyToken=null]]",
                () => new CCRectReader()

                );

            ContentTypeReaderManager.AddTypeCreator(
                "Microsoft.Xna.Framework.Content.ReflectiveReader`1[[Cocos2D.CCPoint, cocos2d-xna, Version=2.0.3.0, Culture=neutral, PublicKeyToken=null]]",
                () => new CCPointReader()

                );
            ContentTypeReaderManager.AddTypeCreator(
                "Microsoft.Xna.Framework.Content.ReflectiveReader`1[[Cocos2D.CCSize, cocos2d-xna, Version=2.0.3.0, Culture=neutral, PublicKeyToken=null]]",
                () => new CCSizeReader()

                );

            ContentTypeReaderManager.AddTypeCreator(
                "Microsoft.Xna.Framework.Content.ReflectiveReader`1[[Cocos2D.CCBMFontConfiguration+CCKerningHashElement, cocos2d-xna, Version=2.0.3.0, Culture=neutral, PublicKeyToken=null]]",
                () => new KerningHashElementReader()

                );

            ContentTypeReaderManager.AddTypeCreator(
                "Microsoft.Xna.Framework.Content.ReflectiveReader`1[[Cocos2D.CCBMFontConfiguration+CCBMFontPadding, cocos2d-xna, Version=2.0.3.0, Culture=neutral, PublicKeyToken=null]]",
                () => new CCBMFontPaddingtReader()

                );

            s_readersInited = true;
        }
#endif

        private class AssetEntry
        {
            public readonly string AssetFileName;
            public readonly bool WeakReference;
            public readonly bool UseContentReader;
            private object _asset;

            public AssetEntry(object asset, string assetFileName, bool weakReference, bool useContentReader)
            {
                AssetFileName = assetFileName;
                WeakReference = weakReference;
                UseContentReader = useContentReader;
                Asset = asset;
            }

            public object Asset
            {
                set
                {
                    if (WeakReference)
                    {
                        _asset = new WeakReference(value);
                    }
                    else
                    {
                        _asset = value;
                    }
                }
                get
                {
                    if (WeakReference)
                    {
                        if (((WeakReference)_asset).IsAlive)
                        {
                            return ((WeakReference)_asset).Target;
                        }
                        return null;
                    }
                    else
                    {
                        return _asset;
                    }
                }
            }
        }

        private Dictionary<string, AssetEntry> _loadedAssets;
        
        private Dictionary<string, string> _assetLookupDict = new Dictionary<string, string>();
        private List<string> _searchPaths = new List<string>();
        private List<string> _searchResolutionsOrder = new List<string>(); 

        private Dictionary<string, string> _failedAssets = new Dictionary<string, string>();

        public CCContentManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _loadedAssets = new Dictionary<string, AssetEntry>();
        }

        public CCContentManager(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory)
        {
            _loadedAssets = new Dictionary<string, AssetEntry>();
        }

        public T TryLoad<T>(string assetName)
        {
            return TryLoad<T>(assetName, false);
        }

        public T TryLoad<T>(string assetName, bool weakReference)
        {
            if (_failedAssets.ContainsKey(assetName))
            {
                return default(T);
            }

            try
            {
                return Load<T>(assetName, weakReference);
            }
            catch (Exception)
            {
                _failedAssets[assetName] = null;
                
                return default(T);
            }
        }

        public override T Load<T>(string assetName)
        {
            if (_failedAssets.ContainsKey(assetName))
            {
                throw new ContentLoadException("Failed to load the asset file from " + assetName);
            }
                
            try
            {
                return Load<T>(assetName, false);
            }
            catch (Exception)
            {
                _failedAssets[assetName] = null;

                throw;
            }
        }

        public T Load<T>(string assetName, bool weakReference)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new ArgumentNullException("assetName");
            }

            // Check for a previously loaded asset first
            AssetEntry entry;
            if (_loadedAssets.TryGetValue(assetName, out entry))
            {
                if (entry.Asset is T)
                {
                    return (T)entry.Asset;
                }
                else
                {
                    return InternalLoad<T>(assetName, entry.AssetFileName, weakReference);
                }
            }

            var realName = GetRealName(assetName);

            CheckDefaultPath(_searchPaths);
            CheckDefaultPath(_searchResolutionsOrder);

            foreach (var searchPath in _searchPaths)
            {
                foreach (string resolutionOrder  in _searchResolutionsOrder)
                {
                    var path = Path.Combine(Path.Combine(searchPath, resolutionOrder), realName);

                    try
                    {
                        //TODO: for platforms with access to the file system, first check for the existence of the file 
                        return InternalLoad<T>(assetName, path, weakReference);
                    }
                    catch (ContentLoadException)
                    {
                        // try other path
                    }
                }
            }

            throw new ContentLoadException("Failed to load the asset file from " + assetName);
        }

        private string GetRealName(string assetName)
        {
            if (_assetLookupDict.ContainsKey(assetName))
            {
                return _assetLookupDict[assetName];
            }
            return assetName;
        }

        public override void Unload()
        {
            base.Unload();

            _loadedAssets.Clear();
        }

        private T InternalLoad<T>(string assetName, string path, bool weakReference)
        {
            T result = default(T);

            bool useContentReader = true;

            try
            {
                //Special types
                if (typeof (T) == typeof (string))
                {
                    //TODO: No need CCContent, use TileContainer
                    var content = ReadAsset<CCContent>(path, null);
                    result = (T) (object) content.Content;

                    useContentReader = false;
                }
                else
                {
                    // Load the asset.
                    result = ReadAsset<T>(path, null);
                }
            }
            catch (ContentLoadException)
            {
                try
                {
                    string assetPath = Path.Combine(RootDirectory, path);

                    if (typeof(T) == typeof(string))
                    {
                        using (var streamContent = TitleContainer.OpenStream(assetPath))
                        {
                            using (StreamReader reader = new StreamReader(streamContent, Encoding.UTF8))
                            {
                                result = (T)(object) reader.ReadToEnd();
                            }
                        }
                        useContentReader = false;
                    }
                    else if (typeof(T) == typeof(PlistDocument))
                    {
                        try
                        {
                            using (var streamContent = TitleContainer.OpenStream(assetPath))
                            {
                                result = (T) (object) new PlistDocument(streamContent);
                            }
                        }
                        catch (Exception)
                        {
                            //TODO: Remove This
                            var content = ReadAsset<CCContent>(path, null);
                            result = (T) (object) new PlistDocument(content.Content);
                        }

                        useContentReader = false;
                    }
#if XNA
                    else if (typeof (T) == typeof (Texture2D) && Path.HasExtension(path))
                    {
                        var service =
                            (IGraphicsDeviceService) ServiceProvider.GetService(typeof (IGraphicsDeviceService));

                        using (var streamContent = TitleContainer.OpenStream(assetPath))
                        {
                            result = (T) (object) Texture2D.FromStream(service.GraphicsDevice, streamContent);
                        }
                        useContentReader = false;
                    }
#endif
                    else
                    {
                        throw;
                    }
                }
                catch (Exception)
                {

                    throw new ContentLoadException("Failed to load the asset file from " + assetName);
                }
            }

            var assetEntry = new AssetEntry(result, path, weakReference, useContentReader);

            _loadedAssets[assetName] = assetEntry;

            if (result is GraphicsResource)
            {
                (result as GraphicsResource).Disposing += AssetDisposing;
            }

            return result;
        }

        private void AssetDisposing(object sender, EventArgs e)
        {
            foreach (var loadedAsset in _loadedAssets)
            {
                if (loadedAsset.Value.Asset == sender)
                {
                    _loadedAssets.Remove(loadedAsset.Key);
                    return;
                }
            }
        }

#if MONOGAME
        protected override void ReloadGraphicsAssets()
        {
            foreach (var pair in _loadedAssets)
            {
                if (pair.Value.UseContentReader && pair.Value.Asset != null)
                {
                    LoadedAssets.Add(pair.Value.AssetFileName, pair.Value.Asset);
                }
            }

            base.ReloadGraphicsAssets();

            foreach (var pair in LoadedAssets)
            {
                foreach (var pair2 in _loadedAssets)
                {
                    if (pair2.Value.AssetFileName == pair.Key)
                    {
                        _loadedAssets[pair2.Key].Asset = pair.Value;
                    }
                }
            }
            
            LoadedAssets.Clear();
        }
#else
        public void ReloadGraphicsAssets()
        {
            foreach (var asset in _loadedAssets)
            {
                asset.Value.Asset = null;
            }
        }
#endif

        public Stream GetAssetStream(string assetName)
        {
            var realName = GetRealName(assetName);

            CheckDefaultPath(_searchPaths);
            CheckDefaultPath(_searchResolutionsOrder);

            foreach (var searchPath in _searchPaths)
            {
                foreach (string resolutionOrder in _searchResolutionsOrder)
                {
                    var path = Path.Combine(Path.Combine(RootDirectory, Path.Combine(searchPath, resolutionOrder)), realName);

                    try
                    {
                        //TODO: for platforms with access to the file system, first check for the existence of the file 
                        return TitleContainer.OpenStream(path);
                    }
                    catch (Exception)
                    {
                        // try other path
                    }
                }
            }

            throw new ContentLoadException("Failed to load the asset stream from " + assetName);
        }

        public virtual void LoadFilenameLookupDictionaryFromFile(string filename)
        {
            var document = Load<PlistDocument>(filename, true);
            
            SetFilenameLookupDictionary(document.Root as PlistDictionary);
        }

        public virtual void SetFilenameLookupDictionary(PlistDictionary filenameLookupDict)
        {
            //TODO: Load lookup names from PlistDictionary
        }

        public List<string> SearchResolutionsOrder
        {
            get { return _searchResolutionsOrder; }
        }

        public List<string> SearchPaths
        {
            get { return _searchPaths; }
        }

        private void CheckDefaultPath(List<string> paths)
        {
            for (int i = paths.Count - 1; i >= 0; i--)
            {
                if (paths[i] == "")
                {
                    return;
                }
            }

            paths.Add("");
        }
    }
}
