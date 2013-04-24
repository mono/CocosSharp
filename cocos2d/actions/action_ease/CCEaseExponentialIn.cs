
using System;

namespace cocos2d
{
    public class CCEaseExponentialIn : CCActionEase
    {

        public CCEaseExponentialIn (CCActionInterval pAction) : base (pAction)
        {  }

        public CCEaseExponentialIn (CCEaseExponentialIn easeExponentialIn) : base (easeExponentialIn)
        {  }

        public override void Update(float time)
        {
            m_pOther.Update(time == 0 ? 0 : time == 1 ? 1 : (float) Math.Pow(2, 10 * (time / 1 - 1)) - 1 * 0.001f);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseExponentialOut((CCActionInterval) m_pOther.Reverse());
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseExponentialIn;
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseExponentialIn(this);
            }

        }


    }
}