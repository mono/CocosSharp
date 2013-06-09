using System;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCEaseElasticIn : CCEaseElastic
    {
        public CCEaseElasticIn(CCActionInterval pAction) : base(pAction, 0.3f)
        {
        }

        public CCEaseElasticIn(CCActionInterval pAction, float fPeriod) : base(pAction, fPeriod)
        {
        }

        protected CCEaseElasticIn(CCEaseElasticIn easeElasticIn) : base(easeElasticIn)
        {
        }

        public override void Update(float time)
        {
            m_pInner.Update(CCEaseMath.ElasticIn(time, m_fPeriod));
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseElasticOut((CCActionInterval) m_pInner.Reverse(), m_fPeriod);
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseElasticIn;
                pCopy.InitWithAction((CCActionInterval) (m_pInner.Copy()), m_fPeriod);

                return pCopy;
            }
            return new CCEaseElasticIn(this);
        }
    }
}