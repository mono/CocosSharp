
namespace cocos2d
{

    public class CCProgressFromTo : CCActionInterval
    {
        protected float m_fFrom;
        protected float m_fTo;

        /// <summary>
        /// Initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        public bool InitWithDuration(float duration, float fFromPercentage, float fToPercentage)
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

        public override object Copy(ICopyable pZone)
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
                pZone =  (pCopy);
            }

            base.Copy(pZone);
            pCopy.InitWithDuration(m_fDuration, m_fFrom, m_fTo);

            return pCopy;
        }

        public override CCFiniteTimeAction Reverse()
        {
            return Create(m_fDuration, m_fTo, m_fFrom);
        }

        public override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            ((CCProgressTimer) (m_pTarget)).Percentage = m_fFrom;
        }

        public override void Update(float time)
        {
            ((CCProgressTimer) (m_pTarget)).Percentage = m_fFrom + (m_fTo - m_fFrom) * time;
        }

        /// <summary>
        /// Creates and initializes the action with a duration, a "from" percentage and a "to" percentage
        /// </summary>
        public static CCProgressFromTo Create(float duration, float fFromPercentage, float fToPercentage)
        {
            var pProgressFromTo = new CCProgressFromTo();
            pProgressFromTo.InitWithDuration(duration, fFromPercentage, fToPercentage);
            return pProgressFromTo;
        }
    }
}