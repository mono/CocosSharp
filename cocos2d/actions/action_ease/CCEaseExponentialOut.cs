
using System;

namespace cocos2d
{
    public class CCEaseExponentialOut : CCActionEase
    {

        public CCEaseExponentialOut (CCActionInterval pAction) : base (pAction)
        { }

        public CCEaseExponentialOut (CCEaseExponentialOut easeExponentialOut) : base (easeExponentialOut)
        { }

        public override void Update(float time)
        {
            m_pOther.Update(time == 1 ? 1 : time == 0 ? 0 : (-(float) Math.Pow(2, -10 * time / 1) + 1));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseExponentialIn((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseExponentialOut;
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseExponentialOut(this);
            }

        }


    }
}