using System;

namespace Cocos2D
{
    public class CCWaves : CCGrid3DAction
    {
        protected bool m_bHorizontal;
        protected bool m_bVertical;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected int m_nWaves;

        public float Amplitude
        {
            get { return m_fAmplitude; }
            set { m_fAmplitude = value; }
        }

        public override float AmplitudeRate
        {
            get { return m_fAmplitudeRate; }
            set { m_fAmplitudeRate = value; }
        }

        protected virtual bool InitWithWaves(int wav, float amp, bool h, bool v, CCGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_nWaves = wav;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;
                m_bHorizontal = h;
                m_bVertical = v;

                return true;
            }

            return false;
        }

        public override object Copy(ICopyable pZone)
        {
            CCWaves pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCWaves) (pZone);
            }
            else
            {
                pCopy = new CCWaves();
                pZone =  (pCopy);
            }

            base.Copy(pZone);

            pCopy.InitWithWaves(m_nWaves, m_fAmplitude, m_bHorizontal, m_bVertical, m_sGridSize, m_fDuration);

            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.X + 1; ++i)
            {
                for (j = 0; j < m_sGridSize.Y + 1; ++j)
                {
                    CCVertex3F v = OriginalVertex(new CCGridSize(i, j));

                    if (m_bVertical)
                    {
                        v.X = (v.X + ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.Y * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    }

                    if (m_bHorizontal)
                    {
                        v.Y = (v.Y + ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.X * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    }

                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }

        protected CCWaves()
        {
        }
        public CCWaves(int wav, float amp, bool h, bool v, CCGridSize gridSize, float duration) : base(duration)
        {
            InitWithWaves(wav, amp, h, v, gridSize, duration);
        }
    }
}