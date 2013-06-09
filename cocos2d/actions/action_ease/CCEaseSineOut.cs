using System;

namespace Cocos2D
{
    public class CCEaseSineOut : CCActionEase
    {
        public CCEaseSineOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseSineOut(CCEaseSineOut easeSineOut) : base(easeSineOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.SineOut(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseSineIn((CCActionInterval) m_pInner.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseSineOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseSineOut(this);
        }
    }
}