
namespace cocos2d
{
    public class CCEaseBounceInOut : CCEaseBounce
    {
        public override void Update(float time)
        {
            float newT;

            if (time < 0.5f)
            {
                time = time * 2;
                newT = (1 - BounceTime(1 - time)) * 0.5f;
            }
            else
            {
                newT = BounceTime(time * 2 - 1) * 0.5f + 0.5f;
            }

            m_pOther.Update(newT);
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCEaseBounceInOut pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseBounceInOut;
            }
            else
            {
                pCopy = new CCEaseBounceInOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseBounceInOut.Create((CCActionInterval)m_pOther.Reverse());
        }

        public new static CCEaseBounceInOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseBounceInOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}