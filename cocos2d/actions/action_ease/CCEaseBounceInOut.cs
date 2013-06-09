namespace Cocos2D
{
    public class CCEaseBounceInOut : CCActionEase
    {
        public CCEaseBounceInOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseBounceInOut(CCEaseBounceInOut easeBounceInOut) : base(easeBounceInOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.BounceInOut(time));
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBounceInOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseBounceInOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBounceInOut((CCActionInterval) m_pInner.Reverse());
        }
    }
}