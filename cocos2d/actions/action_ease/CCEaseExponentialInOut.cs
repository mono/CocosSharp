
using System;

namespace cocos2d
{
    public class CCEaseExponentialInOut : CCActionEase
    {
        public override void Update(float time)
        {
            if (time == 0 || time == 1)
            {
                m_pOther.Update(time);
            }
            else
            {
                time /= 0.5f;

                if (time < 1)
                {
                    time = 0.5f * (float) Math.Pow(2, 10 * (time - 1));
                }
                else
                {
                    time = 0.5f * (-(float) Math.Pow(2, -10 * (time - 1)) + 2);
                }

                m_pOther.Update(time);
            }
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseExponentialInOut pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseExponentialInOut;
            }
            else
            {
                pCopy = new CCEaseExponentialInOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval)m_pOther.Reverse());
        }

        public new static CCEaseExponentialInOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseExponentialInOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}