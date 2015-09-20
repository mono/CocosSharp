using System.Collections.Generic;
using System;
using CocosSharp;

namespace CocosDenshion
{   
    [Obsolete("CCSimpleAudioEngine is obsolete; Use CCAudioEngine in namespace CocosSharp")]
    public class CCSimpleAudioEngine : CCAudioEngine
    {
        static CCSimpleAudioEngine instance = new CCSimpleAudioEngine();

        public new static CCSimpleAudioEngine SharedEngine
        {
            get { return instance; }
        }
    }
}

namespace CocosSharp
{
    public class CCAudioEngine
    {
        internal static readonly string[] AllowedTypesMac = { "m4a", "aac", "mp3", "wav", "aifc", "caf" };
        static CCAudioEngine instance = new CCAudioEngine();

        // The list of sounds that are configured for looping. These need to be stopped when the game pauses.
        Dictionary<int, int> loopedSounds = new Dictionary<int, int>();

        Dictionary<int, CCEffectPlayer> list = new Dictionary<int,CCEffectPlayer>();
        CCMusicPlayer music = new CCMusicPlayer();
        float effectsVolume = 1.0f;


        #region Properties

        public static CCAudioEngine SharedEngine
        {
            get { return instance; }
        }

        public bool BackgroundMusicPlaying
        {
            get { return music.Playing; }
        }

        public float BackgroundMusicVolume
        {
            get { return music.Volume; }
            set { music.Volume = value; }
        }

        // The volume of the effects max value is 1.0,the min value is 0.0
        public float EffectsVolume
        {
            get { return effectsVolume; }
            set 
            { 
                effectsVolume = CCMathHelper.Clamp(value, 0.0f, 1.0f);

                lock (list)
                {
                    foreach (CCEffectPlayer soundEffect in list.Values)
                    {
                        soundEffect.Volume = effectsVolume;
                    }
                }
            }
        }

        #endregion Properties


        public static string FullPath(string path)
        {
            // todo: return self now
            return path;
        }

        public void End()
        {
            music.Close();

            lock (list) 
            {
                foreach (var kvp in list) 
                {
                    kvp.Value.Close();
                }

                list.Clear();
            }
        }

        /// <summary>
        /// Restore the media player's state to how it was prior to the game launch. You need to do this when the game terminates
        /// if you run music that clobbers the music that was playing before the game launched.
        /// </summary>
        public void RestoreMediaState()
        {
            music.RestoreMediaState();
        }

        public void SaveMediaState()
        {
            music.SaveMediaState();
        }

        public void PreloadBackgroundMusic(string filename)
        {
            music.Open(FullPath(filename), filename.GetHashCode());
        }

        public void PlayBackgroundMusic(string filename, bool loop=false)
        {
            if (null == filename)
            {
                return;
            }

            float musicVolume = music.Volume;

            // Opening a music file resets the volume
            music.Open(FullPath(filename), filename.GetHashCode());

            music.Volume = musicVolume;

            music.Play(loop);
        }

        public void StopBackgroundMusic(bool releaseData=false)
        {
            if (releaseData)
            {
                music.Close();
            }
            else
            {
                music.Stop();
            }
        }

        public void PauseBackgroundMusic()
        {
            music.Pause();
        }

        public void ResumeBackgroundMusic()
        {
            music.Resume();
        }

        public void RewindBackgroundMusic()
        {
            music.Rewind();
        }

        public bool WillPlayBackgroundMusic()
        {
            return false;
        }

        public void PauseEffect(int fxid) 
        {
            lock (list)
            {
                try
                {
                    if (list.ContainsKey(fxid))
                    {
                        list[fxid].Pause();
                    }
                }
                catch (Exception ex)
                {
                    CCLog.Log("Unexpected exception while playing a SoundEffect: {0}", fxid);
                    CCLog.Log(ex.ToString());
                }
            }
        }

        public void PauseAllEffects()
        {
            List<CCEffectPlayer> l = new List<CCEffectPlayer>();

            lock (list)
            {
                try
                {
                    l.AddRange(list.Values);
                }
                catch (Exception ex)
                {
                    CCLog.Log("Unexpected exception while pausing all effects.");
                    CCLog.Log(ex.ToString());
                }
            }
            foreach (CCEffectPlayer p in l)
            {
                PauseEffect(p.SoundID);
            }
        }

        public void ResumeEffect(int fxid)
        {
            lock (list)
            {
                try
                {
                    if (list.ContainsKey(fxid))
                    {
                        list[fxid].Resume();
                    }
                }
                catch (Exception ex)
                {
                    CCLog.Log("Unexpected exception while resuming a SoundEffect: {0}", fxid);
                    CCLog.Log(ex.ToString());
                }
            }
        }

        public void ResumeAllEffects()
        {

            List<CCEffectPlayer> l = new List<CCEffectPlayer>();

            lock (list)
            {
                try
                {
                    l.AddRange(list.Values);
                }
                catch (Exception ex)
                {
                    CCLog.Log("Unexpected exception while resuming all effects.");
                    CCLog.Log(ex.ToString());
                }
            }
            foreach (CCEffectPlayer p in l)
            {
                p.Resume();
            }

        }

        public void StopAllEffects()
        {
            List<CCEffectPlayer> l = new List<CCEffectPlayer>();

            lock (list)
            {
                try
                {
                    l.AddRange(list.Values);
                }
                catch (Exception ex)
                {
                    CCLog.Log("Unexpected exception while stopping all effects.");
                    CCLog.Log(ex.ToString());
                }

                foreach (CCEffectPlayer p in l)
                {
                    StopEffect(p.SoundID);
                }

                list.Clear();
            }

        }

        public int PlayEffect(int fxid)
        {
            PlayEffect(fxid, false);
            return (fxid);
        }

        public int PlayEffect(int fxid, bool bLoop)
        {
            lock (list)
            {
                try
                {
                    if (list.ContainsKey(fxid))
                    {
                        lock (loopedSounds)
                        {
                            list[fxid].Play(bLoop);
                            if (bLoop)
                            {
                                loopedSounds[fxid] = fxid;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CCLog.Log("Unexpected exception while playing a SoundEffect: {0}", fxid);
                    CCLog.Log(ex.ToString());
                }
            }

            return fxid;
        }

        public int PlayEffect (string filename, bool loop=false)
        {
            int nId = filename.GetHashCode ();

            PreloadEffect (filename);

            lock (list)
            {
                try
                {
                    if (list.ContainsKey(nId))
                    {
                        lock (loopedSounds)
                        {
                            list[nId].Play(loop);
                            if (loop)
                            {
                                loopedSounds[nId] = nId;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    CCLog.Log("Unexpected exception while playing a SoundEffect: {0}", filename);
                    CCLog.Log(ex.ToString());
                }
            }

            return nId;
        }

        public void StopEffect(int soundId)
        {
            lock (list)
            {
                if (list.ContainsKey(soundId))
                {
                    list[soundId].Stop();
                }
            }
            lock (loopedSounds)
            {
                if (loopedSounds.ContainsKey(soundId))
                {
                    loopedSounds.Remove(soundId);
                }
            }
        }

        public void StopAllLoopingEffects()
        {
            if (loopedSounds.Count > 0)
            {
                int[] a = new int[loopedSounds.Keys.Count];
                loopedSounds.Keys.CopyTo(a, 0);
                foreach (int key in a)
                {
                    StopEffect(key);
                }
            }
        }

        // Load the sound effect found with the given path. The sound effect is only loaded one time and the
        // effect is cached as an instance of EffectPlayer.
        public void PreloadEffect(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            int nId = filename.GetHashCode();
            lock (list)
            {
                if (list.ContainsKey(nId))
                {
                    return;
                }
            }
            CCEffectPlayer eff = new CCEffectPlayer();
            eff.Open(FullPath(filename), nId);
            eff.Volume = effectsVolume;
            lock (list)
            {
                list[nId] = eff;
            }
        }

        public void UnloadEffect(string filename)
        {
            int nId = filename.GetHashCode();
            lock (list) 
            {
                if (list.ContainsKey(nId))
                {
                    list.Remove(nId);
                }
            }
            lock (loopedSounds)
            {
                if (loopedSounds.ContainsKey(nId))
                {
                    loopedSounds.Remove(nId);
                }
            }
        }
    }
}