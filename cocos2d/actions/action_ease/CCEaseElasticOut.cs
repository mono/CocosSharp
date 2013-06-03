
using System;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCEaseElasticOut : CCEaseElastic
    {

		public CCEaseElasticOut (CCActionInterval pAction) : base(pAction)
		{ }
		
		public CCEaseElasticOut (CCActionInterval pAction, float fPeriod) : base (pAction, fPeriod)
		{ }
		
		protected CCEaseElasticOut (CCEaseElasticOut easeElasticOut) : base (easeElasticOut)
		{ }

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
            return new CCEaseElasticIn((CCActionInterval) m_pOther.Reverse(), m_fPeriod);
        }

        public override object Copy(ICCCopyable pZone)
        {
            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseElasticOut;
				pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fPeriod);
				
				return pCopy;
			}
            else
            {
                return new CCEaseElasticOut(this);
            }

        }

    }
}