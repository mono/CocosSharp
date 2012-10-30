
using System;

namespace cocos2d
{
    public class CCEaseExponentialIn : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(time == 0 ? 0 : time == 1 ? 1 : (float) Math.Pow(2, 10 * (time / 1 - 1)) - 1 * 0.001f);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseExponentialOut.Create((CCActionInterval) m_pOther.Reverse());
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCEaseExponentialIn pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseExponentialIn;
            }
            else
            {
                pCopy = new CCEaseExponentialIn();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public new static CCEaseExponentialIn Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseExponentialIn();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}