using System;

namespace CocosSharp
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

		public int Waves
		{
			get { return m_nWaves; }
			set { m_nWaves = value; }
		}


        #region Constructors

		public CCWaves3D(float duration, CCGridSize gridSize, int waves = 0, float amplitude = 0) : base(duration, gridSize)
        {
            InitWaves3D(waves, amplitude);
        }

        // Perform deep copy of CCWaves3D
        public CCWaves3D(CCWaves3D waves3d) : base(waves3d)
        {
            InitWaves3D(waves3d.m_nWaves, waves3d.m_fAmplitude);
        }

        private void InitWaves3D(int waves, float amplitude)
        {
            m_nWaves = waves;
            m_fAmplitude = amplitude;
            m_fAmplitudeRate = 1.0f;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCWaves3D(this);
        }

        public override void Update(float time)
        {
            int i, j;
            for (i = 0; i < m_sGridSize.X + 1; ++i)
            {
                for (j = 0; j < m_sGridSize.Y + 1; ++j)
                {
                    CCVertex3F v = OriginalVertex(new CCGridSize(i, j));
                    v.Z += ((float) Math.Sin((float) Math.PI * time * m_nWaves * 2 + (v.Y + v.X) * .01f) * m_fAmplitude *
                            m_fAmplitudeRate);
                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }
    }
}