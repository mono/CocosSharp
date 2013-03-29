using System;

namespace cocos2d
{
    public class CCEaseIn : CCEaseRateAction
    {
        public override void Update(float time)
        {
            m_pOther.Update((float) Math.Pow(time, m_fRate));
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCEaseIn pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseIn;
            }
            else
            {
                pCopy = new CCEaseIn();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fRate);

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval) m_pOther.Reverse(), 1 / m_fRate);
        }

        public new static CCEaseIn Create(CCActionInterval pAction, float fRate)
        {
            var pRet = new CCEaseIn();
            pRet.InitWithAction(pAction, fRate);
            return pRet;
        }
    }
}