using System;

namespace Cocos2D
{
    public class CCWaves3D : CCGrid3DAction
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

        public override object Copy(ICopyable pZone)
        {
            CCWaves3D pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCWaves3D) (pZone);
            }
            else
            {
                pCopy = new CCWaves3D();
                pZone =  (pCopy);
            }

            base.Copy(pZone);

            pCopy.InitWithWaves(m_nWaves, m_fAmplitude, m_sGridSize, m_fDuration);

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
                    v.Z += ((float) Math.Sin((float) Math.PI * time * m_nWaves * 2 + (v.Y + v.X) * .01f) * m_fAmplitude * m_fAmplitudeRate);
                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }

        protected CCWaves3D()
        {
        }
        public CCWaves3D(int wav, float amp, CCGridSize gridSize, float duration) : base(duration)
        {
            InitWithWaves(wav, amp, gridSize, duration);
        }
    }
}