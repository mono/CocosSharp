using System;

namespace Cocos2D
{
    public class CCEaseSineInOut : CCActionEase
    {
        public CCEaseSineInOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseSineInOut(CCEaseSineInOut easeSineInOut) : base(easeSineInOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.SineInOut(time));
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCEaseSineInOut) (pZone);
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseSineInOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseSineInOut((CCActionInterval) m_pInner.Reverse());
        }
    }
}