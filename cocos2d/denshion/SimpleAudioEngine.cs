using System.Collections.Generic;
using System;

namespace CocosDenshion
{
    public class SimpleAudioEngine
    {
        //private static string s_szRootPath;
        //private static ulong s_dwRootLen;
        //private static string s_szFullPath;

        private static SimpleAudioEngine s_SharedEngine;
        private static List<KeyValuePair<int, EffectPlayer>> s_List;
        private static MusicPlayer s_Music;

        public static List<KeyValuePair<int, EffectPlayer>> SharedList
        {
            get { return s_List ?? (s_List = new List<KeyValuePair<int, EffectPlayer>>()); }
        }

        private static MusicPlayer SharedMusic
        {
            get { return s_Music ?? (s_Music = new MusicPlayer()); }
        }

        /**
        @brief Get the shared Engine object,it will new one when first time be called
        */

        public static SimpleAudioEngine SharedEngine
        {
            get { return s_SharedEngine ?? (s_SharedEngine = new SimpleAudioEngine()); }
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
            get { return EffectPlayer.Volume; }
            set { EffectPlayer.Volume = value; }
        }

        public static string FullPath(string szPath)
        {
            // todo: return self now
            return szPath;
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
            SharedMusic.RestoreMediaState();
        }

        /// <summary>
        /// Save the media player's current playback state.
        /// </summary>
        public void SaveMediaState()
        {
            SharedMusic.SaveMediaState();
        }
        /**
        @brief  Set the zip file name
        @param pszZipFileName The relative path of the .zip file
        */

        public static void SetResource(string pszZipFileName)
        {
        }

        /**
         @brief Preload background music
         @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
         */

        public void PreloadBackgroundMusic(string pszFilePath)
        {
            SharedMusic.Open(FullPath(pszFilePath), pszFilePath.GetHashCode());
        }

        /**
        @brief Play background music
        @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
        @param bLoop Whether the background music loop or not
        */

        public void PlayBackgroundMusic(string pszFilePath, bool bLoop)
        {
            if (null == pszFilePath)
            {
                return;
            }

            SharedMusic.Open(FullPath(pszFilePath), pszFilePath.GetHashCode());
            SharedMusic.Play(bLoop);
        }

        /**
        @brief Play background music
        @param pszFilePath The path of the background music file,or the FileName of T_SoundResInfo
        */

        public void PlayBackgroundMusic(string pszFilePath)
        {
            PlayBackgroundMusic(pszFilePath, false);
        }

        /**
        @brief Stop playing background music
        @param bReleaseData If release the background music data or not.As default value is false
        */

        public void StopBackgroundMusic(bool bReleaseData)
        {
            if (bReleaseData)
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
            return SharedMusic.IsPlaying();
        }

        // properties
        /**
        @brief the volume of the background music max value is 1.0,the min value is 0.0
        */

        // for sound effects
        /**
        @brief Play sound effect
        @param pszFilePath The path of the effect file,or the FileName of T_SoundResInfo
        @bLoop Whether to loop the effect playing, default value is false
        */

        private Dictionary<int,int> _LoopedSounds = new Dictionary<int,int>();

        public int PlayEffect (string pszFilePath, bool bLoop)
        {
            int nRet = pszFilePath.GetHashCode ();

            PreloadEffect (pszFilePath);

            lock (SharedList) {
                try {
                    foreach (var kvp in SharedList) {
                        if (nRet == kvp.Key) {
                            kvp.Value.Play (bLoop);
                            if (bLoop)
                            {
                                _LoopedSounds[nRet] = nRet;
                            }
                        }
                    }
                } 
                catch (Exception) {
                }
            }

            return nRet;
        }

        /**
        @brief Play sound effect
        @param pszFilePath The path of the effect file,or the FileName of T_SoundResInfo
        */

        public int PlayEffect(string pszFilePath)
        {
            return PlayEffect(pszFilePath, false);
        }

        /**
        @brief Stop playing sound effect
        @param nSoundId The return value of function playEffect
        */

        public void StopEffect (int nSoundId)
        {
            lock (SharedList) {
                foreach (var kvp in SharedList) {
                    if (nSoundId == kvp.Key) {
                        kvp.Value.Stop ();
                        if (_LoopedSounds.ContainsKey(nSoundId))
                        {
                            _LoopedSounds.Remove(nSoundId);
                        }
                    }
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
                if (_LoopedSounds.Count > 0)
                {
                    int[] a = new int[_LoopedSounds.Keys.Count];
                    _LoopedSounds.Keys.CopyTo(a, 0);
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

        public void PreloadEffect (string pszFilePath)
        {
            if (pszFilePath.Length <= 0) {
                return;
            }

            int nId = pszFilePath.GetHashCode ();
            lock (SharedList) {
                for (int i = 0; i < SharedList.Count; i++) {
                    if (SharedList [i].Key == nId) {
                        return;
                    }
                }

                var eff = new EffectPlayer ();
                eff.Open (FullPath (pszFilePath), nId);
                SharedList.Add (new KeyValuePair<int, EffectPlayer> (nId, eff));
            }
        }

        /**
        @brief  		unload the preloaded effect from internal buffer
        @param[in]		pszFilePath		The path of the effect file,or the FileName of T_SoundResInfo
        */

        public void UnloadEffect (string pszFilePath)
        {
            int nId = pszFilePath.GetHashCode ();
            lock (SharedList) {
                for (int i = 0; i < SharedList.Count; i++) {
                    if (SharedList [i].Key == nId) {
                        SharedList.RemoveAt (i);
                    }
                }
            }
			if(_LoopedSounds.ContainsKey(nId)) {
				_LoopedSounds.Remove(nId);
			}
        }
    }
}