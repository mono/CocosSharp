namespace CocosSharp
{
    public class CCSkewBy : CCSkewTo
    {
        #region Constructors

        public CCSkewBy(float t, float deltaSkewX, float deltaSkewY) : base(t, deltaSkewX, deltaSkewY)
        {
            InitCCSkewBy(deltaSkewX, deltaSkewY);
        }

        protected CCSkewBy(CCSkewBy skewBy) : base(skewBy)
        {
            InitCCSkewBy(skewBy.m_fSkewX, skewBy.m_fSkewY);
        }

        private void InitCCSkewBy(float sx, float sy)
        {
            m_fSkewX = sx;
            m_fSkewY = sy;
        }

        #endregion Constructors


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