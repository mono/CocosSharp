namespace Cocos2D
{
    public class CCShaky3D : CCGrid3DAction
    {
        protected bool m_bShakeZ;
        protected int m_nRandrange;

        protected virtual bool InitWithRange(int range, bool shakeZ, CCGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_nRandrange = range;
                m_bShakeZ = shakeZ;

                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            CCShaky3D pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCShaky3D) (pZone);
            }
            else
            {
                pCopy = new CCShaky3D();
                pZone = pCopy;
            }

            base.Copy(pZone);

            pCopy.InitWithRange(m_nRandrange, m_bShakeZ, m_sGridSize, m_fDuration);
            return pCopy;
        }

        public override void Update(float time)
        {
            int i, j;

            for (i = 0; i < (m_sGridSize.X + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.Y + 1); ++j)
                {
                    CCVertex3F v = OriginalVertex(new CCGridSize(i, j));
                    v.X += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    v.Y += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    if (m_bShakeZ)
                    {
                        v.Z += (CCRandom.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    }

                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }

        protected CCShaky3D()
        {
        }
        public CCShaky3D(int range, bool shakeZ, CCGridSize gridSize, float duration) : base(duration)
        {
            InitWithRange(range, shakeZ, gridSize, duration);
        }
    }
}