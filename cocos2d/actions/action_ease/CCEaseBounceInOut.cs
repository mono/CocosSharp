namespace CocosSharp
{
    public class CCEaseBounceInOut : CCActionEase
    {
        #region Constructors

        public CCEaseBounceInOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseBounceInOut(CCEaseBounceInOut easeBounceInOut) : base(easeBounceInOut)
        {
        }

        #endregion Constructors


        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.BounceInOut(time));
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseBounceInOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBounceInOut((CCActionInterval) m_pInner.Reverse());
        }
    }
}