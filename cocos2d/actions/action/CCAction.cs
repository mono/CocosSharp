namespace CocosSharp
{
    public enum CCActionTag
    {
        //! Default tag
        Invalid = -1,
    }

    public class CCAction
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

        #region Constructors

        public CCAction()
        {
            m_nTag = (int) CCActionTag.Invalid;
        }


        #endregion Constructor

		protected internal virtual CCActionState StartAction (CCNode target)
		{
			return new CCActionState (this, target);

		}

    }

	public class CCActionState {

		public CCNode Target { get; protected set; }
		public CCNode OriginalTarget { get; protected set; }
		public CCAction Action { get; protected set;}

		public CCActionState (CCAction action, CCNode target)
		{
			this.Action = action;
			this.Target = target;
			this.OriginalTarget = target;
		}

		public virtual bool IsDone
		{
			get { return true; }
		}

		public virtual void Stop()
		{
			Target = null;
		}

		public virtual void Step(float dt)
		{
#if DEBUG
			CCLog.Log("[Action State step]. override me");
#endif
		}

		public virtual void Update(float time)
		{
#if DEBUG
			CCLog.Log("[Action State update]. override me");
#endif
		}
	}
}