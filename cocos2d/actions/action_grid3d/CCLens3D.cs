using System;

namespace CocosSharp
{
    public class CCLens3D : CCGrid3DAction
    {
        protected bool m_bDirty;
        protected float m_fLensEffect;
        protected float m_fRadius;
        protected CCPoint m_position;
        protected CCPoint m_positionInPixels;
        protected bool m_bConcave;

        public float LensEffect
        {
            get { return m_fLensEffect; }
            set { m_fLensEffect = value; }
        }

        public bool Concave
        {
            get { return m_bConcave; }
            set { m_bConcave = value; }
        }

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

                    m_bDirty = true;
                }
            }
        }


        #region Constructors

        public CCLens3D()
        {
        }

        public CCLens3D(float duration, CCGridSize gridSize, CCPoint position, float radius) : base(duration, gridSize)
        {
            InitLens3D(position, radius);
        }

        // Perform a deep copy of CCLens3D
        public CCLens3D(CCLens3D lens3D) : base(lens3D)
        {
            InitLens3D(lens3D.m_position, lens3D.m_fRadius);
        }

        private void InitLens3D(CCPoint position, float radius)
        {
            m_position = new CCPoint(-1, -1);
            m_positionInPixels = CCPoint.Zero;

            Position = position;
            m_fRadius = radius;
            m_fLensEffect = 0.7f;
            m_bConcave = false;
            m_bDirty = true;
        }


        #endregion Constructors

        public override object Copy(ICCCopyable pZone)
        {
            return new CCLens3D(this);
        }

        public override void Update(float time)
        {
            if (m_bDirty)
            {
                int i, j;

                for (i = 0; i < m_sGridSize.X + 1; ++i)
                {
                    for (j = 0; j < m_sGridSize.Y + 1; ++j)
                    {
                        CCVertex3F v = OriginalVertex(new CCGridSize(i, j));
                        var vect = new CCPoint(m_positionInPixels.X - v.X, m_positionInPixels.Y - v.Y);
                        float r = vect.Length;

                        if (r < m_fRadius)
                        {
                            r = m_fRadius - r;
                            float pre_log = r / m_fRadius;
                            if (pre_log == 0)
                            {
                                pre_log = 0.001f;
                            }

                            float l = (float) Math.Log(pre_log) * m_fLensEffect;
                            float new_r = (float) Math.Exp(l) * m_fRadius;

                            if (Math.Sqrt((vect.X * vect.X + vect.Y * vect.Y)) > 0)
                            {
                                vect = CCPoint.Normalize(vect);

                                CCPoint new_vect = vect * new_r;
                                v.Z += (m_bConcave ? -1.0f : 1.0f) * new_vect.Length * m_fLensEffect;
                            }
                        }

                        SetVertex(new CCGridSize(i, j), ref v);
                    }
                }

                m_bDirty = false;
            }
        }
    }
}