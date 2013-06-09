namespace Cocos2D
{
    public class CCEaseBounceIn : CCActionEase
    {
        public CCEaseBounceIn(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseBounceIn(CCEaseBounceIn easeBounceIn) : base(easeBounceIn)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.BounceIn(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBounceOut((CCActionInterval) m_pInner.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBounceIn;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseBounceIn(this);
        }
    }
}