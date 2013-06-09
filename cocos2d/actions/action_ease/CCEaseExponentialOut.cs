using System;

namespace Cocos2D
{
    public class CCEaseExponentialOut : CCActionEase
    {
        public CCEaseExponentialOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseExponentialOut(CCEaseExponentialOut easeExponentialOut) : base(easeExponentialOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.ExponentialOut(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseExponentialIn((CCActionInterval) m_pInner.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseExponentialOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseExponentialOut(this);
        }
    }
}