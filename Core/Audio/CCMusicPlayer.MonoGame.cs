using System;
using CocosSharp;
using Microsoft.Xna.Framework.Media;
using XnaMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;

#if WINDOWS_PHONE8
using Microsoft.Phone.Shell;
#endif

namespace CocosSharp
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
            get { return XnaMediaPlayer.Volume; }

            set
            {
                value = CCMathHelper.Clamp(value, 0.0f, 1.0f);
                XnaMediaPlayer.Volume = value;
            }
        }

        // Returns true if any song is playing in the media player, even if it is not one of the songs in the game.
        public override bool Playing
        {
            get { return (MediaState.Playing == XnaMediaPlayer.State); }
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
                if (MediaState.Playing == XnaMediaPlayer.State) 
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
            if(XnaMediaPlayer.State == MediaState.Playing) SaveMediaState();
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
                XnaMediaPlayer.IsRepeating = loop;
                XnaMediaPlayer.Play(music);
                didPlayGameSong = true;
            }
        }

        public override void Pause()
        {
            XnaMediaPlayer.Pause();
        }

        public override void Resume()
        {
            XnaMediaPlayer.Resume();
        }

        public override void Stop()
        {
            XnaMediaPlayer.Stop();
        }

        public override void Rewind()
        {
            Song s = XnaMediaPlayer.Queue.ActiveSong;

            Stop();

            if (null != music)
            {
                XnaMediaPlayer.Play(music);
            }
            else if (s != null)
            {
                XnaMediaPlayer.Play(s);
            }
        }

        #endregion Music controls


        public override void SaveMediaState()
        {
            try
            {
                // User is playing a song, so remember the song state.
                songToPlayAfterClose = XnaMediaPlayer.Queue.ActiveSong;
                volumeAfterClose = XnaMediaPlayer.Volume;
                isRepeatingAfterClose = XnaMediaPlayer.IsRepeating;
                isShuffleAfterClose = XnaMediaPlayer.IsShuffled;
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
                    XnaMediaPlayer.IsShuffled = isShuffleAfterClose;
                    XnaMediaPlayer.IsRepeating = isRepeatingAfterClose;
                    XnaMediaPlayer.Volume = volumeAfterClose;
                    XnaMediaPlayer.Play(songToPlayAfterClose);
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

