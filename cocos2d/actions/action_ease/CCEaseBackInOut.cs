namespace CocosSharp
{
    public class CCEaseBackInOut : CCActionEase
    {
        #region Constructors

        public CCEaseBackInOut(CCActionInterval pAction) : base(pAction)
        {
        }

        protected CCEaseBackInOut(CCEaseBackInOut easeBackInOut) : base(easeBackInOut)
        {
        }

        #endregion Constructors


        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.BackInOut(time));
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseBackInOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBackInOut((CCActionInterval) m_pInner.Reverse());
        }
    }
}