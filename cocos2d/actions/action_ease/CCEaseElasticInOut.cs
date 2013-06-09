using System;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCEaseElasticInOut : CCEaseElastic
    {
        public CCEaseElasticInOut(CCActionInterval pAction) : base(pAction, 0.3f)
        {
        }

        public CCEaseElasticInOut(CCActionInterval pAction, float fPeriod) : base(pAction, fPeriod)
        {
        }

        protected CCEaseElasticInOut(CCEaseElasticInOut easeElasticInOut) : base(easeElasticInOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.ElasticInOut(time, m_fPeriod));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseElasticInOut((CCActionInterval) m_pInner.Reverse(), m_fPeriod);
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseElasticInOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()), m_fPeriod);

                return pCopy;
            }
            return new CCEaseElasticInOut(this);
        }
    }
}