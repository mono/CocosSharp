namespace Cocos2D
{
    public class CCEaseBounceOut : CCActionEase
    {
        public CCEaseBounceOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseBounceOut(CCEaseBounceOut easeBounceOut) : base(easeBounceOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.BounceOut(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBounceIn((CCActionInterval) m_pInner.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBounceOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseBounceOut(this);
        }
    }
}