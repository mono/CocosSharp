
using System;
using Microsoft.Xna.Framework;

namespace Cocos2D
{
    public class CCEaseElasticIn : CCEaseElastic
    {

		public CCEaseElasticIn (CCActionInterval pAction) : base(pAction)
		{ }
		
		public CCEaseElasticIn (CCActionInterval pAction, float fPeriod) : base (pAction, fPeriod)
		{ }

		protected CCEaseElasticIn (CCEaseElasticIn easeElasticIn) : base (easeElasticIn)
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
                time = time - 1;
                newT = -(float) (Math.Pow(2, 10 * time) * Math.Sin((time - s) * MathHelper.Pi * 2.0f / m_fPeriod));
            }

            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseElasticOut((CCActionInterval) m_pOther.Reverse(), m_fPeriod);
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseElasticIn;
				pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fPeriod);
				
				return pCopy;
			}
            else
            {
                return new CCEaseElasticIn(this);
            }

        }

    }
}