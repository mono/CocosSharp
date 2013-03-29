
using System;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCEaseElasticInOut : CCEaseElastic
    {
        public override void Update(float time)
        {
            float newT;

            if (time == 0 || time == 1)
            {
                newT = time;
            }
            else
            {
                time = time * 2;
                if (m_fPeriod == 0)
                {
                    m_fPeriod = 0.3f * 1.5f;
                }

                float s = m_fPeriod / 4;

                time = time - 1;
                if (time < 0)
                {
                    newT = (float) (-0.5f * Math.Pow(2, 10 * time) * Math.Sin((time - s) * MathHelper.TwoPi / m_fPeriod));
                }
                else
                {
                    newT = (float) (Math.Pow(2, -10 * time) * Math.Sin((time - s) * MathHelper.TwoPi / m_fPeriod) * 0.5f + 1);
                }
            }

            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create((CCActionInterval)m_pOther.Reverse(), m_fPeriod);
        }

        public override object Copy(ICopyable pZone)
        {
            CCEaseElasticInOut pCopy;

            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = pZone as CCEaseElasticInOut;
            }
            else
            {
                pCopy = new CCEaseElasticInOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fPeriod);

            return pCopy;
        }

        public new static CCEaseElasticInOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseElasticInOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }

        public new static CCEaseElasticInOut Create(CCActionInterval pAction, float fPeriod)
        {
            var pRet = new CCEaseElasticInOut();
            pRet.InitWithAction(pAction, fPeriod);
            return pRet;
        }
    }
}