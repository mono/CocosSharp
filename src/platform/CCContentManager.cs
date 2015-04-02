using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CocosSharp
{
	public class CCContentManager : ContentManager
    {
		#region Asset entry private class

		class AssetEntry
		{
			public readonly string AssetFileName;
			public readonly bool WeakReference;
			public readonly bool UseContentReader;
			object asset;


			#region Properties

			public object Asset
			{
				get
				{
					if (WeakReference)
					{
						if (((WeakReference)asset).IsAlive)
						{
							return ((WeakReference)asset).Target;
						}
						return null;
					}
					else
					{
						return asset;
					}
				}

				set
				{
					if (WeakReference)
					{
						asset = new WeakReference(value);
					}
					else
					{
						asset = value;
					}
				}
			}

			#endregion Properties


			#region Constructors

			public AssetEntry(object asset, string assetFileName, bool weakReference, bool useContentReader)
			{
				AssetFileName = assetFileName;
				WeakReference = weakReference;
				UseContentReader = useContentReader;
				Asset = asset;
			}

			#endregion Constructors
		}

		#endregion Asset entry private class


		public static CCContentManager SharedContentManager;

		Dictionary<string, AssetEntry> loadedAssets;
		Dictionary<string, string> assetLookupDict = new Dictionary<string, string>();
		Dictionary<Tuple<string, Type>, string> failedAssets = new Dictionary<Tuple<string, Type>, string>();

		List<string> searchPaths = new List<string>();
		List<string> searchResolutionsOrder = new List<string>(); 


		#region Constructors

		public CCContentManager(IServiceProvider serviceProvider) : base(serviceProvider)
		{
			loadedAssets = new Dictionary<string, AssetEntry>();
		}

		public CCContentManager(IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory)
		{
			loadedAssets = new Dictionary<string, AssetEntry>();
		}

		#endregion Constructors


        internal static void Initialize(IServiceProvider serviceProvider, string rootDirectory)
        {
            SharedContentManager = new CCContentManager(serviceProvider, rootDirectory);
        }

		string GetRealName(string assetName)
		{
			if (assetLookupDict.ContainsKey(assetName))
			{
				return assetLookupDict[assetName];
			}
			return assetName;
		}

		public T TryLoad<T>(string assetName, bool weakReference=false)
        {
            var assetKey = Tuple.Create(assetName, typeof(T));
            if (failedAssets.ContainsKey(assetKey))
            {
                return default(T);
            }

            try
            {
                return Load<T>(assetName, weakReference);
            }
            catch (Exception)
            {
                failedAssets[assetKey] = null;
                
                return default(T);
            }
        }

        public override T Load<T>(string assetName)
        {
            var assetKey = Tuple.Create(assetName, typeof(T));
            if (failedAssets.ContainsKey(assetKey))
            {
                throw new ContentLoadException("Failed to load the asset file from " + assetName);
            }
                
            try
            {
                return Load<T>(assetName, false);
            }
            catch (Exception)
            {
                failedAssets[assetKey] = null;

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
            if (loadedAssets.TryGetValue(assetName, out entry))
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

            CheckDefaultPath(searchPaths);
            CheckDefaultPath(searchResolutionsOrder);

            foreach (var searchPath in searchPaths)
            {
                foreach (string resolutionOrder in searchResolutionsOrder)
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

        public override void Unload()
        {
            base.Unload();

            loadedAssets.Clear();
        }

        T InternalLoad<T>(string assetName, string path, bool weakReference)
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

            loadedAssets[assetName] = assetEntry;

            if (result is GraphicsResource)
            {
                (result as GraphicsResource).Disposing += AssetDisposing;
            }

            return result;
        }

        void AssetDisposing(object sender, EventArgs e)
        {
            foreach (var loadedAsset in loadedAssets)
            {
                if (loadedAsset.Value.Asset == sender)
                {
                    loadedAssets.Remove(loadedAsset.Key);
                    return;
                }
            }
        }

#if MONOGAME
        protected override void ReloadGraphicsAssets()
        {
            foreach (var pair in loadedAssets)
            {
                if (pair.Value.UseContentReader && pair.Value.Asset != null)
                {
                    LoadedAssets.Add(pair.Value.AssetFileName, pair.Value.Asset);
                }
            }

            base.ReloadGraphicsAssets();

            foreach (var pair in LoadedAssets)
            {
                foreach (var pair2 in loadedAssets)
                {
                    if (pair2.Value.AssetFileName == pair.Key)
                    {
                        loadedAssets[pair2.Key].Asset = pair.Value;
                    }
                }
            }
            
            LoadedAssets.Clear();
        }
#else
        public void ReloadGraphicsAssets()
        {
            foreach (var asset in loadedAssets)
            {
                asset.Value.Asset = null;
            }
        }
#endif

        public Stream GetAssetStream(string assetName, out string fileName)
        {
            fileName = string.Empty;
            var realName = GetRealName(assetName);

            CheckDefaultPath(searchPaths);
            CheckDefaultPath(searchResolutionsOrder);

            foreach (var searchPath in searchPaths)
            {
                foreach (string resolutionOrder in searchResolutionsOrder)
                {
                    var path = Path.Combine(Path.Combine(RootDirectory, Path.Combine(searchPath, resolutionOrder)), realName);

                    try
                    {
                        fileName = path;
                        //TODO: for platforms with access to the file system, first check for the existence of the file 
                        return TitleContainer.OpenStream(path);
                    }
                    catch (Exception)
                    {
                        // try other path
                    }
                }
            }

            fileName = string.Empty;
            throw new ContentLoadException("Failed to load the asset stream from " + assetName);
        }

        public Stream GetAssetStream(string assetName)
        {
            var fileName = string.Empty;
            return GetAssetStream(assetName, out fileName);
        }

        public byte[] GetAssetStreamAsBytes(string assetName, out string fileName)
        {
            fileName = string.Empty;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    GetAssetStream(assetName, out fileName).CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch { return null; }
        }

        public byte[] GetAssetStreamAsBytes(string assetName)
        {

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    GetAssetStream(assetName).CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch { return null; }
        }

        public List<string> SearchResolutionsOrder
        {
            get { return searchResolutionsOrder; }

			internal set { searchResolutionsOrder = value; }
        }

        public List<string> SearchPaths
        {
            get { return searchPaths; }

			internal set { searchPaths = value; }
        }

        void CheckDefaultPath(List<string> paths)
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
