namespace CocosSharp
{
    public class CCBlink : CCActionInterval
    {
        protected uint m_nTimes;
        protected bool m_bOriginalState;


        #region Constructors

        public CCBlink(float duration, uint uBlinks) : base(duration)
        {
            InitCCBlink(uBlinks);
        }

        // Perform deep copy of CCBlink
        protected CCBlink(CCBlink blink) : base(blink)
        {
            InitCCBlink(blink.m_nTimes);
        }

        private void InitCCBlink(uint uBlinks)
        {
            m_nTimes = uBlinks;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCBlink(this);
        }

        public override void Stop()
        {
            m_pTarget.Visible = m_bOriginalState;
            base.Stop();
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_bOriginalState = target.Visible;
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