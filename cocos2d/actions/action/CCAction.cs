
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
			return (CCAction)CopyWithZone(null);
		}

        public virtual object CopyWithZone(CCZone zone)
        {
            CCZone tmpZone = zone;
            CCAction ret;
            if (tmpZone != null && tmpZone.m_pCopyObject != null)
            {
                ret = (CCAction) tmpZone.m_pCopyObject;
            }
            else
            {
                ret = new CCAction();
            }

            ret.m_nTag = m_nTag;
            return ret;
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

        public static CCAction Create()
        {
            return new CCAction();
        }
    }
}