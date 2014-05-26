using System;
using CocosSharp;
using Microsoft.Xna.Framework.Media;

#if WINDOWS_PHONE8
using Microsoft.Phone.Shell;
#endif

namespace CocosDenshion
{
	public partial class CCMusicPlayer
	{
		/// Track if we did play our own game song, otherwise the media player is owned
		/// by the user of the device and that user is listening to background music.
		bool didPlayGameSong;
		bool isRepeatingAfterClose;
		bool isShuffleAfterClose;
		float volumeAfterClose = 1f;
		Song songToPlayAfterClose;
		Song music;


		#region Properties

		public override float Volume
		{
			get { return MediaPlayer.Volume; }

			set
			{
				value = CCMathHelper.Clamp(value, 0.0f, 1.0f);
				MediaPlayer.Volume = value;
			}
		}

		// Returns true if any song is playing in the media player, even if it is not one of the songs in the game.
		public override bool Playing
		{
			get { return (MediaState.Playing == MediaPlayer.State); }
		}

		// Returns true if one of the game songs is playing.
		public override bool PlayingMySong
		{
			get 
			{
				if (!didPlayGameSong) 
				{
				return false;
				}
				if (MediaState.Playing == MediaPlayer.State) 
				{
				return true;
				}

				return false;
			}
		}

		#endregion Properties


		#region Constructor

		public CCMusicPlayer()
		{
			if(MediaPlayer.State == MediaState.Playing) SaveMediaState();
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
				MediaPlayer.IsRepeating = loop;
				MediaPlayer.Play(music);
				didPlayGameSong = true;
			}
		}

		public override void Pause()
		{
			MediaPlayer.Pause();
		}

		public override void Resume()
		{
			MediaPlayer.Resume();
		}

		public override void Stop()
		{
			MediaPlayer.Stop();
		}

		public override void Rewind()
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

		#endregion Music controls


		public override void SaveMediaState()
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

		public override void RestoreMediaState()
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
				catch(Exception ex)
				{
					CCLog.Log("Failed to restore the media state of the game.");
					CCLog.Log(ex.ToString());
				}
			}
		}

		public override void Open(string fileName, int uId)
		{
			base.Open(fileName, uId);
			music = CCContentManager.SharedContentManager.Load<Song>(fileName);
		}

		public override void Close()
		{
			base.Close();
			music = null;
		}

	}
}

