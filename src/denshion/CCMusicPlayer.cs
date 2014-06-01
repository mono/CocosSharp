using System;
using System.IO;
using CocosSharp;

namespace CocosDenshion
{
	public abstract class CCMusicPlayerCore
	{
		public int SoundID { get; private set; } 
		public abstract float Volume { get; set; }
		public abstract bool Playing { get; }
		public abstract bool PlayingMySong { get; }

		public virtual void SaveMediaState() {}
		public virtual void RestoreMediaState() {}

		public abstract void Play(bool loop = false);
		public abstract void Pause();
		public abstract void Resume();
		public abstract void Stop();
		public abstract void Rewind();

		protected abstract void DisposeManagedResources();

		public virtual void Open(string fileName, int soundId)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return;
			}

			Close();

			SoundID = soundId;
		}

		public virtual void Close()
		{
			if (PlayingMySong)
			{
				Stop();
			}
		}
	}

	/// <summary>
    /// This interface controls the media player on the device. For Microsoft mobile devices
	/// that play music, e.g. Zune and phone, you must not intefere with the background music
    /// unless the user has allowed it.
    /// </summary>
	public partial class CCMusicPlayer : CCMusicPlayerCore, IDisposable
    {
		bool alreadyDisposed;


		#region Cleaning up

		~CCMusicPlayer()
		{
			this.Dispose(false);
		}

		public void Dispose()
		{
			this.Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(alreadyDisposed)
				return;

			if(PlayingMySong)
			{
				Stop();
			}

			if(disposing) 
			{
				// Dispose of managed resources
				DisposeManagedResources();
			}

			try
			{
				RestoreMediaState();
			}
			catch(Exception)
			{
				// Ignore
			}

			alreadyDisposed = true;
		}

		#endregion Cleaning up
    }
}