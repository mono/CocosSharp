using System;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCEaseSineIn : CCActionEase
    {
        public CCEaseSineIn(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseSineIn(CCEaseSineIn easesineIn) : base(easesineIn)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.SineIn(time));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseSineOut((CCActionInterval) m_pInner.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCEaseSineIn) (pZone);
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()));

                return pCopy;
            }
            return new CCEaseSineIn(this);
        }
    }
}