namespace CocosSharp
{
	public class CCActionInstant : CCFiniteTimeAction
	{

		#region Constructors

		protected CCActionInstant ()
		{
		}

		protected CCActionInstant (float d)
			: base (d)
		{
		}

		#endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCActionInstantState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCActionInstant (Duration);
		}
	}

	public class CCActionInstantState : CCFiniteTimeActionState
	{

		public CCActionInstantState (CCActionInstant action, CCNode target)
			: base (action, target)
		{
		}

		public override bool IsDone {
			get { return true; }
		}

		public override void Step (float dt)
		{
			Update (1);
		}

		public override void Update (float time)
		{
			// ignore
		}
	}

}