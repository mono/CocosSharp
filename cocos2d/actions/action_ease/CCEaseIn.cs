using System;

namespace CocosSharp
{
    public class CCEaseIn : CCEaseRateAction
    {
        #region Constructors

        public CCEaseIn(CCActionInterval pAction, float fRate) : base(pAction, fRate)
        {
        }

        public CCEaseIn(CCEaseIn easeIn) : base(easeIn)
        {
        }

        #endregion Constructors


        public override void Update(float time)
        {
            m_pInner.Update((float) Math.Pow(time, m_fRate));
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseIn(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseIn((CCActionInterval) m_pInner.Reverse(), 1 / m_fRate);
        }
    }
}