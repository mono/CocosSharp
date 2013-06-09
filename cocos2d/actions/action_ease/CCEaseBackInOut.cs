namespace Cocos2D
{
    public class CCEaseBackInOut : CCActionEase
    {
        public CCEaseBackInOut(CCActionInterval pAction) : base(pAction)
        {
        }

        protected CCEaseBackInOut(CCEaseBackInOut easeBackInOut) : base(easeBackInOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.BackInOut(time));
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBackInOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseBackInOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBackInOut((CCActionInterval) m_pInner.Reverse());
        }
    }
}