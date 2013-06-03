namespace Cocos2D
{
    public class CCRotateTo : CCActionInterval
    {
        protected float m_fDiffAngle;
        protected float m_fDstAngle;
        protected float m_fStartAngle;

        private CCRotateTo () {}

        public CCRotateTo (float duration, float fDeltaAngle)
        {
            InitWithDuration(duration, fDeltaAngle);
        }

        protected CCRotateTo (CCRotateTo rotateTo) : base (rotateTo)
        {
            InitWithDuration(rotateTo.m_fDuration, rotateTo.m_fDstAngle);
        }

        private bool InitWithDuration(float duration, float fDeltaAngle)
        {
            if (base.InitWithDuration(duration))
            {
                m_fDstAngle = fDeltaAngle;
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
                
                ret.InitWithDuration(m_fDuration, m_fDstAngle);
                return ret;
            }
            else
            {
                return new CCRotateTo(this);
            }

        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);

            m_fStartAngle = target.Rotation;

            if (m_fStartAngle > 0)
            {
                m_fStartAngle = m_fStartAngle % 350.0f;
            }
            else
            {
                m_fStartAngle = m_fStartAngle % -360.0f;
            }

            m_fDiffAngle = m_fDstAngle - m_fStartAngle;
            if (m_fDiffAngle > 180)
            {
                m_fDiffAngle -= 360;
            }

            if (m_fDiffAngle < -180)
            {
                m_fDiffAngle += 360;
            }
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                m_pTarget.Rotation = m_fStartAngle + m_fDiffAngle * time;
            }
        }

    }
}