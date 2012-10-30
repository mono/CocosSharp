namespace cocos2d
{
    public class CCShaky3D : CCGrid3DAction
    {
        protected bool m_bShakeZ;
        protected int m_nRandrange;

        public bool InitWithRange(int range, bool shakeZ, ccGridSize gridSize, float duration)
        {
            if (base.InitWithSize(gridSize, duration))
            {
                m_nRandrange = range;
                m_bShakeZ = shakeZ;

                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone pZone)
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

            for (i = 0; i < (m_sGridSize.x + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.y + 1); ++j)
                {
                    ccVertex3F v = OriginalVertex(new ccGridSize(i, j));
                    v.x += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    v.y += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    if (m_bShakeZ)
                    {
                        v.z += (Random.Next() % (m_nRandrange * 2)) - m_nRandrange;
                    }

                    SetVertex(new ccGridSize(i, j), ref v);
                }
            }
        }

        public static CCShaky3D Create(int range, bool shakeZ, ccGridSize gridSize, float duration)
        {
            var pAction = new CCShaky3D();
            pAction.InitWithRange(range, shakeZ, gridSize, duration);
            return pAction;
        }
    }
}