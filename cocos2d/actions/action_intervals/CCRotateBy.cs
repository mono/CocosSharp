namespace Cocos2D
{
    public class CCRotateBy : CCActionInterval
    {
        protected float m_fAngleX;
        protected float m_fAngleY;
        protected float m_fStartAngleX;
        protected float m_fStartAngleY;

        public CCRotateBy(float duration, float fDeltaAngle)
        {
            InitWithDuration(duration, fDeltaAngle);
        }

        public CCRotateBy(float duration, float fDeltaAngleX, float fDeltaAngleY)
        {
            InitWithDuration(duration, fDeltaAngleX, fDeltaAngleY);
        }

        protected CCRotateBy(CCRotateBy rotateTo)
            : base(rotateTo)
        {
            InitWithDuration(rotateTo.m_fDuration, rotateTo.m_fAngleX, rotateTo.m_fAngleY);
        }

        private bool InitWithDuration(float duration, float fDeltaAngle)
        {
            if (base.InitWithDuration(duration))
            {
                m_fAngleX = m_fAngleY = fDeltaAngle;
                return true;
            }
            return false;
        }

        private bool InitWithDuration(float duration, float fDeltaAngleX, float fDeltaAngleY)
        {
            if (base.InitWithDuration(duration))
            {
                m_fAngleX = fDeltaAngleX;
                m_fAngleY = fDeltaAngleY;
                return true;
            }
            return false;
        }

        public override object Copy(ICCCopyable zone)
        {
            if (zone != null)
            {
                var ret = zone as CCRotateBy;
                if (ret == null)
                {
                    return null;
                }
                base.Copy(ret);

                ret.InitWithDuration(m_fDuration, m_fAngleX, m_fAngleY);

                return ret;
            }
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