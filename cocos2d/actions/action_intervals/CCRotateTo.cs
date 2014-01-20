namespace CocosSharp
{
    public class CCRotateTo : CCActionInterval
    {
        protected float m_fDiffAngleY;
        protected float m_fDiffAngleX;
        protected float m_fDstAngleX;
        protected float m_fDstAngleY;
        protected float m_fStartAngleX;
        protected float m_fStartAngleY;


        #region Constructors

        public CCRotateTo(float duration, float fDeltaAngleX, float fDeltaAngleY) : base(duration)
        {
            InitCCRotateTo(fDeltaAngleX, fDeltaAngleY);
        }

        public CCRotateTo(float duration, float fDeltaAngle) : this(duration, fDeltaAngle, fDeltaAngle)
        {
        }

        protected CCRotateTo(CCRotateTo rotateTo) : base(rotateTo)
        {
            InitCCRotateTo(rotateTo.m_fDstAngleX, rotateTo.m_fDstAngleY);
        }

        private void InitCCRotateTo(float fDeltaAngleX, float fDeltaAngleY)
        {
            m_fDstAngleX = fDeltaAngleX;
            m_fDstAngleY = fDeltaAngleY;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCRotateTo(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            // Calculate X
            m_fStartAngleX = m_pTarget.RotationX;
            if (m_fStartAngleX > 0)
            {
                m_fStartAngleX = m_fStartAngleX % 360.0f;
            }
            else
            {
                m_fStartAngleX = m_fStartAngleX % -360.0f;
            }

            m_fDiffAngleX = m_fDstAngleX - m_fStartAngleX;
            if (m_fDiffAngleX > 180)
            {
                m_fDiffAngleX -= 360;
            }
            if (m_fDiffAngleX < -180)
            {
                m_fDiffAngleX += 360;
            }

            //Calculate Y: It's duplicated from calculating X since the rotation wrap should be the same
            m_fStartAngleY = m_pTarget.RotationY;

            if (m_fStartAngleY > 0)
            {
                m_fStartAngleY = m_fStartAngleY % 360.0f;
            }
            else
            {
                m_fStartAngleY = m_fStartAngleY % -360.0f;
            }

            m_fDiffAngleY = m_fDstAngleY - m_fStartAngleY;
            if (m_fDiffAngleY > 180)
            {
                m_fDiffAngleY -= 360;
            }

            if (m_fDiffAngleY < -180)
            {
                m_fDiffAngleY += 360;
            }
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                m_pTarget.RotationX = m_fStartAngleX + m_fDiffAngleX * time;
                m_pTarget.RotationY = m_fStartAngleY + m_fDiffAngleY * time;
            }
        }
    }
}