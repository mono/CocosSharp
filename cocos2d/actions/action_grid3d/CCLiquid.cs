using System;

namespace CocosSharp
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

		public int Waves
		{
			get { return m_nWaves; }
			set { m_nWaves = value; }
		}

        #region Constructors

		public CCLiquid(float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0) : base(duration, gridSize)
        {
            InitCCLiquid(waves, amplitude);
        }

        // Perform deep copy of CCLiquid
        public CCLiquid(CCLiquid liquid)
        {
            InitCCLiquid(liquid.m_nWaves, liquid.m_fAmplitude);
        }

        private void InitCCLiquid(int waves, float amplitude)
        {
            m_nWaves = waves;
            m_fAmplitude = amplitude;
            m_fAmplitudeRate = 1.0f;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCLiquid(this);
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 1; i < m_sGridSize.X; ++i)
            {
                for (j = 1; j < m_sGridSize.Y; ++j)
                {
                    CCVertex3F v = OriginalVertex(i, j);
                    v.X = (v.X +
                           ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.X * .01f) * m_fAmplitude *
                            m_fAmplitudeRate));
                    v.Y = (v.Y +
                           ((float) Math.Sin(time * (float) Math.PI * m_nWaves * 2 + v.Y * .01f) * m_fAmplitude *
                            m_fAmplitudeRate));
                    SetVertex(i, j, ref v);
                }
            }
        }
    }
}