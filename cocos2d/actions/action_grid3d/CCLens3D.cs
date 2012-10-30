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
                if (!value.@equals(m_position))
                {
                    m_position = value;
                    m_positionInPixels.x = value.x * CCDirector.SharedDirector.ContentScaleFactor;
                    m_positionInPixels.y = value.y * CCDirector.SharedDirector.ContentScaleFactor;
                    m_bDirty = true;
                }
            }
        }

        public bool InitWithPosition(CCPoint pos, float r, ccGridSize gridSize, float duration)
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

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCLens3D pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                // in case of being called at sub class
                pCopy = (CCLens3D) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCLens3D();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithPosition(m_position, m_fRadius, m_sGridSize, m_fDuration);

            return pCopy;
        }

        public override void Update(float time)
        {
            if (m_bDirty)
            {
                int i, j;

                for (i = 0; i < m_sGridSize.x + 1; ++i)
                {
                    for (j = 0; j < m_sGridSize.y + 1; ++j)
                    {
                        ccVertex3F v = OriginalVertex(new ccGridSize(i, j));
                        var vect = new CCPoint(m_positionInPixels.x - v.x, m_positionInPixels.y - v.y);
                        float r = CCPointExtension.ccpLength(vect);

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

                            if (Math.Sqrt((vect.x * vect.x + vect.y * vect.y)) > 0)
                            {
                                vect = CCPointExtension.ccpNormalize(vect);

                                CCPoint new_vect = CCPointExtension.ccpMult(vect, new_r);
                                v.z += CCPointExtension.ccpLength(new_vect) * m_fLensEffect;
                            }
                        }

                        SetVertex(new ccGridSize(i, j), ref v);
                    }
                }

                m_bDirty = false;
            }
        }

        public static CCLens3D Create(CCPoint pos, float r, ccGridSize gridSize, float duration)
        {
            var pAction = new CCLens3D();
            pAction.InitWithPosition(pos, r, gridSize, duration);
            return pAction;
        }
    }
}