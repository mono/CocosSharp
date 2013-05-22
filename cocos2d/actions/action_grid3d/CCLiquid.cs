using System;

namespace Cocos2D
{
    public class CCLiquid : CCGrid3DAction
    {
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected int m_nWaves;

        public CCLiquid ()
        { }

        public CCLiquid (int wav, float amp, CCGridSize gridSize, float duration)
        {
            InitWithWaves(wav, amp, gridSize, duration);
        }

        public CCLiquid (CCLiquid liquid) 
        {
            InitWithWaves(liquid.m_nWaves, liquid.m_fAmplitude, liquid.m_sGridSize, liquid.m_fDuration);
        }

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


        public bool InitWithWaves(int wav, float amp, CCGridSize gridSize, float duration)
        {
            if (InitWithSize(gridSize, duration))
            {
                m_nWaves = wav;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;

                return true;
            }
            return false;
        }

        public override object Copy(ICopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCLiquid) (pZone);
                base.Copy(pZone);
                
                pCopy.InitWithWaves(m_nWaves, m_fAmplitude, m_sGridSize, m_fDuration);
                
                return pCopy;
            }
            else
            {
                return new CCLiquid(this);
            }

        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 1; i < m_sGridSize.X; ++i)
            {
                for (j = 1; j < m_sGridSize.Y; ++j)
                {
                    CCVertex3F v = OriginalVertex(new CCGridSize(i, j));
                    v.X = (v.X + ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.X * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    v.Y = (v.Y + ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.Y * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }

    }
}