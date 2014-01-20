using System;

namespace CocosSharp
{
    public class CCEaseExponentialOut : CCActionEase
    {
        #region Constructors

        public CCEaseExponentialOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseExponentialOut(CCEaseExponentialOut easeExponentialOut) : base(easeExponentialOut)
        {
        }

        #endregion Constructors


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
            return new CCEaseExponentialOut(this);
        }
    }
}