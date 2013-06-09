using System;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCEaseElasticOut : CCEaseElastic
    {
        public CCEaseElasticOut(CCActionInterval pAction) : base(pAction, 0.3f)
        {
        }

        public CCEaseElasticOut(CCActionInterval pAction, float fPeriod) : base(pAction, fPeriod)
        {
        }

        protected CCEaseElasticOut(CCEaseElasticOut easeElasticOut) : base(easeElasticOut)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.ElasticOut(time, m_fPeriod));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseElasticIn((CCActionInterval) m_pInner.Reverse(), m_fPeriod);
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseElasticOut;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()), m_fPeriod);

                return pCopy;
            }
            return new CCEaseElasticOut(this);
        }
    }
}