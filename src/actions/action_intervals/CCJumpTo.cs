namespace CocosSharp
{
	public class CCJumpTo : CCJumpBy
	{
		#region Constructors

		public CCJumpTo (float duration, CCPoint position, float height, uint jumps) : base (duration, position, height, jumps)
		{
		}

		#endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCJumpToState (this, target);

		}

	}

	public class CCJumpToState : CCJumpByState
	{

		public CCJumpToState (CCJumpBy action, CCNode target)
			: base (action, target)
		{ 
			Delta = new CCPoint (Delta.X - StartPosition.X, Delta.Y - StartPosition.Y);
		}
	}

}