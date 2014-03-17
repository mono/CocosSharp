namespace CocosSharp
{
    public class CCMoveTo : CCMoveBy
    {
        #region Constructors

        public CCMoveTo(float duration, CCPoint position): base(duration, position)
        {
			EndPosition = position;
        }

        #endregion Constructors

		public CCPoint PositionEnd
		{
			get { return EndPosition; }
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
			StartPosition = target.Position;
			PositionDelta = action.PositionEnd - target.Position;
		}

		public override void Update(float time)
		{
			if (Target != null)
			{
				CCPoint currentPos = Target.Position;
				CCPoint diff = currentPos - PreviousPosition;
				//m_startPosition = m_startPosition + diff;
				CCPoint newPos = StartPosition + PositionDelta * time;
				Target.Position = newPos;
				PreviousPosition = newPos;
			}
		}
	}

}