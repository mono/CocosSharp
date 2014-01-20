using System;

namespace CocosSharp
{
    public class CCEaseExponentialInOut : CCActionEase
    {
        #region Constructors

        public CCEaseExponentialInOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseExponentialInOut(CCEaseExponentialInOut easeExponentialInOut) : base(easeExponentialInOut)
        {
        }

        #endregion Constructors


        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.ExponentialInOut(time));
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseExponentialInOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseExponentialInOut((CCActionInterval) m_pInner.Reverse());
        }
    }
}