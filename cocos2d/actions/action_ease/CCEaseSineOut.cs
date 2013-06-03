
using System;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCEaseSineOut : CCActionEase
    {

        public CCEaseSineOut (CCActionInterval pAction) : base (pAction)
        { }

        public CCEaseSineOut (CCEaseSineOut easeSineOut) : base (easeSineOut)
        { }

        public override void Update(float time)
        {
            m_pOther.Update((float)Math.Sin(time * MathHelper.PiOver2));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseSineIn((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseSineOut;
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseSineOut(this);
            }

        }

    }
}