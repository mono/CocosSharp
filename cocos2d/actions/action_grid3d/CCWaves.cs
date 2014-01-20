using System;

namespace CocosSharp
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


        #region Constructors

        protected CCWaves()
        {
        }

        public CCWaves(float duration, CCGridSize gridSize, int waves, float amplitude, bool horizontal, bool vertical)
            : base(duration, gridSize)
        {
            InitCCWaves(waves, amplitude, horizontal, vertical);
        }

        // Perform deep copy of CCWaves
        public CCWaves(CCWaves waves) : base(waves)
        {
            InitCCWaves(waves.m_nWaves, waves.m_fAmplitude, waves.m_bHorizontal, waves.m_bVertical);
        }

        private void InitCCWaves(int waves, float amplitude, bool horizontal, bool vertical)
        {
            m_nWaves = waves;
            m_fAmplitude = amplitude;
            m_fAmplitudeRate = 1.0f;
            m_bHorizontal = horizontal;
            m_bVertical = vertical;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCWaves(this);
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
                        v.X = (v.X +
                               ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.Y * .01f) * m_fAmplitude *
                                m_fAmplitudeRate));
                    }

                    if (m_bHorizontal)
                    {
                        v.Y = (v.Y +
                               ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.X * .01f) * m_fAmplitude *
                                m_fAmplitudeRate));
                    }

                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }
    }
}