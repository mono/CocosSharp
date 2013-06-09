using System;

namespace Cocos2D
{
    public class CCEaseExponentialInOut : CCActionEase
    {
        public CCEaseExponentialInOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseExponentialInOut(CCEaseExponentialInOut easeExponentialInOut) : base(easeExponentialInOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.ExponentialInOut(time));
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseExponentialInOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseExponentialInOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseExponentialInOut((CCActionInterval) m_pInner.Reverse());
        }
    }
}