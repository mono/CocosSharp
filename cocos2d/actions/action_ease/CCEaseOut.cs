using System;

namespace Cocos2D
{
    public class CCEaseOut : CCEaseRateAction
    {
        public CCEaseOut(CCActionInterval pAction, float fRate) : base(pAction, fRate)
        {
        }

        public CCEaseOut(CCEaseOut easeOut) : base(easeOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update((float) (Math.Pow(time, 1 / m_fRate)));
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCEaseOut) (pZone);
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()), m_fRate);

                return pCopy;
            }
            return new CCEaseOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseOut((CCActionInterval) m_pInner.Reverse(), 1 / m_fRate);
        }
    }
}