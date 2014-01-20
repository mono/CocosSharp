namespace CocosSharp
{
    public class CCEaseBackOut : CCActionEase
    {
        #region Constructors

        public CCEaseBackOut(CCActionInterval pAction) : base(pAction)
        {
        }

        protected CCEaseBackOut(CCEaseBackOut easeBackOut) : base(easeBackOut)
        {
        }

        #endregion Constructors


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
            return new CCEaseBackOut(this);
        }
    }
}