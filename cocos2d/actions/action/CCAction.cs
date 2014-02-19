namespace CocosSharp
{
    public enum CCActionTag
    {
        //! Default tag
        Invalid = -1,
    }

    public class CCAction
    {
		public int Tag { get; set; }
		public CCNode OriginalTarget { get; private set; }
		public CCNode Target { get; protected set; }

        #region Constructors

        public CCAction()
        {
            Tag = (int) CCActionTag.Invalid;
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