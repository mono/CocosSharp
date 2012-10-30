namespace cocos2d
{
    public class CCRotateBy : CCActionInterval
    {
        protected float m_fAngle;
        protected float m_fStartAngle;

        public bool InitWithDuration(float duration, float fDeltaAngle)
        {
            if (base.InitWithDuration(duration))
            {
                m_fAngle = fDeltaAngle;
                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCRotateBy ret;

            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = tmpZone.m_pCopyObject as CCRotateBy;
                if (ret == null)
                {
                    return null;
                }
            }
            else
            {
                ret = new CCRotateBy();
                tmpZone = new CCZone(ret);
            }

            base.CopyWithZone(tmpZone);

            ret.InitWithDuration(m_fDuration, m_fAngle);

            return ret;
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
            return Create(m_fDuration, -m_fAngle);
        }

        public static CCRotateBy Create(float duration, float fDeltaAngle)
        {
            var ret = new CCRotateBy();
            ret.InitWithDuration(duration, fDeltaAngle);
            return ret;
        }
    }
}