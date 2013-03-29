
using System;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCEaseElasticOut : CCEaseElastic
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
                float s = m_fPeriod / 4;
                newT = (float) (Math.Pow(2, -10 * time) * Math.Sin((time - s) * MathHelper.Pi * 2f / m_fPeriod) + 1);
            }

            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseElasticIn.Create((CCActionInterval) m_pOther.Reverse(), m_fPeriod);
        }

        public override object CopyWithZone(CCZone pZone)
        {
            CCEaseElasticOut pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseElasticOut;
            }
            else
            {
                pCopy = new CCEaseElasticOut();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fPeriod);

            return pCopy;
        }

        public new static CCEaseElasticOut Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseElasticOut();
            pRet.InitWithAction(pAction);
            return pRet;
        }


        public new static CCEaseElasticOut Create(CCActionInterval pAction, float fPeriod)
        {
            var pRet = new CCEaseElasticOut();
            pRet.InitWithAction(pAction, fPeriod);
            return pRet;
        }
    }
}