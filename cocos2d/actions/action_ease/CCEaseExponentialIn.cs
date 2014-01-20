using System;

namespace CocosSharp
{
    public class CCEaseExponentialIn : CCActionEase
    {
        #region Constructors

        public CCEaseExponentialIn(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseExponentialIn(CCEaseExponentialIn easeExponentialIn) : base(easeExponentialIn)
        {
        }

        #endregion Constructors


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
            return new CCEaseExponentialIn(this);
        }
    }
}