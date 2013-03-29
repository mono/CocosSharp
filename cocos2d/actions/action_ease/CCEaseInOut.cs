
using System;

namespace cocos2d
{
    public class CCEaseInOut : CCEaseRateAction
    {
        public override void Update(float time)
        {
            time *= 2;

            if (time < 1)
            {
                m_pOther.Update(0.5f * (float) Math.Pow(time, m_fRate));
            }
            else
            {
                m_pOther.Update(1.0f - 0.5f * (float) Math.Pow(2 - time, m_fRate));
            }
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseInOut pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseInOut;
            }
            else
            {
                pCopy = new CCEaseInOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fRate);

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval) m_pOther.Reverse(), m_fRate);
        }


        public new static CCEaseInOut Create(CCActionInterval pAction, float fRate)
        {
            var pRet = new CCEaseInOut();
            pRet.InitWithAction(pAction, fRate);
            return pRet;
        }
    }
}