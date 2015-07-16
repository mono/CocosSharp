using System;
using System.Collections.Generic;
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

		static CCParticleSystemCache sharedParticleSystemCache;

		readonly object dictLock = new object();

		List<AsyncStruct> asyncLoadedConfigs = new List<AsyncStruct>();
		Dictionary<string, CCParticleSystemConfig> configs;
		Action ProcessingAction { get; set; }

		CCScheduler Scheduler { get; set; }

		#region Properties

		public static CCParticleSystemCache SharedParticleSystemCache
		{
			get
			{
				if (sharedParticleSystemCache == null)
					sharedParticleSystemCache = new CCParticleSystemCache(new CCScheduler());

				return sharedParticleSystemCache;
			}
		}


		object Task { get; set; }

		public CCParticleSystemConfig this[string key]
		{
			get
			{
				return ParticleSystemForKey(key);
			}
		}

		#endregion Properties



		#region Cleaning up

		public static void PurgeSharedParticleSystemCache()
		{
			if (sharedParticleSystemCache != null)
			{
				sharedParticleSystemCache.Dispose();
				sharedParticleSystemCache = null;
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
			if (disposing && configs != null)
			{
				foreach (var t in configs.Values)
				{
					t.Dispose();
				}

				configs = null;
			}
		}

		#endregion Cleaning up


		#region Constructors

		public CCParticleSystemCache(CCApplication application)
			: this(application.Scheduler)
		{
            sharedParticleSystemCache = this;
		}

		CCParticleSystemCache(CCScheduler scheduler)
		{
			Scheduler = scheduler;

			configs = new Dictionary<string, CCParticleSystemConfig>();

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
								Scheduler.Schedule(
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

		#endregion Constructors


		public void Update(float dt)
		{
		}

		public void UnloadContent()
		{
			configs.Clear();
		}

		public bool Contains(string assetFile)
		{
			return configs.ContainsKey(assetFile);
		}

		public void AddParticleSystemAsync(string fileConfig, Action<CCParticleSystemConfig> action, string directoryName = null)
		{
			Debug.Assert(!String.IsNullOrEmpty(fileConfig), "ParticleSystemConfigCache: fileConfig MUST not be NULL");

			lock (asyncLoadedConfigs)
			{
				asyncLoadedConfigs.Add(new AsyncStruct() { FileName = fileConfig, Action = action, OnLoad = OnConfigLoad });
			}

			if (Task == null)
			{
				Task = CCTask.RunAsync(Scheduler, ProcessingAction);
			}
		}

		void OnConfigLoad(CCParticleSystemConfig config, Action<CCParticleSystemConfig> action)
		{

			// Right now Mac can not load images with data defined asyncly so we want to short circuit this and 
			// just load the data from disk.  If not then we will perform an async load on the image
#if MACOS
            config.LoadParticleTexture ();
            if (action != null)
                action (config);
#else

			string textureData = config.TextureData;

			// We will try loading the texture data first if it exists.  Hopefully we get lucky
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

					var imageBytes = ZipUtils.Inflate(dataBytes);
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
														config.Texture = tex2;

														if (config.Texture == null)
															config.Texture = CCParticleExample.DefaultTexture;

														if (action != null)
															action(config);
													});
												}
												catch (Exception)
												{
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
							config.Texture = tex2;

							if (config.Texture == null)
								config.Texture = CCParticleExample.DefaultTexture;

							if (action != null)
								action(config);
						});
					}
					catch (Exception)
					{
						config.Texture = CCParticleExample.DefaultTexture;
					}

					CCFileUtils.IsPopupNotify = bNotify;

				}
			}
#endif
		}

		public CCParticleSystemConfig AddParticleSystem(string fileConfig, string directoryName = null)
		{
			return AddParticleSystem(fileConfig, directoryName, false);
		}

		CCParticleSystemConfig AddParticleSystem(string fileConfig, string directoryName, bool loadAsync)
		{
			Debug.Assert(!String.IsNullOrEmpty(fileConfig), "ParticleSystemConfigCache: fileConfig MUST not be NULL");

			CCParticleSystemConfig psConfig = null;

			var assetName = fileConfig;
			if (Path.HasExtension(assetName))
			{
				assetName = CCFileUtils.RemoveExtension(assetName);
			}

			lock (dictLock)
			{
				configs.TryGetValue(assetName, out psConfig);
			}
			if (psConfig == null)
			{
				psConfig = new CCParticleSystemConfig(fileConfig, directoryName, loadAsync);

				if (psConfig != null)
				{
					lock (dictLock)
					{
						configs[assetName] = psConfig;
					}
				}
				else
				{
					return null;
				}
			}

			return psConfig;
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

				configs.TryGetValue(key, out config);
			}
			catch (ArgumentNullException)
			{
				CCLog.Log("Particle System with key {0} does not exist.", key);
			}

			return config;
		}

		public void RemoveAll()
		{
			configs.Clear();
		}

		public void RemoveUnused()
		{
			if (configs.Count > 0)
			{
				var tmp = new Dictionary<string, WeakReference>();

				foreach (var pair in configs)
				{
					tmp.Add(pair.Key, new WeakReference(pair.Value));
				}

				configs.Clear();

				GC.Collect();

				foreach (var pair in tmp)
				{
					if (pair.Value.IsAlive)
					{
						configs.Add(pair.Key, (CCParticleSystemConfig)pair.Value.Target);
					}
				}
			}
		}

		public void Remove(CCParticleSystemConfig particleSystem)
		{
			if (particleSystem == null)
			{
				return;
			}

			string key = null;

			foreach (var pair in configs)
			{
				if (pair.Value == particleSystem)
				{
					key = pair.Key;
					break;
				}
			}

			if (key != null)
			{
				configs.Remove(key);
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

			configs.Remove(particleSystemKeyName);
		}

		public void DumpCachedInfo()
		{
			int count = 0;
			int total = 0;

			foreach (var pair in configs)
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
	}
}