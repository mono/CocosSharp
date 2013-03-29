
using System;

namespace cocos2d
{
    public class CCEaseExponentialOut : CCActionEase
    {
        public override void Update(float time)
        {
            m_pOther.Update(time == 1 ? 1 : time == 0 ? 0 : (-(float) Math.Pow(2, -10 * time / 1) + 1));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseExponentialIn.Create((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseExponentialOut pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseExponentialOut;
            }
            else
            {
                pCopy = new CCEaseExponentialOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));

            return pCopy;
        }

        public new static CCEaseExponentialOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseExponentialOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }
    }
}