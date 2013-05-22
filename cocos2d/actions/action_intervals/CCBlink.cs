namespace Cocos2D
{
    public class CCBlink : CCActionInterval
    {
        protected uint m_nTimes;

        public CCBlink (float duration, uint uBlinks)
        {
            InitWithDuration(duration, uBlinks);
        }

        protected CCBlink (CCBlink blink) : base (blink)
        {
            InitWithDuration(m_fDuration, m_nTimes);
        }

        protected bool InitWithDuration(float duration, uint uBlinks)
        {
            if (base.InitWithDuration(duration))
            {
                m_nTimes = uBlinks;
                return true;
            }

            return false;
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCBlink) (pZone);
                base.Copy(pZone);
                
                pCopy.InitWithDuration(m_fDuration, m_nTimes);
                return pCopy;
            }
            else
            {
                return new CCBlink(this);
            }

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
            return new CCBlink(m_fDuration, m_nTimes);
        }
    }
}