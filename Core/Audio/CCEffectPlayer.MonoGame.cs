using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace CocosSharp
{
    public partial class CCEffectPlayer
    {
        SoundEffect effect;
        SoundEffectInstance sfxInstance;

        List<SoundEffectInstance> sfxInstances = new List<SoundEffectInstance>();


        #region Properties

        public override float Volume
        {
            get { return SoundEffect.MasterVolume; }
            set
            {
                value = CCMathHelper.Clamp(value, 0.0f, 1.0f);
                SoundEffect.MasterVolume = value;
            }
        }

        public override bool Playing
        {
            get 
            {
                if (sfxInstance != null) 
                {
                    return (sfxInstance.State == SoundState.Playing);
                }
                return false;
            }
        }

        #endregion Properties


        #region Cleaning up

        protected override void DisposeManagedResources()
        {
            if (effect != null)
                effect.Dispose();


            if (sfxInstance != null)
                sfxInstance.Dispose();
        }

        #endregion Cleaning up


        #region Effect controls

        public override void Play(bool loop=false)
        {
            if (null == effect)
            {
                return;
            }
            //          if (loop)
            //          {
            // If looping, then get an instance of this sound effect so that it can be
            // stopped.
            sfxInstance = effect.CreateInstance();
            sfxInstance.IsLooped = loop;
            sfxInstances.Add(sfxInstance);
            //          }
            if (sfxInstance != null)
            {
                sfxInstance.Play();
            }
            else
            {
                effect.Play();
            }
        }

        public override void Pause()
        {
            if (sfxInstances.Count > 0)
            {
                for (var x = 0; x < sfxInstances.Count; x++)
                {
                    var instance = sfxInstances[x];

                    if (instance.IsDisposed)
                        sfxInstances.RemoveAt(x);

                    if (instance.State == SoundState.Playing)
                        instance.Pause();
                }
            }
        }

        public override void Resume()
        {
            //          if (sfxInstance != null && !sfxInstance.IsDisposed && sfxInstance.State == SoundState.Paused)
            //          {
            //              sfxInstance.Resume();
            //          }

            if (sfxInstances.Count > 0)
            {
                for (var x = 0; x < sfxInstances.Count; x++)
                {
                    var instance = sfxInstances[x];

                    if (instance.IsDisposed)
                        sfxInstances.RemoveAt(x);

                    if (instance.State == SoundState.Paused)
                        instance.Resume();
                }
            }

        }

        public override void Stop()
        {
            //          if (sfxInstance != null && !sfxInstance.IsDisposed && sfxInstance.State == SoundState.Playing)
            //          {
            //              sfxInstance.Stop();
            //          }

            if (sfxInstances.Count > 0)
            {
                for (var x = 0; x < sfxInstances.Count; x++)
                {
                    var instance = sfxInstances[x];

                    if (instance.IsDisposed)
                        sfxInstances.RemoveAt(x);

                    if (instance.State == SoundState.Playing)
                        instance.Stop();
                }
            }
        }

        public override void Rewind()
        {
            CCLog.Log("Rewind is invalid for sound effect");
        }

        #endregion Effect controls


        public override void Open(string filename, int uid)
        {
            base.Open(filename, uid);

            try
            {
                effect = CCContentManager.SharedContentManager.Load<SoundEffect>(filename);
            }
            catch (Exception)
            {
                string srcfile = filename;
                if (srcfile.IndexOf('.') > -1)
                {
                    srcfile = srcfile.Substring(0, srcfile.LastIndexOf('.'));
                    effect = CCContentManager.SharedContentManager.Load<SoundEffect>(srcfile);
                }
            }
        }

        public override void Close()
        {
            base.Close();

            effect = null;
        }
    }
}

