using System;

namespace cocos2d
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

        public bool InitWithWaves(int wav, float amp, bool h, bool v, ccGridSize gridSize, float duration)
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

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCWaves pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCWaves) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCWaves();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithWaves(m_nWaves, m_fAmplitude, m_bHorizontal, m_bVertical, m_sGridSize, m_fDuration);

            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < m_sGridSize.x + 1; ++i)
            {
                for (j = 0; j < m_sGridSize.y + 1; ++j)
                {
                    ccVertex3F v = OriginalVertex(new ccGridSize(i, j));

                    if (m_bVertical)
                    {
                        v.x = (v.x + ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.y * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    }

                    if (m_bHorizontal)
                    {
                        v.y = (v.y + ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.x * .01f) * m_fAmplitude * m_fAmplitudeRate));
                    }

                    SetVertex(new ccGridSize(i, j), ref v);
                }
            }
        }

        public static CCWaves Create(int wav, float amp, bool h, bool v, ccGridSize gridSize, float duration)
        {
            var pAction = new CCWaves();
            pAction.InitWithWaves(wav, amp, h, v, gridSize, duration);
            return pAction;
        }
    }
}