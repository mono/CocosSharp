
using System;

namespace cocos2d
{
    public class CCEaseOut : CCEaseRateAction
    {
        public override void Update(float time)
        {
            m_pOther.Update((float) (Math.Pow(time, 1 / m_fRate)));
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCEaseOut pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseOut) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCEaseOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fRate);

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval)m_pOther.Reverse(), 1 / m_fRate);
        }

        public new static CCEaseOut Create(CCActionInterval pAction, float fRate)
        {
            var pRet = new CCEaseOut();
            pRet.InitWithAction(pAction, fRate);
            return pRet;
        }
    }
}