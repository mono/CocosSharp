
using System;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCEaseSineIn : CCActionEase
    {

        public CCEaseSineIn (CCActionInterval pAction) : base (pAction)
        { }

        public CCEaseSineIn (CCEaseSineIn easesineIn) : base (easesineIn)
        { }

        public override void Update(float time)
        {
            m_pOther.Update(-1 * (float) Math.Cos(time * MathHelper.PiOver2) + 1);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseSineOut((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCEaseSineIn) (pZone);
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseSineIn(this);
            }

        }


    }
}