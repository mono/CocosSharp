using System;

namespace CocosSharp
{
    public class CCEaseSineOut : CCActionEase
    {
        #region Constructors

        public CCEaseSineOut(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseSineOut(CCEaseSineOut easeSineOut) : base(easeSineOut)
        {
        }

        #endregion Constructors


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
            return new CCEaseSineOut(this);
        }
    }
}