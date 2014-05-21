using System;
using System.IO;
#if MACOS
using MonoMac.AVFoundation;
using MonoMac.Foundation;
#elif IOS
using MonoTouch.AVFoundation;
using MonoTouch.Foundation;
#else
using Microsoft.Xna.Framework.Media;
#endif

#if WINDOWS_PHONE8
using Microsoft.Phone.Shell;
#endif
using CocosSharp;

namespace CocosDenshion
{
    /// <summary>
    /// This interface controls the media player on the device. For Microsoft mobile devices
    /// that play music, e.g. Zune and phone, you must not intefere with the background music
    /// unless the user has allowed it.
    /// </summary>
	public class CCMusicPlayer : IDisposable
    {
		#if MACOS || IOS
        static readonly string[] allowedTypes = { "m4a", "aac", "mp3" };
		AVAudioPlayer music;
		#else
        /// Track if we did play our own game song, otherwise the media player is owned
        /// by the user of the device and that user is listening to background music.
        bool didPlayGameSong;
        bool isRepeatingAfterClose;
        bool isShuffleAfterClose;
        float volumeAfterClose = 1f;
		Song songToPlayAfterClose;
		Song music;
		#endif


		#region Properties

		public int SoundID { get; private set; }

		public float Volume
		{
			get 
			{ 
				#if MACOS || IOS
				return music != null ? music.Volume : 0.0f;
				#else
				return MediaPlayer.Volume;
				#endif
			}

			set
			{
				value = CCMathHelper.Clamp(value, 0.0f, 1.0f);

				#if MACOS || IOS
				if(music != null) music.Volume = value;
				#else
				MediaPlayer.Volume = value;
				#endif
			}
		}

		// Returns true if any song is playing in the media player, even if it is not one of the songs in the game.
		public bool Playing
		{
			get 
			{
				#if MACOS
				// On Mac, AVAudioSession is not implemented. Moreover, on a desktop we're not worried if other audio is playing.
				return PlayingMySong; 
				#elif IOS
                return PlayingMySong || AVAudioSession.SharedInstance().OtherAudioPlaying;
				#else
				return (MediaState.Playing == MediaPlayer.State)
				#endif
			}
		}

		// Returns true if one of the game songs is playing.
		public bool PlayingMySong
		{
			get 
			{
				#if MACOS || IOS
				return music != null ? music.Playing : false;
				#elif
				if (!didPlayGameSong) 
				{
					return false;
				}
				if (MediaState.Playing == MediaPlayer.State) 
				{
					return true;
				}

				return false;
				#endif
			}
		}

		#endregion Properties


		#region Constructor

        public CCMusicPlayer()
        {
            SoundID = 0;

			#if IOS
			AVAudioSession.SharedInstance().Init();
			#elif !MACOS
			if(MediaPlayer.State == MediaState.Playing) SaveMediaState();
			#endif
        }

		#endregion Constructor


        public void SaveMediaState()
        {
			#if !(MACOS || IOS)
			try
            {
                // User is playing a song, so remember the song state.
                songToPlayAfterClose = MediaPlayer.Queue.ActiveSong;
                volumeAfterClose = MediaPlayer.Volume;
                isRepeatingAfterClose = MediaPlayer.IsRepeating;
                isShuffleAfterClose = MediaPlayer.IsShuffled;
            }
            catch (Exception ex)
            {
                CCLog.Log("Failed to save the media state of the game.");
                CCLog.Log(ex.ToString());
            }
			#endif
        }

        public void RestoreMediaState()
        {
			#if !(MACOS || IOS)
			if (songToPlayAfterClose != null && didPlayGameSong)
            {
                try
                {
                MediaPlayer.IsShuffled = isShuffleAfterClose;
                MediaPlayer.IsRepeating = isRepeatingAfterClose;
                MediaPlayer.Volume = volumeAfterClose;
                MediaPlayer.Play(songToPlayAfterClose);
            }
                catch (Exception ex)
                {
                    CCLog.Log("Failed to restore the media state of the game.");
                    CCLog.Log(ex.ToString());
                }
        	}
			#endif
        }


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
			if (disposing) 
			{
				// Dispose of managed resources
			}

			/*
			Close here eventually calls the static method MediaPlayer.Stop().
			This breaks the chain of dispose calls (i.e. we have no ivars to dispose of), so we have
			to do this cleaning up regardless of whether or not this object was explictily disposed
			*/
			this.Close();

			#if !(MACOS || IOS)
			try
			{
				RestoreMediaState();
			}
			catch (Exception)
			{
				// Ignore
			}
			#endif
		}

		#endregion Cleaning up


		public void Open(string fileName, int uId)
        {
			if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            Close();

			SoundID = uId;

			#if MACOS || IOS
			string relFilePath = Path.Combine(CCContentManager.SharedContentManager.RootDirectory, fileName);
			string absFilePath = null;
			foreach(string formatType in allowedTypes)
			{
                absFilePath = NSBundle.MainBundle.PathForResource(relFilePath, formatType);
				if(absFilePath !=null)
					break;
			}
            music = AVAudioPlayer.FromUrl(new NSUrl(absFilePath, false));
			#elif
			music = CCContentManager.SharedContentManager.Load<Song>(fileName);
			#endif

        }

		public void Play(bool loop=false)
        {
			if (music !=null)
            {
				#if MACOS || IOS
				music.NumberOfLoops = loop ? -1 : 1;
				music.Play();
				#elif
				MediaPlayer.IsRepeating = bLoop;
                MediaPlayer.Play(music);
				didPlayGameSong = true;
				#endif
            }
        }

        public void Close()
        {
			if (PlayingMySong)
            {
                Stop();
            }

            music = null;
        }

        public void Pause()
        {
			#if MACOS || IOS
			music.Pause();
			#elif
			MediaPlayer.Pause();
			#endif
        }

        public void Resume()
        {
			#if MACOS || IOS
			music.Play();
			#elif
			MediaPlayer.Resume();
			#endif
        }

        public void Stop()
        {
			#if MACOS || IOS
			music.Stop();
			#elif
			MediaPlayer.Stop();
			#endif
        }

        public void Rewind()
        {
			#if MACOS || IOS
			music.Pause();
			music.CurrentTime = 0.0f;
			#elif
			Song s = MediaPlayer.Queue.ActiveSong;

            Stop();

            if (null != music)
            {
                MediaPlayer.Play(music);
            }
            else if (s != null)
            {
                MediaPlayer.Play(s);
            }
			#endif
        }
    }
}