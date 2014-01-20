namespace CocosSharp
{
    public class CCEaseBounceIn : CCActionEase
    {
        #region Constructors

        public CCEaseBounceIn(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseBounceIn(CCEaseBounceIn easeBounceIn) : base(easeBounceIn)
        {
        }

        #endregion Constructors


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
            return new CCEaseBounceIn(this);
        }
    }
}