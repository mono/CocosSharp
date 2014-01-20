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

        public virtual bool IsDone
        {
            get { return true; }
        }


        #region Constructors

        public CCAction()
        {
            m_nTag = (int) CCActionTag.Invalid;
        }

        protected CCAction(CCAction action)
        {
            m_nTag = action.m_nTag;
        }

        #endregion Constructor


        public virtual CCAction Copy()
        {
            return (CCAction) Copy(null);
        }

        public virtual object Copy(ICCCopyable zone)
        {
            return new CCAction(this);
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