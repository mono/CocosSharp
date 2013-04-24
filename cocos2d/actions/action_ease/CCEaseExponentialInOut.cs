
using System;

namespace cocos2d
{
    public class CCEaseExponentialInOut : CCActionEase
    {

        public CCEaseExponentialInOut (CCActionInterval pAction) : base (pAction)
        { }

        public CCEaseExponentialInOut (CCEaseExponentialInOut easeExponentialInOut) : base (easeExponentialInOut)
        { }

        public override void Update(float time)
        {
            if (time == 0 || time == 1)
            {
                m_pOther.Update(time);
            }
            else
            {
                time /= 0.5f;

                if (time < 1)
                {
                    time = 0.5f * (float) Math.Pow(2, 10 * (time - 1));
                }
                else
                {
                    time = 0.5f * (-(float) Math.Pow(2, -10 * (time - 1)) + 2);
                }

                m_pOther.Update(time);
            }
        }

        public override object Copy(ICopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseExponentialInOut;
                pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()));
                
                return pCopy;
            }
            else
            {
                return new CCEaseExponentialInOut(this);
            }

        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseExponentialInOut((CCActionInterval)m_pOther.Reverse());
        }


    }
}