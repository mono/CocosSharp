namespace CocosSharp
{
    public class CCRotateBy : CCActionInterval
    {
        protected float m_fAngleX;
        protected float m_fAngleY;

        #region Constructors

        public CCRotateBy(float duration, float fDeltaAngleX, float fDeltaAngleY) : base(duration)
        {
			m_fAngleX = fDeltaAngleX;
			m_fAngleY = fDeltaAngleY;
        }

        public CCRotateBy(float duration, float fDeltaAngle) : this(duration, fDeltaAngle, fDeltaAngle)
        {
        }

        #endregion Constructors

		public float AngleX
		{
			get { return m_fAngleX; }
		}

		public float AngleY
		{
			get { return m_fAngleY; }
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCRotateByState (this, target);

		}

		public override bool HasState {
			get {
				return true;
			}
		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCRotateBy(m_fDuration, -m_fAngleX, -m_fAngleY);
        }
    }

	public class CCRotateByState : CCActionIntervalState
	{

		protected float m_fAngleX;
		protected float m_fAngleY;
		protected float m_fStartAngleX;
		protected float m_fStartAngleY;

		public CCRotateByState (CCRotateBy action, CCNode target)
			: base(action, target)
		{ 
			m_fAngleX = action.AngleX;
			m_fAngleY = action.AngleY;
			m_fStartAngleX = target.RotationX;
			m_fStartAngleY = target.RotationY;

		}

		public override void Update(float time)
		{
			// XXX: shall I add % 360
			if (Target != null)
			{
				Target.RotationX = m_fStartAngleX + m_fAngleX * time;
				Target.RotationY = m_fStartAngleY + m_fAngleY * time;
			}
		}

	}

}