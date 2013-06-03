namespace Cocos2D
{
    public class CCRotateBy : CCActionInterval
    {
        protected float m_fAngle;
        protected float m_fStartAngle;

        public CCRotateBy (float duration, float fDeltaAngle)
        {
            InitWithDuration(duration, fDeltaAngle);
        }

        protected CCRotateBy (CCRotateBy rotateTo) : base(rotateTo)
        {
            InitWithDuration(rotateTo.m_fDuration, rotateTo.m_fAngle);
        }

        private bool InitWithDuration(float duration, float fDeltaAngle)
        {
            if (base.InitWithDuration(duration))
            {
                m_fAngle = fDeltaAngle;
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
                
                ret.InitWithDuration(m_fDuration, m_fAngle);

                return ret;
            }
            else
            {
                return new CCRotateBy(this);
            }

        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fStartAngle = target.Rotation;
        }

        public override void Update(float time)
        {
            // XXX: shall I add % 360
            if (m_pTarget != null)
            {
                m_pTarget.Rotation = m_fStartAngle + m_fAngle * time;
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCRotateBy (m_fDuration, -m_fAngle);
        }


    }
}