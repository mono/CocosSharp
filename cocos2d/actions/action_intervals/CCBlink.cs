namespace cocos2d
{
    public class CCBlink : CCActionInterval
    {
        protected uint m_nTimes;

        public static CCBlink Create(float duration, uint uBlinks)
        {
            var pBlink = new CCBlink();
            pBlink.InitWithDuration(duration, uBlinks);
            return pBlink;
        }

        public bool InitWithDuration(float duration, uint uBlinks)
        {
            if (base.InitWithDuration(duration))
            {
                m_nTimes = uBlinks;
                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCBlink pCopy;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCBlink) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCBlink();
                pZone = new CCZone(pCopy);
            }

            base.CopyWithZone(pZone);

            pCopy.InitWithDuration(m_fDuration, m_nTimes);

            return pCopy;
        }

        public override void Update(float time)
        {
            if (m_pTarget != null && ! IsDone)
            {
                float slice = 1.0f / m_nTimes;
                // float m = fmodf(time, slice);
                float m = time % slice;
                m_pTarget.Visible = m > (slice / 2);
            }
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration, m_nTimes);
        }
    }
}