using System;
using System.IO;
using CocosSharp;

#if MACOS
using MonoMac.AVFoundation;
using MonoMac.Foundation;
#elif IOS
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
#endif

namespace CocosDenshion
{
	public partial class CCEffectPlayer
	{
		static readonly string[] allowedTypes = { "m4a", "aac", "mp3", "wav" };
		AVAudioPlayer soundEffect;


		#region Properties

		public override float Volume
		{
			get { return soundEffect != null ? soundEffect.Volume : 0.0f; }
			set
			{
				value = CCMathHelper.Clamp(value, 0.0f, 1.0f);
				soundEffect.Volume = value;
			}
		}

		public override bool Playing
		{
			get { return soundEffect != null ? soundEffect.Playing : false; }
		}

		#endregion Properties


		#region Cleaning up

		protected override void DisposeManagedResources()
		{
			if (soundEffect != null)
				soundEffect.Dispose();
		}

		#endregion Cleaning up


		#region Effect controls

		public override void Play(bool loop=false)
		{
			if (soundEffect !=null)
			{
				soundEffect.NumberOfLoops = loop ? -1 : 1;
				soundEffect.Play();
			}
		}

		public override void Pause()
		{
			if (soundEffect == null)
				return;

			soundEffect.Pause();
		}

		public override void Resume()
		{
			if (soundEffect == null)
				return;

			soundEffect.Play();
		}

		public override void Stop()
		{
			if (soundEffect == null)
				return;

			soundEffect.Stop();
		}

		public override void Rewind()
		{
			if (soundEffect == null)
				return;

			soundEffect.Pause();
			soundEffect.CurrentTime = 0.0f;
		}

		#endregion Effect controls


		public override void Open(string filename, int uid)
		{
			base.Open(filename, uid);

			string relFilePath = Path.Combine(CCContentManager.SharedContentManager.RootDirectory, filename);
			string absFilePath = null;
			foreach(string formatType in allowedTypes)
			{
				absFilePath = NSBundle.MainBundle.PathForResource(relFilePath, formatType);
				if(absFilePath !=null)
					break;
			}
			soundEffect = AVAudioPlayer.FromUrl(new NSUrl(absFilePath, false));
		}


		public override void Close()
		{
			base.Close();

			soundEffect = null;
		}
	}
}

