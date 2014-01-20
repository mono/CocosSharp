using System;

namespace CocosSharp
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


        #region Constructors

        public CCAccelDeccelAmplitude(CCAction pAction, float duration) : base(duration)
        {
            InitWithAction(pAction);
        }

        private void InitWithAction(CCAction pAction)
        {
            m_fRate = 1.0f;
            m_pOther = pAction as CCActionInterval;
        }

        #endregion Constructors


        protected internal override void StartWithTarget(CCNode target)
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
            return new CCAccelDeccelAmplitude(m_pOther.Reverse(), m_fDuration);
        }
    }
}