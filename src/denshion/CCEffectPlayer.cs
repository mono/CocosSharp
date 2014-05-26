using System;
using CocosSharp;

namespace CocosDenshion
{
	public abstract class CCEffectPlayerCore
	{
		public int SoundID { get; private set; }
		public abstract float Volume { get; set; }
		public abstract bool Playing { get; }


		public abstract void Play(bool loop = false);
		public abstract void Pause();
		public abstract void Resume();
		public abstract void Stop();
		public abstract void Rewind();

		protected abstract void DisposeManagedResources();

		public virtual void Open(string filename, int uid)
		{
			if (string.IsNullOrEmpty(filename))
			{
				return;
			}

			Close();

			SoundID = uid;
		}

		public virtual void Close()
		{
			Stop();
		}
	}

	public partial class CCEffectPlayer : CCEffectPlayerCore, IDisposable
    {
        bool alreadyDisposed;


        #region Cleaning up

		// No unmanaged resources, so no need for finalizer

		public void Dispose()
		{
			this.Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
            if(alreadyDisposed)
                return;

            if(disposing) 
			{
				DisposeManagedResources();
			}

            alreadyDisposed = true;
		}

		#endregion Cleaning up
    }
}