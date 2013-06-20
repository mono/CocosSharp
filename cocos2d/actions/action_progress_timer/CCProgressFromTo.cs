namespace Cocos2D
{
    public class CCProgressFromTo : CCActionInterval
    {
        protected float m_fFrom;
        protected float m_fTo;

        /// <summary>
        /// Initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        protected virtual bool InitWithDuration(float duration, float fFromPercentage, float fToPercentage)
        {
            // if (CCActionInterval::initWithDuration(duration))
            if (InitWithDuration(duration))
            {
                m_fTo = fToPercentage;
                m_fFrom = fFromPercentage;
                return true;
            }

            return false;
        }

        public override object Copy(ICCCopyable pZone)
        {
            CCProgressFromTo pCopy;
            if (pZone != null)
            {
                //in case of being called at sub class
                pCopy = (CCProgressFromTo) (pZone);
            }
            else
            {
                pCopy = new CCProgressFromTo();
                pZone = (pCopy);
            }

            base.Copy(pZone);
            pCopy.InitWithDuration(m_fDuration, m_fFrom, m_fTo);

            return pCopy;
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

        protected CCProgressFromTo()
        {
        }

        /// <summary>
        /// Creates and initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        public CCProgressFromTo(float duration, float fFromPercentage, float fToPercentage) : base(duration)
        {
            InitWithDuration(duration, fFromPercentage, fToPercentage);
        }
    }
}