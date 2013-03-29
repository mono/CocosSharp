using System;

namespace cocos2d
{
    public class CCEaseIn : CCEaseRateAction
    {
        public override void Update(float time)
        {
            m_pOther.Update((float) Math.Pow(time, m_fRate));
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseIn pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseIn;
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