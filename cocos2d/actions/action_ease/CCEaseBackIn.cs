namespace CocosSharp
{
    public class CCEaseBackIn : CCActionEase
    {
        #region Constructors

        public CCEaseBackIn(CCActionInterval pAction) : base(pAction)
        {
        }

        protected CCEaseBackIn(CCEaseBackIn easeBackIn) : base(easeBackIn)
        {
        }

        #endregion Constructors


        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.BackIn(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseBackOut((CCActionInterval) m_pInner.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseBackIn(this);
        }
    }
}