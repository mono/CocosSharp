using System;

namespace cocos2d
{
    public class CCLens3D : CCGrid3DAction
    {
        protected bool m_bDirty;
        protected float m_fLensEffect;
        protected float m_fRadius;
        protected CCPoint m_position;
        protected CCPoint m_positionInPixels;

        public CCLens3D ()
        { }

        public CCLens3D (CCPoint pos, float r, CCGridSize gridSize, float duration) : base()
        {
            InitWithPosition(pos, r, gridSize, duration);
        }

        public CCLens3D (CCLens3D lens3D)
        {
            InitWithPosition(lens3D.m_position, lens3D.m_fRadius, lens3D.m_sGridSize, lens3D.m_fDuration);
        }

        public float LensEffect
        {
            get { return m_fLensEffect; }
            set { m_fLensEffect = value; }
        }

        public CCPoint Position
        {
            get { return m_position; }
            set
            {
                if (!value.Equals(m_position))
                {
                    m_position = value;
                    m_positionInPixels.X = value.X * CCDirector.SharedDirector.ContentScaleFactor;
                    m_positionInPixels.Y = value.Y * CCDirector.SharedDirector.ContentScaleFactor;
                    m_bDirty = true;
                }
            }
        }

        public bool InitWithPosition(CCPoint pos, float r, CCGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_position = new CCPoint(-1, -1);
                m_positionInPixels = new CCPoint();

                Position = pos;
                m_fRadius = r;
                m_fLensEffect = 0.7f;
                m_bDirty = true;

                return true;
            }

            return false;
        }

        public override object Copy(ICopyable pZone)
        {
            if (pZone != null)
            {
                // in case of being called at sub class
                var pCopy = (CCLens3D) (pZone);
                base.Copy(pZone);
                
                pCopy.InitWithPosition(m_position, m_fRadius, m_sGridSize, m_fDuration);
                
                return pCopy;
            }
            else
            {
                return new CCLens3D(this);
            }

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
                                v.Z += new_vect.Length * m_fLensEffect;
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