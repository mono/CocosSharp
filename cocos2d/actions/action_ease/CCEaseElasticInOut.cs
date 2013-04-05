
using System;
using Microsoft.Xna.Framework;

namespace cocos2d
{
    public class CCEaseElasticInOut : CCEaseElastic
    {

		public CCEaseElasticInOut (CCActionInterval pAction) : base(pAction)
		{ }
		
		public CCEaseElasticInOut (CCActionInterval pAction, float fPeriod) : base (pAction, fPeriod)
		{ }
		
		protected CCEaseElasticInOut (CCEaseElasticInOut easeElasticInOut) : base (easeElasticInOut)
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
                time = time * 2;
                if (m_fPeriod == 0)
                {
                    m_fPeriod = 0.3f * 1.5f;
                }

                float s = m_fPeriod / 4;

                time = time - 1;
                if (time < 0)
                {
                    newT = (float) (-0.5f * Math.Pow(2, 10 * time) * Math.Sin((time - s) * MathHelper.TwoPi / m_fPeriod));
                }
                else
                {
                    newT = (float) (Math.Pow(2, -10 * time) * Math.Sin((time - s) * MathHelper.TwoPi / m_fPeriod) * 0.5f + 1);
                }
            }

            m_pOther.Update(newT);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseElasticInOut((CCActionInterval)m_pOther.Reverse(), m_fPeriod);
        }

        public override object Copy(ICopyable pZone)
        {

            if (pZone != null)
            {
                //in case of being called at sub class
                var pCopy = pZone as CCEaseElasticInOut;
				pCopy.InitWithAction((CCActionInterval) (m_pOther.Copy()), m_fPeriod);
				
				return pCopy;
			}
            else
            {
                return new CCEaseElasticInOut(this);
            }

        }

    }
}