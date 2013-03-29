namespace cocos2d
{
    public class CCShaky3D : CCGrid3DAction
    {
        protected bool m_bShakeZ;
        protected int m_nRandrange;

        public bool InitWithRange(int range, bool shakeZ, CCGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_nRandrange = range;
                m_bShakeZ = shakeZ;

                return true;
            }

            return false;
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCShaky3D pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCShaky3D) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCShaky3D();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

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
                    v.X += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    v.Y += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    if (m_bShakeZ)
                    {
                        v.Z += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    }

                    SetVertex(new CCGridSize(i, j), ref v);
                }
            }
        }

        public static CCShaky3D Create(int range, bool shakeZ, CCGridSize gridSize, float duration)
        {
            var pAction = new CCShaky3D();
            pAction.InitWithRange(range, shakeZ, gridSize, duration);
            return pAction;
        }
    }
}