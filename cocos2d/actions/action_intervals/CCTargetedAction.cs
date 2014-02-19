namespace CocosSharp
{
	public class CCTargetedAction : CCActionInterval
	{
		public CCFiniteTimeAction TargetedAction { get; private set; }
		public CCNode ForcedTarget { get; private set; }

		#region Constructors

		public CCTargetedAction(CCNode target, CCFiniteTimeAction pAction) : base(pAction.Duration)
		{
			ForcedTarget = target;
			TargetedAction = pAction;
		}

		#endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCTargetedActionState (this, target);

		}

		public override CCFiniteTimeAction Reverse()
		{
			return new CCTargetedAction(ForcedTarget, TargetedAction.Reverse());
		}
	}

	public class CCTargetedActionState : CCActionIntervalState
	{
		protected CCFiniteTimeAction TargetedAction { get; set; }
		protected CCFiniteTimeActionState ActionState { get; set; }
		protected CCNode ForcedTarget { get; set; }

		public CCTargetedActionState (CCTargetedAction action, CCNode target)
			: base(action, target)
		{	

			ForcedTarget = action.ForcedTarget;
			TargetedAction = action.TargetedAction;

			if (!TargetedAction.HasState)
				TargetedAction.StartWithTarget(ForcedTarget);
			else
				ActionState = (CCFiniteTimeActionState)TargetedAction.StartAction(ForcedTarget);

		}

		public override void Stop()
		{

			if (!TargetedAction.HasState)
				TargetedAction.Stop();
			else
				ActionState.Stop();
		}

		public override void Update(float time)
		{
			if (!TargetedAction.HasState)
				TargetedAction.Update(time);
			else
				ActionState.Update(time);
		}


	}

}