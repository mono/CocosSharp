using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Cocos2D
{
    public class CCContentManager : ContentManager
    {
        public static CCContentManager SharedContentManager;

        internal static void Initialize(IServiceProvider serviceProvider, string rootDirectory)
        {
            SharedContentManager = new CCContentManager(serviceProvider, rootDirectory);
        }

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

        private Dictionary<string, string> _assetPathCache = new Dictionary<string, string>();

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
            try
            {
                return Load<T>(assetName, weakReference);
            }
            catch (Exception)
            {
                _assetPathCache[assetName] = null;
                
                return default(T);
            }
        }

        public override T Load<T>(string assetName)
        {
            return Load<T>(assetName, false);
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
                if (typeof(T) == typeof(string))
                {
                    //TODO: No need CCContent, use TileContainer
                    var content = ReadAsset<CCContent>(path, null);
                    result = (T)(object)content.Content;
                    
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

                if (typeof(T) == typeof(PlistDocument))
                {
					string assetPath = Path.Combine(RootDirectory, path);
                    
                    using (var streamContent = TitleContainer.OpenStream(assetPath))
                    {
                        result = (T) (object) new PlistDocument(streamContent);
                    }

                    useContentReader = false;
                }
#if XNA
                else if (typeof(T) == typeof(Texture2D) && Path.HasExtension(path))
                {
                    string assetPath = Path.Combine(RootDirectory, path);

                    var servece = (IGraphicsDeviceService)ServiceProvider.GetService(typeof(IGraphicsDeviceService));

                    using (var streamContent = TitleContainer.OpenStream(assetPath))
                    {
                        result = (T)(object)Texture2D.FromStream(servece.GraphicsDevice, streamContent);
                    }
                    useContentReader = false;
                }
#endif
                else
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
                    var path = Path.Combine(Path.Combine(searchPath, resolutionOrder), realName);

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
