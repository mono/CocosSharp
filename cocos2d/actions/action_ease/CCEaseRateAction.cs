
namespace cocos2d
{
    public class CCEaseRateAction : CCActionEase
    {
        protected float m_fRate;

        public float Rate
        {
            get { return m_fRate; }
            set { m_fRate = value; }
        }

        public bool InitWithAction(CCActionInterval pAction, float fRate)
        {
            if (base.InitWithAction(pAction))
            {
                m_fRate = fRate;
                return true;
            }

            return false;
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCEaseRateAction pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseRateAction) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCEaseRateAction();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fRate);

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval) m_pOther.Reverse(), 1 / m_fRate);
        }

        public static CCEaseRateAction Create(CCActionInterval pAction, float fRate)
        {
            var pRet = new CCEaseRateAction();
            pRet.InitWithAction(pAction, fRate);
            return pRet;
        }
    }
}