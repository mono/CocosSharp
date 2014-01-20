namespace CocosSharp
{
    public class CCEaseElastic : CCActionEase
    {
        protected float m_fPeriod;

        public float Period
        {
            get { return m_fPeriod; }
            set { m_fPeriod = value; }
        }


        #region Constructors

        public CCEaseElastic(CCActionInterval pAction, float fPeriod) : base(pAction)
        {
            InitWithAction(pAction, fPeriod);
        }

        public CCEaseElastic(CCActionInterval pAction) : this(pAction, 0.3f)
        {
        }

        // Perform a deep copy of CCEaseElastic
        protected CCEaseElastic(CCEaseElastic easeElastic) : base(easeElastic)
        {
            InitWithAction((CCActionInterval) (easeElastic.m_pInner.Copy()), easeElastic.m_fPeriod);
        }

        private void InitWithAction(CCActionInterval pAction, float fPeriod)
        {
            m_fPeriod = fPeriod;
        }

        #endregion Constructors


        public override CCFiniteTimeAction Reverse()
        {
            //assert(0);
            return null;
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseElastic(this);
        }
    }
}