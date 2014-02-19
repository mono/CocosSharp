using System.Diagnostics;

namespace CocosSharp
{
    public class CCRepeatForever : CCActionInterval
    {
		public CCActionInterval InnerAction { get; private set; }


        #region Constructors

		public CCRepeatForever (params CCFiniteTimeAction[] actions)
		{
			Debug.Assert(actions != null);
			InnerAction = new CCSequence (actions);

		}

        public CCRepeatForever(CCActionInterval action)
        {
			Debug.Assert(action != null);
			InnerAction = action;
        }

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCRepeatForeverState (this, target);

		}

		public override CCFiniteTimeAction Reverse()
        {
            return new CCRepeatForever(InnerAction.Reverse() as CCActionInterval);
        }
    }

	public class CCRepeatForeverState : CCActionIntervalState
	{

		private CCActionInterval InnerAction { get; set; }
		private CCActionIntervalState InnerActionState { get; set; }

		public CCRepeatForeverState (CCRepeatForever action, CCNode target)
			: base(action, target)
		{ 
			InnerAction = action.InnerAction;
			if (InnerAction.HasState)
				InnerActionState = (CCActionIntervalState)InnerAction.StartAction (target);
			else
				InnerAction.StartWithTarget (target);
		}

		public override void Step(float dt)
		{
			if (!InnerAction.HasState) {
				InnerAction.Step (dt);

				if (InnerAction.IsDone) {
					float diff = InnerAction.Elapsed - InnerAction.Duration;
					InnerAction.StartWithTarget (Target);
					InnerAction.Step (0f);
					InnerAction.Step (diff);
				}
			} else {

				InnerActionState.Step (dt);

				if (InnerActionState.IsDone) {
					float diff = InnerActionState.Elapsed - InnerActionState.Duration;
					InnerActionState = (CCActionIntervalState)InnerAction.StartAction(Target);
					InnerActionState.Step (0f);
					InnerActionState.Step (diff);
				}
			}
		}

		public override bool IsDone
		{
			get { return false; }
		}

	}
}