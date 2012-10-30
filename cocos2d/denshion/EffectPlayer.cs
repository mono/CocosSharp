using Microsoft.Xna.Framework.Audio;
using cocos2d;

namespace CocosDenshion
{
    public class EffectPlayer
    {
        public static ulong s_mciError;
        private SoundEffect m_effect;
        private int m_nSoundId;

        public EffectPlayer()
        {
            m_nSoundId = 0;
        }

        public static float Volume
        {
            get { return SoundEffect.MasterVolume; }
            set
            {
                if (value >= 0.0f && value <= 1.0f)
                {
                    SoundEffect.MasterVolume = value;
                }
            }
        }

        ~EffectPlayer()
        {
            Close();
        }

        public void Open(string pFileName, int uId)
        {
            if (string.IsNullOrEmpty(pFileName))
            {
                return;
            }

            Close();

            m_effect = CCApplication.SharedApplication.Content.Load<SoundEffect>(pFileName);

            m_nSoundId = uId;
        }

        public void Play(bool bLoop)
        {
            if (null == m_effect)
            {
                return;
            }

            m_effect.Play();
        }

        public void Play()
        {
            Play(false);
        }

        public void Close()
        {
            Stop();

            m_effect = null;
        }

        public void Pause()
        {
            CCLog.Log("Pause is invalid for sound effect");
        }

        public void Resume()
        {
            CCLog.Log("Resume is invalid for sound effect");
        }

        public void Stop()
        {
            CCLog.Log("Stop is invalid for sound effect");
        }

        public void Rewind()
        {
            CCLog.Log("Rewind is invalid for sound effect");
        }

        public bool IsPlaying()
        {
            CCLog.Log("IsPlaying is invalid for sound effect");
            return false;
        }

        public int SoundID
        {
            get { return m_nSoundId; }
        }

        // the volume is gloabal, it will affect other effects' volume
    }
}