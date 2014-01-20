namespace CocosSharp
{
    public class CCProgressTo : CCActionInterval
    {
        protected float m_fFrom;
        protected float m_fTo;


        #region Constructors

        protected CCProgressTo()
        {
        }

        /// <summary>
        /// Creates and initializes with a duration and a percent
        /// </summary>
        public CCProgressTo(float duration, float fPercent) : base(duration)
        {
            InitCCProgressTo(fPercent);
        }

        // Perform deep copy of CCProgressTo
        public CCProgressTo(CCProgressTo progress) : base(progress)
        {
            InitCCProgressTo(progress.m_fTo);
        }

        /// <summary>
        /// Initializes with a duration and a percent
        /// </summary>
        private void InitCCProgressTo(float fPercent)
        {
            m_fTo = fPercent;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable pZone)
        {
            return new CCProgressTo(this);
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_fFrom = ((CCProgressTimer) (target)).Percentage;
            // XXX: Is this correct ?
            // Adding it to support CCRepeat
            if (m_fFrom == 100)
            {
                m_fFrom = 0;
            }
        }

        public override void Update(float time)
        {
            ((CCProgressTimer) m_pTarget).Percentage = m_fFrom + (m_fTo - m_fFrom) * time;
        }
    }
}