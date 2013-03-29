namespace cocos2d
{
    public class CCScaleTo : CCActionInterval
    {
        protected float m_fDeltaX;
        protected float m_fDeltaY;
        protected float m_fEndScaleX;
        protected float m_fEndScaleY;
        protected float m_fScaleX;
        protected float m_fScaleY;
        protected float m_fStartScaleX;
        protected float m_fStartScaleY;

        public bool InitWithDuration(float duration, float s)
        {
            if (base.InitWithDuration(duration))
            {
                m_fEndScaleX = s;
                m_fEndScaleY = s;

                return true;
            }

            return false;
        }

        public bool InitWithDuration(float duration, float sx, float sy)
        {
            if (base.InitWithDuration(duration))
            {
                m_fEndScaleX = sx;
                m_fEndScaleY = sy;

                return true;
            }

            return false;
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCScaleTo pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCScaleTo) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCScaleTo();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithDuration(m_fDuration, m_fEndScaleX, m_fEndScaleY);

            return pCopy;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fStartScaleX = target.ScaleX;
            m_fStartScaleY = target.ScaleY;
            m_fDeltaX = m_fEndScaleX - m_fStartScaleX;
            m_fDeltaY = m_fEndScaleY - m_fStartScaleY;
        }

        public override void Update(float time)
        {
            if (m_pTarget != null)
            {
                m_pTarget.ScaleX = m_fStartScaleX + m_fDeltaX * time;
                m_pTarget.ScaleY = m_fStartScaleY + m_fDeltaY * time;
            }
        }

        public static CCScaleTo Create(float duration, float s)
        {
            var pScaleTo = new CCScaleTo();
            pScaleTo.InitWithDuration(duration, s);
            //pScaleTo->autorelease();

            return pScaleTo;
        }

        public static CCScaleTo Create(float duration, float sx, float sy)
        {
            var pScaleTo = new CCScaleTo();
            pScaleTo.InitWithDuration(duration, sx, sy);
            return pScaleTo;
        }
    }
}