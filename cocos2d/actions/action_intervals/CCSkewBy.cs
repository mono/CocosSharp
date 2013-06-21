namespace Cocos2D
{
    public class CCSkewBy : CCSkewTo
    {
        public CCSkewBy(float t, float deltaSkewX, float deltaSkewY) : base(t, deltaSkewX, deltaSkewY)
        {
            InitWithDuration(t, deltaSkewX, deltaSkewY);
        }

        protected CCSkewBy(CCSkewBy skewBy) : base(skewBy)
        {
            InitWithDuration(skewBy.m_fDuration, skewBy.m_fSkewX, skewBy.m_fSkewY);
        }

        protected override bool InitWithDuration(float t, float sx, float sy)
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

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fDeltaX = m_fSkewX;
            m_fDeltaY = m_fSkewY;
            m_fEndSkewX = m_fStartSkewX + m_fDeltaX;
            m_fEndSkewY = m_fStartSkewY + m_fDeltaY;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCSkewBy(m_fDuration, -m_fSkewX, -m_fSkewY);
        }
    }
}