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
	public partial class CCMusicPlayer
	{
		static readonly string[] allowedTypes = { "m4a", "aac", "mp3" };
		AVAudioPlayer music;


		#region Properties

		public override float Volume
		{
			get { return music != null ? music.Volume : 0.0f; }

			set
			{
				value = CCMathHelper.Clamp(value, 0.0f, 1.0f);

				if(music != null) music.Volume = value;
			}
		}

		// Returns true if any song is playing in the media player, even if it is not one of the songs in the game.
		public override bool Playing
		{
			get 
			{
				#if MACOS
				// On Mac, AVAudioSession is not implemented. Moreover, on a desktop we're not worried if other audio is playing.
				return PlayingMySong; 
				#elif IOS
				return PlayingMySong || AVAudioSession.SharedInstance().OtherAudioPlaying;
				#endif
			}
		}

		// Returns true if one of the game songs is playing.
		public override bool PlayingMySong
		{
			get { return music != null ? music.Playing : false; }
		}

		#endregion Properties


		#region Constructor

		public CCMusicPlayer()
		{
			#if IOS
			AVAudioSession.SharedInstance().Init();
			#endif
		}

		#endregion Constructor


		#region Cleaning up

		protected override void DisposeManagedResources()
		{
			if (music != null)
				music.Dispose();
		}

		#endregion Cleaning up


		#region Music controls

		public override void Play(bool loop=false)
		{
			if (music !=null)
			{
				music.NumberOfLoops = loop ? -1 : 1;
				music.Play();
			}
		}

		public override void Pause()
		{
			if (music == null)
				return;

			music.Pause();
		}

		public override void Resume()
		{
			if (music == null)
				return;

			music.Play();
		}

		public override void Stop()
		{
			if (music == null)
				return;

			music.Stop();
		}

		public override void Rewind()
		{
			if (music == null)
				return;

			music.Pause();
			music.CurrentTime = 0.0f;
		}

		#endregion Music controls

			
		public override void Open(string fileName, int uId)
		{
			base.Open(fileName, uId);

			string relFilePath = Path.Combine(CCContentManager.SharedContentManager.RootDirectory, fileName);
			string absFilePath = null;
			foreach(string formatType in allowedTypes)
			{
				absFilePath = NSBundle.MainBundle.PathForResource(relFilePath, formatType);
				if(absFilePath !=null)
					break;
			}
			music = AVAudioPlayer.FromUrl(new NSUrl(absFilePath, false));
		}

		public override void Close()
		{
			base.Close();
			music = null;
		}
	}
}

