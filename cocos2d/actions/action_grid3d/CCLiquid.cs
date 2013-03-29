using System;

namespace cocos2d
{
    public class CCLiquid : CCGrid3DAction
    {
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

        public override object CopyWithZone(CCZone pZone)
        {
            CCLiquid pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCLiquid) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCLiquid();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithWaves(m_nWaves, m_fAmplitude, m_sGridSize, m_fDuration);

            return pCopy;
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

        public static CCLiquid Create(int wav, float amp, CCGridSize gridSize, float duration)
        {
            var pAction = new CCLiquid();
            pAction.InitWithWaves(wav, amp, gridSize, duration);
            return pAction;
        }
    }
}