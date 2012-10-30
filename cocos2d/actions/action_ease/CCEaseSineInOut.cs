
using System;

namespace cocos2d
{
    public class CCEaseSineInOut : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(-0.5f * ((float) Math.Cos((float) Math.PI * time) - 1));
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCEaseSineInOut pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseSineInOut) (pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCEaseSineInOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval)m_pOther.Reverse());
        }

        public new static CCEaseSineInOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseSineInOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}