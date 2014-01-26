using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCRipple3D : CCGrid3DAction
    {
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;
        protected float m_fRadius;
        protected int m_nWaves;
        protected CCPoint m_position;
        protected CCPoint m_positionInPixels;

        public CCPoint Position
        {
            get { return m_position; }
            set
            {
				if (!value.Equals(m_position))
				{
					var scale = CCDirector.SharedDirector.ContentScaleFactor;
					m_position = value;
					m_positionInPixels.X = value.X * scale;
					m_positionInPixels.Y = value.Y * scale;

				}
            }
        }

        public float Amplitude
        {
            get { return m_fAmplitude; }
            set { m_fAmplitude = value; }
        }

        public override float AmplitudeRate
        {
            set { m_fAmplitudeRate = value; }
            get { return m_fAmplitudeRate; }
        }


        #region Constructors

        public CCRipple3D()
        {
        }

        public CCRipple3D(float duration) : base(duration)
        {
        }

        public CCRipple3D(float duration, CCGridSize gridSize, CCPoint position, float radius, int waves, float amplitude) 
            : base(duration, gridSize)
        {
            InitRipple3D(position, radius, waves, amplitude);
        }

        public CCRipple3D(CCRipple3D ripple) : base(ripple)
        {
            InitRipple3D(ripple.m_position, ripple.m_fRadius, ripple.m_nWaves, ripple.m_fAmplitude);
        }

        private void InitRipple3D(CCPoint position, float radius, int waves, float amplitude)
        {
            m_position = new CCPoint(-1, -1);
            m_positionInPixels = CCPoint.Zero;

            Position = position;
            m_fRadius = radius;
            m_nWaves = waves;
            m_fAmplitude = amplitude;
            m_fAmplitudeRate = 1.0f;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCRipple3D(this);
        }

        public override void Update(float time)
        {
            int i, j;

            CCGridSize gs;

            for (i = 0; i < (m_sGridSize.X + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.Y + 1); ++j)
                {
                    gs.X = i;
                    gs.Y = j;

                    CCVertex3F v = OriginalVertex(gs);

                    float x = m_positionInPixels.X - v.X;
                    float y = m_positionInPixels.Y - v.Y;

                    var r = (float) Math.Sqrt((x * x + y * y));

                    if (r < m_fRadius)
                    {
                        r = m_fRadius - r;
                        float r1 = r / m_fRadius;
                        float rate = r1 * r1;
                        v.Z += ((float) Math.Sin(time * MathHelper.Pi * m_nWaves * 2 + r * 0.1f) * m_fAmplitude *
                                m_fAmplitudeRate * rate);
                    }

                    SetVertex(gs, ref v);
                }
            }
        }

    }
}