
using System;

namespace cocos2d
{
    public class CCAccelDeccelAmplitude : CCActionInterval
    {
        protected float m_fRate;
        protected CCActionInterval m_pOther;

        public float Rate
        {
            get { return m_fRate; }
            set { m_fRate = value; }
        }

        public bool InitWithAction(CCAction pAction, float duration)
        {
            if (base.InitWithDuration(duration))
            {
                m_fRate = 1.0f;
                m_pOther = pAction as CCActionInterval;

                return true;
            }

            return false;
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_pOther.StartWithTarget(target);
        }

        public override void Update(float time)
        {
            float f = time * 2;

            if (f > 1)
            {
                f -= 1;
                f = 1 - f;
            }

            ((m_pOther)).AmplitudeRate = (float) Math.Pow(f, m_fRate);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_pOther.Reverse(), m_fDuration);
        }

        public static CCAccelDeccelAmplitude Create(CCAction pAction, float duration)
        {
            var pRet = new CCAccelDeccelAmplitude();
            pRet.InitWithAction(pAction, duration);
            return pRet;
        }
    }
}