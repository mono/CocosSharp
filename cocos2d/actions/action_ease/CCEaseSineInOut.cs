
using System;

namespace Cocos2D
{
    public class CCEaseSineInOut : CCActionEase
    {

        public CCEaseSineInOut (CCActionInterval pAction) : base (pAction)
        { }

        public CCEaseSineInOut (CCEaseSineInOut easeSineInOut) : base (easeSineInOut)
        { }

        public override void Update(float time)
        {
            m_pOther.Update(-0.5f * ((float) Math.Cos((float) Math.PI * time) - 1));
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = (CCEaseSineInOut) (pZone);
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseSineInOut(this);
            }

        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseSineInOut((CCActionInterval)m_pOther.Reverse());
        }

    }
}