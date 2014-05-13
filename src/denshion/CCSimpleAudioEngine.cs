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

		/// <summary>
        /// The list of sounds that are configured for looping. These need to be stopped when the game pauses.
        /// </summary>
		Dictionary<int, int> loopedSounds = new Dictionary<int, int>();

        /// <summary>
        /// The shared sound effect list. The key is the hashcode of the file path.
        /// </summary>
        public static Dictionary<int, CCEffectPlayer> SharedList
        {
            get
            {
                return (list);
            }
        }

        /// <summary>
        /// The shared music player.
        /// </summary>
        private static CCMusicPlayer SharedMusic
        {
            get 
            { 
                return(music); 
            }
        }

        /// <summary>
        /// The singleton instance of this class.
        /// </summary>
        public static CCSimpleAudioEngine SharedEngine
        {
            get { return instance; }
        }

        public float BackgroundMusicVolume
        {
            get { return SharedMusic.Volume; }
            set { SharedMusic.Volume = value; }
        }

        /**
        @brief The volume of the effects max value is 1.0,the min value is 0.0
        */

        public float EffectsVolume
        {
            get { return CCEffectPlayer.Volume; }
            set { CCEffectPlayer.Volume = value; }
        }

        public static string FullPath(string path)
        {
            // todo: return self now
            return path;
        }

        /**
        @brief Release the shared Engine object
        @warning It must be called before the application exit, or a memroy leak will be casued.
        */

        public void End ()
        {
            SharedMusic.Close ();

            lock (SharedList) {
                foreach (var kvp in SharedList) {
                    kvp.Value.Close ();
                }

                SharedList.Clear ();
            }
        }

        /// <summary>
        /// Restore the media player's state to how it was prior to the game launch. You need to do this when the game terminates
        /// if you run music that clobbers the music that was playing before the game launched.
        /// </summary>
        public void RestoreMediaState()
        {
            // CCTask.RunAsync(CocosDenshion.CCSimpleAudioEngine.SharedEngine.RestoreMediaState); 
            SharedMusic.RestoreMediaState();
        }

        /// <summary>
        /// Save the media player's current playback state.
        /// </summary>
        public void SaveMediaState()
        {
            // CCTask.RunAsync(CocosDenshion.CCSimpleAudioEngine.SharedEngine.SaveMediaState); 
            SharedMusic.SaveMediaState();
        }

        /**
         @brief Preload background music
         @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
         */

        public void PreloadBackgroundMusic(string filename)
        {
            SharedMusic.Open(FullPath(filename), filename.GetHashCode());
        }

        /**
        @brief Play background music
        @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
        @param bLoop Whether the background music loop or not
        */

        public void PlayBackgroundMusic(string filename, bool loop)
        {
            if (null == filename)
            {
                return;
            }

            SharedMusic.Open(FullPath(filename), filename.GetHashCode());
            SharedMusic.Play(loop);
        }

        /**
        @brief Play background music
        @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
        */

        public void PlayBackgroundMusic(string filename)
        {
            PlayBackgroundMusic(filename, false);
        }

        /**
        @brief Stop playing background music
        @param bReleaseData If release the background music data or not.As default value is false
        */

        public void StopBackgroundMusic(bool releaseData)
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

        /**
        @brief Stop playing background music
        */

        public void StopBackgroundMusic()
        {
            StopBackgroundMusic(false);
        }

        /**
        @brief Pause playing background music
        */

        public void PauseBackgroundMusic()
        {
            SharedMusic.Pause();
        }

        /**
        @brief Resume playing background music
        */

        public void ResumeBackgroundMusic()
        {
            SharedMusic.Resume();
        }

        /**
        @brief Rewind playing background music
        */

        public void RewindBackgroundMusic()
        {
            SharedMusic.Rewind();
        }

        public bool WillPlayBackgroundMusic()
        {
            return false;
        }

        /**
        @brief Whether the background music is playing
        @return If is playing return true,or return false
        */

        public bool IsBackgroundMusicPlaying()
        {
			return SharedMusic.Playing;
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

        /// <summary>
        /// Play the sound effect with the given path and optionally set it to lopo.
        /// </summary>
        /// <param name="pszFilePath">The path to the sound effect file.</param>
        /// <param name="bLoop">True if the sound effect will play continuously, and false if it will play then stop.</param>
        /// <returns></returns>
        public int PlayEffect (string filename, bool loop)
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
        
        /// <summary>
        /// Plays the given sound effect without looping.
        /// </summary>
        /// <param name="pszFilePath">The path to the sound effect</param>
        /// <returns></returns>
        public int PlayEffect(string filename)
        {
            return PlayEffect(filename, false);
        }

        /// <summary>
        /// Stops the sound effect with the given id. 
        /// </summary>
        /// <param name="nSoundId"></param>
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

        /// <summary>
        /// Stops all of the sound effects that are currently playing and looping.
        /// </summary>
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

        /**
        @brief  		preload a compressed audio file
        @details	    the compressed audio will be decode to wave, then write into an 
        internal buffer in SimpleaudioEngine
        */


        /// <summary>
        /// Load the sound effect found with the given path. The sound effect is only loaded one time and the
        /// effect is cached as an instance of EffectPlayer.
        /// </summary>
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
            SharedList[nId] = eff;
        }

        /**
        @brief  		unload the preloaded effect from internal buffer
        @param[in]		pszFilePath		The path of the effect file,or the FileName of T_SoundResInfo
        */

        public void UnloadEffect (string filename)
        {
            int nId = filename.GetHashCode ();
            lock (SharedList) {
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