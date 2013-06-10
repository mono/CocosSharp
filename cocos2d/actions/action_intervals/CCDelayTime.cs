namespace Cocos2D
{
    public class CCDelayTime : CCActionInterval
    {
        public CCDelayTime(float d)
        {
            InitWithDuration(d);
        }

        protected CCDelayTime(CCDelayTime delayTime) : base(delayTime)
        {
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCDelayTime) (pZone);
                base.Copy(pZone);

                return pCopy;
            }
            else
            {
                return new CCDelayTime(this);
            }
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