using System.Diagnostics;

namespace CocosSharp
{
	public class CCSpeed : CCAction
	{
		public float Speed { get; private set; }

		protected internal CCActionInterval InnerAction { get; private set; }


		#region Constructors

		public CCSpeed (CCActionInterval action, float fRate)
		{
			InnerAction = action;
			Speed = fRate;
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCSpeedState (this, target);
		}

		public virtual CCActionInterval Reverse ()
		{
			return (CCActionInterval)(CCAction)new CCSpeed ((CCActionInterval)InnerAction.Reverse (), Speed);
		}
	}


	#region Action state

	public class CCSpeedState : CCActionState
	{
		public float Speed { get; private set; }

		protected CCActionIntervalState InnerActionState { get; private set; }

		protected internal override bool IsDone {
			get { return InnerActionState.IsDone; }
		}

		public CCSpeedState (CCSpeed action, CCNode target) : base (action, target)
		{
			InnerActionState = (CCActionIntervalState)action.InnerAction.StartAction (target);
			Speed = action.Speed;
		}

		protected internal override void Stop ()
		{
			InnerActionState.Stop ();
			base.Stop ();
		}

		public override void Step (float dt)
		{
			InnerActionState.Step (dt * Speed);
		}
	}

	#endregion Action state
}