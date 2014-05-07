using System;
using Microsoft.Xna.Framework.Media;
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
		bool isRepeatingAfterClose;
		bool isShuffleAfterClose;
        //private MediaQueue m_QueueAfterClose;
		Song songToPlayAfterClose;
		float volumeAfterClose = 1f;

        /// <summary>
        /// Track if we did play our own game song, otherwise the media player is owned
        /// by the user of the device and that user is listening to background music.
        /// </summary>
		bool didPlayGameSong;

		Song music;
		int nSoundId;

        public CCMusicPlayer()
        {
            nSoundId = 0;
            if (MediaPlayer.State == MediaState.Playing)
            {
                SaveMediaState();
            }
        }

        public float Volume
        {
            get { 
                return MediaPlayer.Volume; 
            }

            set
            {
                if (value >= 0.0f && value <= 1.0f)
                {
                    MediaPlayer.Volume = value;
                }
            }
        }

        public int SoundID
        {
            get { return nSoundId; }
        }

        public void SaveMediaState()
        {
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
        }

        public void RestoreMediaState()
        {
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

			try
			{
				RestoreMediaState();
			}
			catch (Exception)
			{
				// Ignore
			}

		}

		#endregion Cleaning up


        public void Open(string pFileName, int uId)
        {
            if (string.IsNullOrEmpty(pFileName))
            {
                return;
            }

            Close();

            music = CCContentManager.SharedContentManager.Load<Song>(pFileName);

            nSoundId = uId;
        }

        public void Play(bool bLoop)
        {
            if (null != music)
            {
                MediaPlayer.IsRepeating = bLoop;
                MediaPlayer.Play(music);
                didPlayGameSong = true;
            }
        }

        public void Play()
        {
            Play(false);
        }

        public void Close()
        {
            if (IsPlaying() && didPlayGameSong)
            {
                Stop();
            }
            music = null;
        }

        /// <summary>
        /// Pauses the current song being played. 
        /// </summary>
        public void Pause()
        {
            MediaPlayer.Pause();
        }

        /// <summary>
        /// Resumes playback of the current song.
        /// </summary>
        public void Resume()
        {
            MediaPlayer.Resume();
        }

        /// <summary>
        /// Stops playback of the current song and resets the playback position to zero.
        /// </summary>
        public void Stop()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// resets the playback of the current song to its beginning.
        /// </summary>
        public void Rewind()
        {
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
        }

        /// <summary>
        /// Returns true if any song is playing in the media player, even if it is not one
        /// of the songs in the game.
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            if (MediaState.Playing == MediaPlayer.State)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if one of the game songs is playing.
        /// </summary>
        /// <returns></returns>
        public bool IsPlayingMySong()
        {
            if (!didPlayGameSong)
            {
                return (false);
            }
            if (MediaState.Playing == MediaPlayer.State)
            {
                return true;
            }

            return false;
        }
    }
}