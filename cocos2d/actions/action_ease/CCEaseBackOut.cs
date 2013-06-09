namespace Cocos2D
{
    public class CCEaseBackOut : CCActionEase
    {
        public CCEaseBackOut(CCActionInterval pAction) : base(pAction)
        {
        }

        protected CCEaseBackOut(CCEaseBackOut easeBackOut) : base(easeBackOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.BackOut(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBackIn((CCActionInterval) m_pInner.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseBackOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseBackOut(this);
        }
    }
}