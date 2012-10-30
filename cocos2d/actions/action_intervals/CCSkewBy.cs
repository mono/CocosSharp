namespace cocos2d
{
    public class CCSkewBy : CCSkewTo
    {
        public override bool InitWithDuration(float t, float sx, float sy)
        {
            bool bRet = false;

            if (base.InitWithDuration(t, sx, sy))
            {
                m_fSkewX = sx;
                m_fSkewY = sy;

                bRet = true;
            }

            return bRet;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fDeltaX = m_fSkewX;
            m_fDeltaY = m_fSkewY;
            m_fEndSkewX = m_fStartSkewX + m_fDeltaX;
            m_fEndSkewY = m_fStartSkewY + m_fDeltaY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration, -m_fSkewX, -m_fSkewY);
        }

        public new static CCSkewBy Create(float t, float deltaSkewX, float deltaSkewY)
        {
            var pSkewBy = new CCSkewBy();
            pSkewBy.InitWithDuration(t, deltaSkewX, deltaSkewY);
            return pSkewBy;
        }
    }
}