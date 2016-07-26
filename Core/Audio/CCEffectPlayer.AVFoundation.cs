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

namespace CocosDenshion
{
    public partial class CCEffectPlayer
    {

        AVAudioPlayer soundEffect;


        #region Properties

        public override float Volume
        {
            get { return soundEffect != null ? soundEffect.Volume : 0.0f; }
            set
            {
                if (soundEffect == null)
                    return;

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
                soundEffect.NumberOfLoops = loop ? -1 : 0;

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


        public override void Open(string fileName, int soundId)
        {
            base.Open(fileName, soundId);

            string relFilePath = Path.Combine(CCContentManager.SharedContentManager.RootDirectory, fileName);
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
                foreach(string formatType in CCSimpleAudioEngine.AllowedTypesMac)
                {
                    absFilePath = NSBundle.MainBundle.PathForResource(relFilePath, formatType);
                    if(absFilePath !=null)
                        break;
                }

            if (absFilePath == null)
                CCLog.Log("CocosSharp: File Name: " + fileName + " was not found.");

            if (absFilePath != null)
            {
                try 
                {
                    soundEffect = AVAudioPlayer.FromUrl(new NSUrl(absFilePath, false));
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

            soundEffect = null;
        }
    }
}

