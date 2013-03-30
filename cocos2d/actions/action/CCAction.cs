
namespace cocos2d
{
    public enum ActionTag
    {
        //! Default tag
        kCCActionTagInvalid = -1,
    }

    public class CCAction : ICopyable
    {
        protected int m_nTag;
        protected CCNode m_pOriginalTarget;
        protected CCNode m_pTarget;

        public CCAction()
        {
            m_nTag = (int) ActionTag.kCCActionTagInvalid;
        }

        public CCAction(CCAction action)
        {
            m_nTag = action.m_nTag;
        }

        public CCNode Target
        {
            get { return m_pTarget; }
            set { m_pTarget = value; }
        }

        public CCNode OriginalTarget
        {
            get { return m_pOriginalTarget; }
            /*
            set
            {
                m_pOriginalTarget = value;
            }
            */
        }

        public int Tag
        {
            get { return m_nTag; }
            set { m_nTag = value; }
        }

		public virtual CCAction Copy()
		{
			return (CCAction)Copy(null);
		}

        public virtual object Copy(ICopyable zone)
        {

            if (zone != null)
            {
                ((CCAction) zone).m_nTag = m_nTag;
                return zone;
            }
            else
            {
                return new CCAction(this);
            }
        }

        public virtual bool IsDone
        {
            get { return true; }
        }

        public virtual void StartWithTarget(CCNode target)
        {
            m_pOriginalTarget = m_pTarget = target;
        }

        public virtual void Stop()
        {
            m_pTarget = null;
        }

        public virtual void Step(float dt)
        {
            CCLog.Log("[Action step]. override me");
        }

        public virtual void Update(float time)
        {
            CCLog.Log("[Action update]. override me");
        }

    }
}