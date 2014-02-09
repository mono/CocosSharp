namespace CocosSharp
{
    public class CCMoveTo : CCMoveBy
    {
        #region Constructors

        public CCMoveTo(float duration, CCPoint position): base(duration, position)
        {
			m_endPosition = position;
        }

        #endregion Constructors

		public CCPoint PositionEnd
		{
			get { return m_endPosition; }
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCMoveToState (this, target);

		}
    }

	public class CCMoveToState : CCMoveByState
	{

		public CCMoveToState (CCMoveTo action, CCNode target)
			: base(action, target)
		{ 
			m_startPosition = target.Position;
			m_positionDelta = action.PositionEnd - target.Position;
		}

		public override void Update(float time)
		{
			if (Target != null)
			{
				CCPoint currentPos = Target.Position;
				CCPoint diff = currentPos - m_previousPosition;
				//m_startPosition = m_startPosition + diff;
				CCPoint newPos = m_startPosition + m_positionDelta * time;
				Target.Position = newPos;
				m_previousPosition = newPos;
			}
		}
	}

}