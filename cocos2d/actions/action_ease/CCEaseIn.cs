using System;

namespace Cocos2D
{
    public class CCEaseIn : CCEaseRateAction
    {
        public CCEaseIn(CCActionInterval pAction, float fRate) : base(pAction, fRate)
        {
        }

        public CCEaseIn(CCEaseIn easeIn) : base(easeIn)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update((float) Math.Pow(time, m_fRate));
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseIn;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()), m_fRate);

                return pCopy;
            }
            return new CCEaseIn(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseIn((CCActionInterval) m_pInner.Reverse(), 1 / m_fRate);
        }
    }
}