namespace CocosSharp
{
    public class CCMoveBy : CCActionInterval
    {
        protected CCPoint m_positionDelta;
        protected CCPoint m_endPosition;
        protected CCPoint m_startPosition;
        protected CCPoint m_previousPosition;


        #region Constructors

        public CCMoveBy(float duration, CCPoint position) : base(duration)
        {
			m_positionDelta = position;
        }

        #endregion Constructors

		public CCPoint PositionDelta
		{
			get { return m_positionDelta; }
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCMoveByState (this, target);

		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCMoveBy(m_fDuration, new CCPoint(-m_positionDelta.X, -m_positionDelta.Y));
        }

		// Take me out later - See comments in CCAction
		public override bool HasState 
		{ 
			get { return true; }
		}

    }

	public class CCMoveByState : CCActionIntervalState
	{
		protected CCPoint m_positionDelta;
		protected CCPoint m_endPosition;
		protected CCPoint m_startPosition;
		protected CCPoint m_previousPosition;

		public CCMoveByState (CCMoveBy action, CCNode target)
			: base(action, target)
		{ 
			m_positionDelta = action.PositionDelta;
			m_previousPosition = m_startPosition = target.Position;

		}

		public override void Update(float time)
		{
			if (Target != null)
			{
				CCPoint currentPos = Target.Position;
				CCPoint diff = currentPos - m_previousPosition;
				m_startPosition = m_startPosition + diff;
				CCPoint newPos = m_startPosition + m_positionDelta * time;
				Target.Position = newPos;
				m_previousPosition = newPos;
			}
		}
	}

}