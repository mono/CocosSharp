namespace CocosSharp
{
    public class CCRotateBy : CCActionInterval
    {
        protected float m_fAngleX;
        protected float m_fAngleY;
        protected float m_fStartAngleX;
        protected float m_fStartAngleY;


        #region Constructors

        public CCRotateBy(float duration, float fDeltaAngleX, float fDeltaAngleY) : base(duration)
        {
            InitCCRotateBy(fDeltaAngleX, fDeltaAngleY);
        }

        public CCRotateBy(float duration, float fDeltaAngle) : this(duration, fDeltaAngle, fDeltaAngle)
        {
        }

        // Perform deep copy of CCRotateBy
        protected CCRotateBy(CCRotateBy rotateTo) : base(rotateTo)
        {
            InitCCRotateBy(rotateTo.m_fAngleX, rotateTo.m_fAngleY);
        }

        private void InitCCRotateBy(float fDeltaAngleX, float fDeltaAngleY)
        {
            m_fAngleX = fDeltaAngleX;
            m_fAngleY = fDeltaAngleY;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCRotateBy(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fStartAngleX = target.RotationX;
            m_fStartAngleY = target.RotationY;
        }

        public override void Update(float time)
        {
            // XXX: shall I add % 360
            if (m_pTarget != null)
            {
                m_pTarget.RotationX = m_fStartAngleX + m_fAngleX * time;
                m_pTarget.RotationY = m_fStartAngleY + m_fAngleY * time;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCRotateBy(m_fDuration, -m_fAngleX, -m_fAngleY);
        }
    }
}