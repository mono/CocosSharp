namespace CocosSharp
{
    public class CCSkewTo : CCActionInterval
    {
        protected float m_fEndSkewX;
        protected float m_fEndSkewY;
		protected float m_fSkewX;
		protected float m_fSkewY;

        #region Constructors

        public CCSkewTo(float t, float sx, float sy) : base(t)
        {
			m_fEndSkewX = sx;
			m_fEndSkewY = sy;
        }

		public CCSkewTo(float t, float skewXY) : this(t, skewXY, skewXY)
		{ }

        #endregion Constructors
		public float SkewToX
		{
			get { return m_fEndSkewX; }
		}

		public float SkewToY
		{
			get { return m_fEndSkewY; }
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCSkewToState (this, target);

		}

    }

	public class CCSkewToState : CCActionIntervalState
	{

		protected float m_fDeltaX;
		protected float m_fDeltaY;
		protected float m_fEndSkewX;
		protected float m_fEndSkewY;
		protected float m_fSkewX;
		protected float m_fSkewY;
		protected float m_fStartSkewX;
		protected float m_fStartSkewY;

		public CCSkewToState (CCSkewTo action, CCNode target)
			: base(action, target)
		{ 
			m_fEndSkewX = action.SkewToX;
			m_fEndSkewY = action.SkewToY;

			m_fStartSkewX = target.SkewX;

			if (m_fStartSkewX > 0)
			{
				m_fStartSkewX = m_fStartSkewX % 180f;
			}
			else
			{
				m_fStartSkewX = m_fStartSkewX % -180f;
			}

			m_fDeltaX = m_fEndSkewX - m_fStartSkewX;

			if (m_fDeltaX > 180)
			{
				m_fDeltaX -= 360;
			}
			if (m_fDeltaX < -180)
			{
				m_fDeltaX += 360;
			}

			m_fStartSkewY = target.SkewY;

			if (m_fStartSkewY > 0)
			{
				m_fStartSkewY = m_fStartSkewY % 360f;
			}
			else
			{
				m_fStartSkewY = m_fStartSkewY % -360f;
			}

			m_fDeltaY = m_fEndSkewY - m_fStartSkewY;

			if (m_fDeltaY > 180)
			{
				m_fDeltaY -= 360;
			}
			if (m_fDeltaY < -180)
			{
				m_fDeltaY += 360;
			}
		}

		public override void Update(float time)
		{
			Target.SkewX = m_fStartSkewX + m_fDeltaX * time;
			Target.SkewY = m_fStartSkewY + m_fDeltaY * time;
		}

	}

}