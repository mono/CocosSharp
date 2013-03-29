
using System;

namespace cocos2d
{
    public class CCEaseSineInOut : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(-0.5f * ((float) Math.Cos((float) Math.PI * time) - 1));
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseSineInOut pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCEaseSineInOut) (pZone);
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