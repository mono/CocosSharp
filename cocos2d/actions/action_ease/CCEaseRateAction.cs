namespace CocosSharp
{
    public class CCEaseRateAction : CCActionEase
    {
        protected float m_fRate;

        public float Rate
        {
            get { return m_fRate; }
            set { m_fRate = value; }
        }


        #region Constructors

        public CCEaseRateAction(CCActionInterval pAction, float fRate) : base(pAction)
        {
            InitWithRate(fRate);
        }

        // Perform deep copy of CCEaseRateAction
        public CCEaseRateAction(CCEaseRateAction easeRateAction) : base(easeRateAction)
        {
            InitWithRate(easeRateAction.m_fRate);
        }

        private void InitWithRate(float fRate)
        {
            m_fRate = fRate;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseRateAction(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseRateAction((CCActionInterval) m_pInner.Reverse(), 1 / m_fRate);
        }
    }
}