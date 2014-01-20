using System;

namespace CocosSharp
{
    public class CCEaseOut : CCEaseRateAction
    {
        #region Constructors

        public CCEaseOut(CCActionInterval pAction, float fRate) : base(pAction, fRate)
        {
        }

        public CCEaseOut(CCEaseOut easeOut) : base(easeOut)
        {
        }

        #endregion Constructors


        public override void Update(float time)
        {
            m_pInner.Update((float) (Math.Pow(time, 1 / m_fRate)));
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseOut((CCActionInterval) m_pInner.Reverse(), 1 / m_fRate);
        }
    }
}