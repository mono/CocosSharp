using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCEaseElasticInOut : CCEaseElastic
    {
        #region Constructors

        public CCEaseElasticInOut(CCActionInterval pAction) : base(pAction, 0.3f)
        {
        }

        public CCEaseElasticInOut(CCActionInterval pAction, float fPeriod) : base(pAction, fPeriod)
        {
        }

        protected CCEaseElasticInOut(CCEaseElasticInOut easeElasticInOut) : base(easeElasticInOut)
        {
        }

        #endregion Constructors


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
            return new CCEaseElasticInOut(this);
        }
    }
}