namespace CocosSharp
{
    public class CCProgressFromTo : CCActionInterval
    {
        protected float m_fFrom;
        protected float m_fTo;


        #region Constructors

        protected CCProgressFromTo()
        {
        }

        /// <summary>
        /// Creates and initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        public CCProgressFromTo(float duration, float fFromPercentage, float fToPercentage) : base(duration)
        {
            InitCCProgressFromTo(fFromPercentage, fToPercentage);
        }

        // Perform deep copy of CCProgressFromTo
        public CCProgressFromTo(CCProgressFromTo progress) : base(progress)
        {
            InitCCProgressFromTo(progress.m_fFrom, progress.m_fTo);
        }

        /// <summary>
        /// Initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        private void InitCCProgressFromTo(float fFromPercentage, float fToPercentage)
        {
            m_fTo = fToPercentage;
            m_fFrom = fFromPercentage;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCProgressFromTo(this);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCProgressFromTo(m_fDuration, m_fTo, m_fFrom);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            ((CCProgressTimer) (m_pTarget)).Percentage = m_fFrom;
        }

        public override void Update(float time)
        {
            ((CCProgressTimer) (m_pTarget)).Percentage = m_fFrom + (m_fTo - m_fFrom) * time;
        }
    }
}