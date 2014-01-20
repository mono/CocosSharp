using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCEaseElasticIn : CCEaseElastic
    {
        #region Constructors

        public CCEaseElasticIn(CCActionInterval pAction) : base(pAction, 0.3f)
        {
        }

        public CCEaseElasticIn(CCActionInterval pAction, float fPeriod) : base(pAction, fPeriod)
        {
        }

        protected CCEaseElasticIn(CCEaseElasticIn easeElasticIn) : base(easeElasticIn)
        {
        }

        #endregion Constructors


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
            return new CCEaseElasticIn(this);
        }
    }
}