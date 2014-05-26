using System.Collections.Generic;
using System;
using CocosSharp;

namespace CocosDenshion
{
    public class CCSimpleAudioEngine
    {
		static Dictionary<int, CCEffectPlayer> list = new Dictionary<int,CCEffectPlayer>();
		static CCMusicPlayer music = new CCMusicPlayer();
		static CCSimpleAudioEngine instance = new CCSimpleAudioEngine();

        // The list of sounds that are configured for looping. These need to be stopped when the game pauses.
		Dictionary<int, int> loopedSounds = new Dictionary<int, int>();

		float effectsVolume = 1.0f;

		#region Properties

		public static CCSimpleAudioEngine SharedEngine
		{
			get { return instance; }
		}

        // The shared sound effect list. The key is the hashcode of the file path.
        public static Dictionary<int, CCEffectPlayer> SharedList
        {
			get { return list; }
        }

        static CCMusicPlayer SharedMusic
        {
            get { return music; }
        }

		public bool BackgroundMusicPlaying
		{
			get { return SharedMusic.Playing; }
		}

		public float BackgroundMusicVolume
        {
            get { return SharedMusic.Volume; }
            set { SharedMusic.Volume = value; }
        }

		// The volume of the effects max value is 1.0,the min value is 0.0
        public float EffectsVolume
        {
			get { return effectsVolume; }
			set 
			{ 
				effectsVolume = CCMathHelper.Clamp(value, 0.0f, 1.0f);

				foreach (CCEffectPlayer soundEffect in list.Values) 
				{
					soundEffect.Volume = effectsVolume;
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
            SharedMusic.Close();

            lock (SharedList) 
			{
                foreach (var kvp in SharedList) 
				{
                    kvp.Value.Close();
                }

                SharedList.Clear();
            }
        }

        /// <summary>
        /// Restore the media player's state to how it was prior to the game launch. You need to do this when the game terminates
        /// if you run music that clobbers the music that was playing before the game launched.
        /// </summary>
        public void RestoreMediaState()
        {
            SharedMusic.RestoreMediaState();
        }

        public void SaveMediaState()
        {
            SharedMusic.SaveMediaState();
        }

        public void PreloadBackgroundMusic(string filename)
        {
            SharedMusic.Open(FullPath(filename), filename.GetHashCode());
        }

		public void PlayBackgroundMusic(string filename, bool loop=false)
        {
            if (null == filename)
            {
                return;
            }

            SharedMusic.Open(FullPath(filename), filename.GetHashCode());
            SharedMusic.Play(loop);
        }

		public void StopBackgroundMusic(bool releaseData=false)
        {
            if (releaseData)
            {
                SharedMusic.Close();
            }
            else
            {
                SharedMusic.Stop();
            }
        }

        public void PauseBackgroundMusic()
        {
            SharedMusic.Pause();
        }

        public void ResumeBackgroundMusic()
        {
            SharedMusic.Resume();
        }

        public void RewindBackgroundMusic()
        {
            SharedMusic.Rewind();
        }

        public bool WillPlayBackgroundMusic()
        {
            return false;
        }

        public void PauseEffect(int fxid) 
        {
            try
            {
                if (SharedList.ContainsKey(fxid))
                {
                    SharedList[fxid].Pause();
                }
            }
            catch (Exception ex)
            {
                CCLog.Log("Unexpected exception while playing a SoundEffect: {0}", fxid);
                CCLog.Log(ex.ToString());
            }
        }

        public void StopAllEffects()
        {
            List<CCEffectPlayer> l = new List<CCEffectPlayer>();

            lock (SharedList)
            {
                try
                {
                    l.AddRange(SharedList.Values);
                    SharedList.Clear();
                }
                catch (Exception ex)
                {
                    CCLog.Log("Unexpected exception while stopping all effects.");
                    CCLog.Log(ex.ToString());
                }
            }
            foreach (CCEffectPlayer p in l)
            {
                p.Stop();
            }

        }

        public int PlayEffect(int fxid)
        {
            PlayEffect(fxid, false);
            return (fxid);
        }

        public int PlayEffect(int fxid, bool bLoop)
        {
            lock (SharedList)
            {
                try
                {
                    if (SharedList.ContainsKey(fxid))
                    {
                        SharedList[fxid].Play(bLoop);
                        if (bLoop)
                        {
                            loopedSounds[fxid] = fxid;
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

            lock (SharedList)
            {
                try
                {
                    if (SharedList.ContainsKey(nId))
                    {
                        SharedList[nId].Play(loop);
                        if (loop)
                        {
                            loopedSounds[nId] = nId;
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
            lock (SharedList)
            {
                if (SharedList.ContainsKey(soundId))
                {
                    SharedList[soundId].Stop();
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
            lock (SharedList)
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
            lock (SharedList)
            {
                if (SharedList.ContainsKey(nId))
                {
                    return;
                }
            }
            CCEffectPlayer eff = new CCEffectPlayer();
            eff.Open(FullPath(filename), nId);
			eff.Volume = effectsVolume;
            SharedList[nId] = eff;
        }

        public void UnloadEffect(string filename)
        {
            int nId = filename.GetHashCode();
            lock (SharedList) 
			{
                if (SharedList.ContainsKey(nId))
                {
                    SharedList.Remove(nId);
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