namespace CocosSharp
{
    public enum CCActionTag
    {
        //! Default tag
        Invalid = -1,
    }

    public class CCAction : ICCCopyable
    {
        protected int m_nTag;
        protected CCNode m_pOriginalTarget;
        protected CCNode m_pTarget;

        public CCAction()
        {
            m_nTag = (int) CCActionTag.Invalid;
        }

        protected CCAction(CCAction action)
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
        }

        public int Tag
        {
            get { return m_nTag; }
            set { m_nTag = value; }
        }

        public virtual CCAction Copy()
        {
            return (CCAction) Copy(null);
        }

        public virtual object Copy(ICCCopyable zone)
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

        protected internal virtual void StartWithTarget(CCNode target)
        {
            m_pOriginalTarget = m_pTarget = target;
        }

        public virtual void Stop()
        {
            m_pTarget = null;
        }

        public virtual void Step(float dt)
        {
#if DEBUG
            CCLog.Log("[Action step]. override me");
#endif
        }

        public virtual void Update(float time)
        {
#if DEBUG
            CCLog.Log("[Action update]. override me");
#endif
        }
    }
}