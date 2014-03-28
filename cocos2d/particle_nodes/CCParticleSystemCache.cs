using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace CocosSharp
{
	public class CCParticleSystemCache : IDisposable, ICCUpdatable
	{
		struct AsyncStruct
		{
			public string FileName { get; set; }
			public string DirectoryName { get; set; }
			public Action<CCParticleSystemConfig, Action<CCParticleSystemConfig>> OnLoad { get; set; }
			public Action<CCParticleSystemConfig> Action { get; set; }
		};

		private List<AsyncStruct> asyncLoadedConfigs = new List<AsyncStruct>();
		private Action ProcessingAction { get; set; }
		private object Task { get; set; }

		private static CCParticleSystemCache sharedParticleSystemCache;

		private readonly object dictLock = new object();
		protected Dictionary<string, CCParticleSystemConfig> psConfigs = new Dictionary<string, CCParticleSystemConfig>();


		private CCParticleSystemCache()
		{
			ProcessingAction = new Action(
				() =>
				{
					while (true)
					{
						AsyncStruct psConfig;

						lock (asyncLoadedConfigs)
						{
							if (asyncLoadedConfigs.Count == 0)
							{
								Task = null;
								return;
							}
							psConfig = asyncLoadedConfigs[0];
							asyncLoadedConfigs.RemoveAt(0);
						}

						try
						{
							var config = AddParticleSystem(psConfig.FileName, psConfig.DirectoryName, true);
							CCLog.Log("Loaded particle system: {0}", psConfig.FileName);
							if (psConfig.OnLoad != null)
							{
								CCDirector.SharedDirector.Scheduler.Schedule (
									f => psConfig.OnLoad(config, psConfig.Action), this, 0, 0, 0, false
								);
							}
						}
						catch (Exception ex)
						{
							CCLog.Log("Failed to load particle system {0}", psConfig.FileName);
							CCLog.Log(ex.ToString());
						}
					}
				}
			);
		}

		public void Update(float dt)
		{
		}

		public static CCParticleSystemCache SharedParticleSystemCache
		{
			get 
			{
				if (sharedParticleSystemCache == null)
				{
					sharedParticleSystemCache = new CCParticleSystemCache();
				}
				return (sharedParticleSystemCache);
			}
		}

		public static void PurgeSharedConfigCache()
		{
			if (sharedParticleSystemCache != null)
			{
				sharedParticleSystemCache.Dispose();
				sharedParticleSystemCache = null;
			}
		}

		public void UnloadContent()
		{
			psConfigs.Clear();
		}

		public bool Contains(string assetFile)
		{
			return psConfigs.ContainsKey(assetFile);
		}

		public void AddParticleSystemAsync(string fileConfig, Action<CCParticleSystemConfig> action, string directoryName = null)
		{
			Debug.Assert(!String.IsNullOrEmpty(fileConfig), "ParticleSystemConfigCache: fileConfig MUST not be NULL");

			lock (asyncLoadedConfigs)
			{
				asyncLoadedConfigs.Add(new AsyncStruct() {FileName = fileConfig, Action = action, OnLoad = OnConfigLoad});
			}

			if (Task == null)
			{
				Task = CCTask.RunAsync(ProcessingAction);
			}
		}

		void OnConfigLoad (CCParticleSystemConfig config, Action<CCParticleSystemConfig> action)
		{

			// Right now Mac can not load images with data defined asyncly so we want to short circuit this and 
			// just load the data from disk.  If not then we will perform an async load on the image
#if MACOS
			config.LoadParticleTexture ();
			if (action != null)
				action (config);
#else


			CCTexture2D tex = null;

			string textureData = config.TextureData;

			// We will try loading the textur data first if it exists.  Hopefully we get lucky
			if (!string.IsNullOrEmpty(textureData))
			{
				//Debug.Assert(!string.IsNullOrEmpty(textureData),
				//    string.Format("CCParticleSystem: textureData does not exist : {0}", textureName));

				int dataLen = textureData.Length;
				if (dataLen != 0)
				{

					var dataBytes = Convert.FromBase64String(textureData);
					Debug.Assert(dataBytes != null,
						string.Format("CCParticleSystem: error decoding textureImageData : {0}", config.TextureName));

					var imageBytes = CCParticleSystemConfig.Inflate(dataBytes);
					Debug.Assert(imageBytes != null,
						string.Format("CCParticleSystem: error init image with Data for texture : {0}", config.TextureName));

					try
					{

                        CCTextureCache.SharedTextureCache.AddImageAsync(imageBytes, config.TextureName, CCSurfaceFormat.Color, (loadedTexture) =>
							{
                                if (loadedTexture != null)
                                {
                                    config.Texture = loadedTexture;
                                    if (action != null)
                                        action(config);
                                }
                                else 
                                {
                                    if (!string.IsNullOrEmpty(config.TextureName))
                                    {
                                        bool bNotify = CCFileUtils.IsPopupNotify;
                                        CCFileUtils.IsPopupNotify = false;
                                        try
                                        {
                                            CCTextureCache.SharedTextureCache.AddImageAsync(config.TextureName, (tex2) =>
                                                {
                                                    config.Texture = tex;
                                                    if (action != null)
                                                        action(config);
                                                });
                                        }
                                        catch (Exception)
                                        {
                                            tex = null;
                                            config.Texture = CCParticleExample.DefaultTexture;
                                        }

                                        CCFileUtils.IsPopupNotify = bNotify;
                                        if (config.Texture == null)
                                            config.Texture = CCParticleExample.DefaultTexture;
                                    }

                                }

							}
						);
					}
					catch (Exception ex)
					{
						CCLog.Log(ex.ToString());
                        config.Texture = CCParticleExample.DefaultTexture;

					}

				}
			}
#endif
		}

		public CCParticleSystemConfig AddParticleSystem(string fileConfig, string directoryName = null)
		{
			return AddParticleSystem (fileConfig, directoryName, false);
		}

		CCParticleSystemConfig AddParticleSystem(string fileConfig, string directoryName, bool loadAsync)
		{
			Debug.Assert (!String.IsNullOrEmpty (fileConfig), "ParticleSystemConfigCache: fileConfig MUST not be NULL");

			CCParticleSystemConfig psConfig = null;

			var assetName = fileConfig;
			if (Path.HasExtension (assetName)) {
				assetName = CCFileUtils.RemoveExtension (assetName);
			}

			lock (dictLock) {
				psConfigs.TryGetValue (assetName, out psConfig);
			}
			if (psConfig == null) {
				psConfig = new CCParticleSystemConfig (fileConfig, directoryName, loadAsync);

				if (psConfig != null) {
					lock (dictLock) {
						psConfigs[assetName] = psConfig;
					}
				} else {
					return null;
				}
			}

			return psConfig;
		}


		public CCParticleSystemConfig this[string key]
		{
			get 
			{
				return ParticleSystemForKey (key);
			}
		}

		public CCParticleSystemConfig ParticleSystemForKey(string key)
		{
			CCParticleSystemConfig config = null;
			try
			{
				if (Path.HasExtension(key))
				{
					key = CCFileUtils.RemoveExtension(key);
				}

				psConfigs.TryGetValue(key, out config);
			}
			catch (ArgumentNullException)
			{
				CCLog.Log("Particle System with key {0} does not exist.", key);
			}

			return config;
		}

		public void RemoveAll()
		{
			psConfigs.Clear();
		}

		public void RemoveUnused()
		{
			if (psConfigs.Count > 0)
			{
				var tmp = new Dictionary<string, WeakReference>();

				foreach (var pair in psConfigs)
				{
					tmp.Add(pair.Key, new WeakReference(pair.Value));
				}

				psConfigs.Clear();

				GC.Collect();

				foreach (var pair in tmp)
				{
					if (pair.Value.IsAlive)
					{
						psConfigs.Add(pair.Key, (CCParticleSystemConfig) pair.Value.Target);
					}
				}
			}
		}

		public void Remove (CCParticleSystemConfig particleSystem)
		{
			if (particleSystem == null)
			{
				return;
			}

			string key = null;

			foreach (var pair in psConfigs)
			{
				if (pair.Value == particleSystem)
				{
					key = pair.Key;
					break;
				}
			}

			if (key != null)
			{
				psConfigs.Remove(key);
			}
		}

		public void RemoveForKey(string particleSystemKeyName)
		{
			if (String.IsNullOrEmpty(particleSystemKeyName))
			{
				return;
			}

			if (Path.HasExtension(particleSystemKeyName))
			{
				particleSystemKeyName = CCFileUtils.RemoveExtension(particleSystemKeyName);
			}

			psConfigs.Remove(particleSystemKeyName);
		}

		public void DumpCachedInfo()
		{
			int count = 0;
			int total = 0;

			var copy = psConfigs.ToList();

			foreach (var pair in copy)
			{
				var texture = pair.Value.Texture.XNATexture;

				if (texture != null)
				{
					var bytes = texture.Width * texture.Height * 4;
					CCLog.Log("{0} {1} x {2} => {3} KB.", pair.Key, texture.Width, texture.Height, bytes / 1024);
					total += bytes;
				}

				count++;
			}
			CCLog.Log("{0} particle systems, for {1} KB ({2:00.00} MB)", count, total / 1024, total / (1024f * 1024f));
		}

		#region Cleaning up

		// No unmanaged resources, so no need for finalizer

		public void Dispose()
		{
			this.Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && psConfigs != null)
			{
				foreach (var t in psConfigs.Values)
				{
					t.Dispose();
				}

				psConfigs = null;
			}
		}

		#endregion Cleaning up
	}
}