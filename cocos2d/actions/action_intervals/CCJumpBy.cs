namespace CocosSharp
{
    public class CCJumpBy : CCActionInterval
    {
        protected CCPoint m_delta;
        protected float m_height;
        protected uint m_nJumps;

        #region Constructors

        public CCJumpBy(float duration, CCPoint position, float height, uint jumps) : base(duration)
        {
            m_delta = position;
            m_height = height;
            m_nJumps = jumps;
        }

        #endregion Constructors

		public CCPoint Position
		{
			get { return m_delta; }
		}

		public float Height
		{
			get { return m_height; }
		}
		public uint Jumps
		{
			get { return m_nJumps; }
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCJumpByState (this, target);

		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCJumpBy(m_fDuration, new CCPoint(-m_delta.X, -m_delta.Y), m_height, m_nJumps);
        }
    }

	public class CCJumpByState : CCActionIntervalState
	{

		protected CCPoint m_delta;
		protected float m_height;
		protected uint m_nJumps;
		protected CCPoint m_startPosition;
		protected CCPoint m_previousPos;

		public CCJumpByState (CCJumpBy action, CCNode target)
			: base(action, target)
		{ 
			m_delta = action.Position;
			m_height = action.Height;
			m_nJumps = action.Jumps;
			m_previousPos = m_startPosition = target.Position;
		}

		public override void Update(float time)
		{
			if (Target != null)
			{
				// Is % equal to fmodf()???
				float frac = (time * m_nJumps) % 1f;
				float y = m_height * 4f * frac * (1f - frac);
				y += m_delta.Y * time;
				float x = m_delta.X * time;

				CCPoint currentPos = Target.Position;

				CCPoint diff = currentPos - m_previousPos;
				m_startPosition = diff + m_startPosition;

				CCPoint newPos = m_startPosition + new CCPoint(x,y);
				Target.Position = newPos;

				m_previousPos = newPos;
			}
		}
	}

}