using System;
using Microsoft.Xna.Framework.Audio;
using CocosSharp;

namespace CocosDenshion
{
	public class CCEffectPlayer : IDisposable
    {
        SoundEffect effect;
		SoundEffectInstance sfxInstance;


		#region Properties

		public static float Volume
		{
			get { return SoundEffect.MasterVolume; }
			set
			{
				if (value >= 0.0f && value <= 1.0f)
				{
					SoundEffect.MasterVolume = value;
				}
			}
		}

		public int SoundID { get; private set; }

		public bool Playing
		{
			get 
			{
				if (sfxInstance != null) 
				{
					return (sfxInstance.State == SoundState.Playing);
				}
				return false;
			}
		}

		#endregion Properties


		#region Constructors

        public CCEffectPlayer()
        {
			SoundID = 0;
        }

		#endregion Constructors


		#region Cleaning up

		// No unmanaged resources, so no need for finalizer

		public void Dispose()
		{
			this.Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && sfxInstance != null) 
			{
				sfxInstance.Dispose();
			}
		}

		#endregion Cleaning up


        public void Open(string filename, int uid)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            Close();

            try
            {
                effect = CCContentManager.SharedContentManager.Load<SoundEffect>(filename);
            }
            catch (Exception)
            {
                string srcfile = filename;
                if (srcfile.IndexOf('.') > -1)
                {
                    srcfile = srcfile.Substring(0, srcfile.LastIndexOf('.'));
                    effect = CCContentManager.SharedContentManager.Load<SoundEffect>(srcfile);
                }
            }

			SoundID = uid;
        }

        public void Play(bool loop)
        {
            if (null == effect)
            {
                return;
            }
            if (loop)
            {
                // If looping, then get an instance of this sound effect so that it can be
                // stopped.
                sfxInstance = effect.CreateInstance();
                sfxInstance.IsLooped = true;
            }
            if (sfxInstance != null)
            {
                sfxInstance.Play();
            }
            else
            {
                effect.Play();
            }
        }

        public void Play()
        {
            Play(false);
        }

        public void Close()
        {
            Stop();

            effect = null;
        }

        public void Pause()
        {
            if (sfxInstance != null && !sfxInstance.IsDisposed && sfxInstance.State == SoundState.Playing)
            {
                sfxInstance.Pause();
            }
//            CCLog.Log("Pause is invalid for sound effect");
        }

        public void Resume()
        {
            if (sfxInstance != null && !sfxInstance.IsDisposed && sfxInstance.State == SoundState.Paused)
            {
                sfxInstance.Play();
            }
//            CCLog.Log("Resume is invalid for sound effect");
        }

        public void Stop()
        {
            if (sfxInstance != null && !sfxInstance.IsDisposed && sfxInstance.State == SoundState.Playing)
            {
                sfxInstance.Stop();
            }
//            CCLog.Log("Stop is invalid for sound effect");
        }

        public void Rewind()
        {
            CCLog.Log("Rewind is invalid for sound effect");
        }
    }
}