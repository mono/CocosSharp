namespace CocosSharp
{
	public class CCTargetedAction : CCActionInterval
	{
		public CCFiniteTimeAction TargetedAction { get; private set; }

		public CCNode ForcedTarget { get; private set; }

		#region Constructors

		public CCTargetedAction (CCNode target, CCFiniteTimeAction pAction) : base (pAction.Duration)
		{
			ForcedTarget = target;
			TargetedAction = pAction;
		}

		#endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCTargetedActionState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCTargetedAction (ForcedTarget, TargetedAction.Reverse ());
		}
	}

	public class CCTargetedActionState : CCActionIntervalState
	{
		protected CCFiniteTimeAction TargetedAction { get; set; }

		protected CCFiniteTimeActionState ActionState { get; set; }

		protected CCNode ForcedTarget { get; set; }

		public CCTargetedActionState (CCTargetedAction action, CCNode target)
			: base (action, target)
		{	
			ForcedTarget = action.ForcedTarget;
			TargetedAction = action.TargetedAction;

			ActionState = (CCFiniteTimeActionState)TargetedAction.StartAction (ForcedTarget);
		}

		protected internal override void Stop ()
		{
			ActionState.Stop ();
		}

		protected internal override void Update (float time)
		{
			ActionState.Update (time);
		}


	}

}