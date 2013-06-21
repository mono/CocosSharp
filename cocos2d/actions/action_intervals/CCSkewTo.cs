namespace Cocos2D
{
    public class CCSkewTo : CCActionInterval
    {
        protected float m_fDeltaX;
        protected float m_fDeltaY;
        protected float m_fEndSkewX;
        protected float m_fEndSkewY;
        protected float m_fSkewX;
        protected float m_fSkewY;
        protected float m_fStartSkewX;
        protected float m_fStartSkewY;

        public CCSkewTo(float t, float sx, float sy)
        {
            InitWithDuration(t, sx, sy);
        }

        protected CCSkewTo(CCSkewTo skewTo) : base(skewTo)
        {
            InitWithDuration(skewTo.m_fDuration, skewTo.m_fEndSkewX, skewTo.m_fStartSkewY);
        }

        protected virtual bool InitWithDuration(float t, float sx, float sy)
        {
            bool bRet = false;

            if (base.InitWithDuration(t))
            {
                m_fEndSkewX = sx;
                m_fEndSkewY = sy;

                bRet = true;
            }

            return bRet;
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCSkewTo) (pZone);
                base.Copy(pZone);

                pCopy.InitWithDuration(m_fDuration, m_fEndSkewX, m_fEndSkewY);

                return pCopy;
            }
            return new CCSkewTo(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            m_fStartSkewX = target.SkewX;

            if (m_fStartSkewX > 0)
            {
                m_fStartSkewX = m_fStartSkewX % 180f;
            }
            else
            {
                m_fStartSkewX = m_fStartSkewX % -180f;
            }

            m_fDeltaX = m_fEndSkewX - m_fStartSkewX;

            if (m_fDeltaX > 180)
            {
                m_fDeltaX -= 360;
            }
            if (m_fDeltaX < -180)
            {
                m_fDeltaX += 360;
            }

            m_fStartSkewY = target.SkewY;

            if (m_fStartSkewY > 0)
            {
                m_fStartSkewY = m_fStartSkewY % 360f;
            }
            else
            {
                m_fStartSkewY = m_fStartSkewY % -360f;
            }

            m_fDeltaY = m_fEndSkewY - m_fStartSkewY;

            if (m_fDeltaY > 180)
            {
                m_fDeltaY -= 360;
            }
            if (m_fDeltaY < -180)
            {
                m_fDeltaY += 360;
            }
        }

        public override void Update(float time)
        {
            m_pTarget.SkewX = m_fStartSkewX + m_fDeltaX * time;
            m_pTarget.SkewY = m_fStartSkewY + m_fDeltaY * time;
        }
    }
}