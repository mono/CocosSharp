namespace CocosSharp
{
	public class CCActionEase : CCActionInterval
	{
		protected internal CCActionInterval InnerAction { get; private set; }

		#region Constructors

		// This can be taken out once all the classes that extend it have had their constructors created.
		protected CCActionEase ()
		{
		}

		public CCActionEase (CCActionInterval pAction) : base (pAction.Duration)
		{
			InnerAction = pAction;
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCActionEaseState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCActionEase ((CCActionInterval)InnerAction.Reverse ());
		}
	}


	#region Action state

	public class CCActionEaseState : CCActionIntervalState
	{
		protected CCActionIntervalState InnerActionState { get; private set; }

		public CCActionEaseState (CCActionEase action, CCNode target) : base (action, target)
		{
			InnerActionState = (CCActionIntervalState)action.InnerAction.StartAction (target);
		}

		protected internal override void Stop ()
		{
			InnerActionState.Stop ();
			base.Stop ();
		}

		protected internal override void Update (float time)
		{
			InnerActionState.Update (time);
		}
	}

	#endregion Action state
}