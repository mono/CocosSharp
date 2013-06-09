using System;

namespace Cocos2D
{
    public class CCEaseExponentialIn : CCActionEase
    {
        public CCEaseExponentialIn(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseExponentialIn(CCEaseExponentialIn easeExponentialIn) : base(easeExponentialIn)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.ExponentialIn(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseExponentialOut((CCActionInterval) m_pInner.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseExponentialIn;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseExponentialIn(this);
        }
    }
}