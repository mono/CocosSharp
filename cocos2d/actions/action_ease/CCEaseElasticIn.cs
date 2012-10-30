
using System;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCEaseElasticIn : CCEaseElastic
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
                time = time - 1;
                newT = -(float) (Math.Pow(2, 10 * time) * Math.Sin((time - s) * MathHelper.Pi * 2.0f / m_fPeriod));
            }

            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return CCEaseElasticOut.Create((CCActionInterval) m_pOther.Reverse(), m_fPeriod);
        }

        public override CCObject CopyWithZone(CCZone pZone)
        {
            CCEaseElasticIn pCopy;

            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = pZone.m_pCopyObject as CCEaseElasticIn;
            }
            else
            {
                pCopy = new CCEaseElasticIn();
            }

            pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fPeriod);

            return pCopy;
        }

        public new static CCEaseElasticIn Create(CCActionInterval pAction)
        {
            var pRet = new CCEaseElasticIn();
            pRet.InitWithAction(pAction);
            return pRet;
        }

        public new static CCEaseElasticIn Create(CCActionInterval pAction, float fPeriod)
        {
            var pRet = new CCEaseElasticIn();
            pRet.InitWithAction(pAction, fPeriod);
            return pRet;
        }
    }
}