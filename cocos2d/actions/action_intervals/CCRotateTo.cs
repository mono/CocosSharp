namespace Cocos2D
{
    public class CCRotateTo : CCActionInterval
    {
        protected float m_fDiffAngleY;
        protected float m_fDiffAngleX;
        protected float m_fDstAngleX;
        protected float m_fDstAngleY;
        protected float m_fStartAngleX;
        protected float m_fStartAngleY;

        private CCRotateTo()
        {
        }

        public CCRotateTo(float duration, float fDeltaAngle)
        {
            InitWithDuration(duration, fDeltaAngle);
        }

        public CCRotateTo(float duration, float fDeltaAngleX, float fDeltaAngleY)
        {
            InitWithDuration(duration, fDeltaAngleX, fDeltaAngleY);
        }

        protected CCRotateTo(CCRotateTo rotateTo)
            : base(rotateTo)
        {
            InitWithDuration(rotateTo.m_fDuration, rotateTo.m_fDstAngleX, rotateTo.m_fDstAngleY);
        }

        private bool InitWithDuration(float duration, float fDeltaAngle)
        {
            if (base.InitWithDuration(duration))
            {
                m_fDstAngleX = m_fDstAngleY = fDeltaAngle;
                return true;
            }
            return false;
        }

        private bool InitWithDuration(float duration, float fDeltaAngleX, float fDeltaAngleY)
        {
            if (base.InitWithDuration(duration))
            {
                m_fDstAngleX = fDeltaAngleX;
                m_fDstAngleY = fDeltaAngleY;
                return true;
            }
            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCRotateTo;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(ret);

                ret.InitWithDuration(m_fDuration, m_fDstAngleX, m_fDstAngleY);
                return ret;
            }
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