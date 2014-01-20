namespace CocosSharp
{
    public class CCEaseBounceOut : CCActionEase
    {
        #region Constructors

        public CCEaseBounceOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseBounceOut(CCEaseBounceOut easeBounceOut) : base(easeBounceOut)
        {
        }

        #endregion Constructors


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
            return new CCEaseBounceOut(this);
        }
    }
}