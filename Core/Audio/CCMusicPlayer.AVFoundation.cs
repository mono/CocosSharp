using System;
using System.Diagnostics;
using System.IO;
using CocosSharp;

#if MACOS
using MonoMac.AVFoundation;
using MonoMac.Foundation;
#elif IOS
using AVFoundation;
using Foundation;
#endif

namespace CocosSharp
{
    public partial class CCMusicPlayer
    {

        AVAudioPlayer music;

        float volume = 0;
        #region Properties

        public override float Volume
        {
            get { return music != null ? music.Volume : volume; }

            set
            {
                value = CCMathHelper.Clamp(value, 0.0f, 1.0f);

                volume = value;

                if (music == null)
                    return;

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

            Volume = 1.0f;
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


        public override void Open(string fileName, int soundId)
        {
            base.Open(fileName, soundId);

            var relFilePath = Path.Combine(CCContentManager.SharedContentManager.RootDirectory, fileName);
            string absFilePath = null;

            // First let's try loading with the file extension type if one exists
            var ext = Path.GetExtension(fileName);

            if (!string.IsNullOrEmpty(ext))
            {
                // trim off extension
                fileName = fileName.Substring(0, fileName.Length - ext.Length);
                // create 
                relFilePath = Path.Combine(CCContentManager.SharedContentManager.RootDirectory, fileName);
                // strip off the period
                ext = ext.Substring(1);
                //  now try loading the resource using the extenion as the file type
                absFilePath = NSBundle.MainBundle.PathForResource(relFilePath, ext);
            }

            if (string.IsNullOrEmpty(absFilePath))
            {
                foreach (string formatType in CCAudioEngine.AllowedTypesMac)
                {
                    absFilePath = NSBundle.MainBundle.PathForResource(relFilePath, formatType);
                    if (absFilePath != null)
                        break;
                }
            }

            if (absFilePath == null)
                CCLog.Log("CocosSharp: File Name: " + fileName + " was not found.");

            if (absFilePath != null)
            {
                try 
                {
                    music = AVAudioPlayer.FromUrl(new NSUrl(absFilePath, false));
                    music.Volume = Volume;
                }
                catch 
                {
                    CCLog.Log("CocosSharp: File Name: " + fileName + " could not be loaded.");
                }
            }
        }

        public override void Close()
        {
            base.Close();
            music = null;
        }
    }
}

