namespace CocosSharp
{
    public class CCDelayTime : CCActionInterval
    {
        #region Constructors

        public CCDelayTime(float d) : base(d)
        {
        }

        protected CCDelayTime(CCDelayTime delayTime) : base(delayTime)
        {
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCDelayTime(this);
        }

        public override void Update(float time)
        {
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCDelayTime(m_fDuration);
        }
    }
}