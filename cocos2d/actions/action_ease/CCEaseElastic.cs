
namespace cocos2d
{
    public class CCEaseElastic : CCActionEase
    {
        protected float m_fPeriod;

        public float Period
        {
            get { return m_fPeriod; }
            set { m_fPeriod = value; }
        }

        public bool InitWithAction(CCActionInterval pAction, float fPeriod)
        {
            if (base.InitWithAction(pAction))
            {
                m_fPeriod = fPeriod;
                return true;
            }

            return false;
        }

        public new bool InitWithAction(CCActionInterval pAction)
        {
            return InitWithAction(pAction, 0.3f);
        }

        public override CCFiniteTimeAction Reverse()
        {
            //assert(0);
            return null;
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCEaseElastic pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseElastic;
            }
            else
            {
                pCopy = new CCEaseElastic();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fPeriod);

            return pCopy;
        }


        public new static CCEaseElastic Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseElastic();
            pRet.InitWithAction(pAction);
            return pRet;
        }

        public static CCEaseElastic Create(CCActionInterval pAction, float fPeriod)
        {
            var pRet = new CCEaseElastic();
            pRet.InitWithAction(pAction, fPeriod);
            return pRet;
        }
    }
}