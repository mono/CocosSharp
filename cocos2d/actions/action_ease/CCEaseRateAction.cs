namespace Cocos2D
{
    public class CCEaseRateAction : CCActionEase
    {
        protected float m_fRate;

        public CCEaseRateAction(CCActionInterval pAction, float fRate) : base(pAction)
        {
            m_fRate = fRate;
        }

        public CCEaseRateAction(CCEaseRateAction easeRateAction) : base(easeRateAction)
        {
            InitWithAction((CCActionInterval) (easeRateAction.m_pInner.Copy()), easeRateAction.m_fRate);
        }

        public float Rate
        {
            get { return m_fRate; }
            set { m_fRate = value; }
        }

        protected bool InitWithAction(CCActionInterval pAction, float fRate)
        {
            if (base.InitWithAction(pAction))
            {
                m_fRate = fRate;
                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCEaseRateAction) (pZone);
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()), m_fRate);

                return pCopy;
            }
            return new CCEaseRateAction(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseRateAction((CCActionInterval) m_pInner.Reverse(), 1 / m_fRate);
        }
    }
}