namespace CocosSharp
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


        #region Constructors

        public CCSkewTo(float t, float sx, float sy) : base(t)
        {
            InitCCSkewTo(sx, sy);
        }

        // Perform deep copy of CCSkewTo
        protected CCSkewTo(CCSkewTo skewTo) : base(skewTo)
        {
            InitCCSkewTo(skewTo.m_fEndSkewX, skewTo.m_fStartSkewY);
        }

        private void InitCCSkewTo(float sx, float sy)
        {
            m_fEndSkewX = sx;
            m_fEndSkewY = sy;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
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