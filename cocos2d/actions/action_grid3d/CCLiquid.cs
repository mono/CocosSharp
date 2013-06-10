using System;

namespace Cocos2D
{
    public class CCLiquid : CCGrid3DAction
    {
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected int m_nWaves;

        public CCLiquid()
        {
        }

        public CCLiquid(float duration, CCGridSize gridSize, int waves, float amplitude)
        {
            InitWithDuratuon(duration, gridSize, waves, amplitude);
        }

        public CCLiquid(CCLiquid liquid)
        {
            InitWithDuratuon(liquid.m_fDuration, liquid.m_sGridSize, liquid.m_nWaves, liquid.m_fAmplitude);
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


        public bool InitWithDuratuon(float duration, CCGridSize gridSize, int waves, float amplitude)
        {
            if (InitWithDuration(duration, gridSize))
            {
                m_nWaves = waves;
                m_fAmplitude = amplitude;
                m_fAmplitudeRate = 1.0f;

                return true;
            }
            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCLiquid) (pZone);
                base.Copy(pZone);

                pCopy.InitWithDuratuon(m_fDuration, m_sGridSize, m_nWaves, m_fAmplitude);

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
                    v.X = (v.X +
                           ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.X * .01f) * m_fAmplitude *
                            m_fAmplitudeRate));
                    v.Y = (v.Y +
                           ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.Y * .01f) * m_fAmplitude *
                            m_fAmplitudeRate));
                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }
    }
}