using System;

namespace CocosSharp
{
    public class CCDeccelAmplitude : CCActionInterval
    {
        protected float m_fRate;
        protected CCActionInterval m_pOther;

        public float Rate
        {
            get { return m_fRate; }
            set { m_fRate = value; }
        }


        #region Constructors

        public CCDeccelAmplitude(CCAction pAction, float duration) : base(duration)
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
            ((m_pOther)).AmplitudeRate = (float) Math.Pow((1 - time), m_fRate);
            m_pOther.Update(time);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCDeccelAmplitude(m_pOther.Reverse(), m_fDuration);
        }
    }
}