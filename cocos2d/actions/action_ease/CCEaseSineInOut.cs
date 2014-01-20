using System;

namespace CocosSharp
{
    public class CCEaseSineInOut : CCActionEase
    {
        #region Constructors

        public CCEaseSineInOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseSineInOut(CCEaseSineInOut easeSineInOut) : base(easeSineInOut)
        {
        }

        #endregion Constructors


        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.SineInOut(time));
        }

        public override object Copy(ICCCopyable pZone)
        {
            return new CCEaseSineInOut(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseSineInOut((CCActionInterval) m_pInner.Reverse());
        }
    }
}