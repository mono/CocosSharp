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


        public bool InitWithWaves(int wav, float amp, ccGridSize gridSize, float duration)
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

        public override CCObject CopyWithZone(CCZone pZone)
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

            for (i = 1; i < m_sGridSize.x; ++i)
            {
                for (j = 1; j < m_sGridSize.y; ++j)
                {
                    ccVertex3F v = OriginalVertex(new ccGridSize(i, j));
                    v.x = (v.x + ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.x * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    v.y = (v.y + ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.y * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    SetVertex(new ccGridSize(i, j), ref v);
                }
            }
        }

        public static CCLiquid Create(int wav, float amp, ccGridSize gridSize, float duration)
        {
            var pAction = new CCLiquid();
            pAction.InitWithWaves(wav, amp, gridSize, duration);
            return pAction;
        }
    }
}