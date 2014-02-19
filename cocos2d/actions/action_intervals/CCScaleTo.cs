namespace CocosSharp
{
    public class CCScaleTo : CCActionInterval
    {
        protected float m_fDeltaX;
        protected float m_fDeltaY;
        protected float m_fEndScaleX;
        protected float m_fEndScaleY;
        protected float m_fScaleX;
        protected float m_fScaleY;
        protected float m_fStartScaleX;
        protected float m_fStartScaleY;


        #region Constructors

        public CCScaleTo(float duration, float s) : base(duration)
        {
            m_fEndScaleX = s;
            m_fEndScaleY = s;
        }

        public CCScaleTo(float duration, float sx, float sy) : base(duration)
        {
            m_fEndScaleX = sx;
            m_fEndScaleY = sy;
        }

        #endregion Constructors

		public float ScaleX
		{
			get { return m_fEndScaleX; }
		}
		public float ScaleY
		{
			get { return m_fEndScaleY; }
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCScaleToState (this, target);

		}

    }

	public class CCScaleToState : CCActionIntervalState
	{
		protected float m_fDeltaX;
		protected float m_fDeltaY;
		protected float m_fEndScaleX;
		protected float m_fEndScaleY;
		protected float m_fScaleX;
		protected float m_fScaleY;
		protected float m_fStartScaleX;
		protected float m_fStartScaleY;

		public CCScaleToState (CCScaleTo action, CCNode target)
			: base(action, target)
		{ 
			m_fStartScaleX = target.ScaleX;
			m_fStartScaleY = target.ScaleY;
			m_fEndScaleX = action.ScaleX;
			m_fEndScaleY = action.ScaleY;
			m_fDeltaX = m_fEndScaleX - m_fStartScaleX;
			m_fDeltaY = m_fEndScaleY - m_fStartScaleY;
		}

		public override void Update(float time)
		{
			if (Target != null)
			{
				Target.ScaleX = m_fStartScaleX + m_fDeltaX * time;
				Target.ScaleY = m_fStartScaleY + m_fDeltaY * time;
			}
		}
	}
}