using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCEaseElasticOut : CCEaseElastic
    {
        #region Constructors

        public CCEaseElasticOut(CCActionInterval pAction) : base(pAction, 0.3f)
        {
        }

        public CCEaseElasticOut(CCActionInterval pAction, float fPeriod) : base(pAction, fPeriod)
        {
        }

        protected CCEaseElasticOut(CCEaseElasticOut easeElasticOut) : base(easeElasticOut)
        {
        }

        #endregion Constructors


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
            return new CCEaseElasticOut(this);
        }
    }
}