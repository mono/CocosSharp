using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCEaseSineIn : CCActionEase
    {
        #region Constructors

        public CCEaseSineIn(CCActionInterval pAction) : base(pAction)
        {
        }

        public CCEaseSineIn(CCEaseSineIn easesineIn) : base(easesineIn)
        {
        }

        #endregion Constructors


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
            return new CCEaseSineIn(this);
        }
    }
}