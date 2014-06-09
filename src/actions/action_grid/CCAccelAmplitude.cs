using System;

namespace CocosSharp
{
	public class CCAccelAmplitude : CCActionInterval
	{
		public float Rate { get; private set; }

		protected internal CCAmplitudeAction OtherAction { get; private set; }


		#region Constructors

		public CCAccelAmplitude (CCAmplitudeAction pAction, float duration, float accelRate = 1.0f) : base (duration)
		{
			Rate = accelRate;
			OtherAction = pAction;
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCAccelAmplitudeState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCAccelAmplitude ((CCAmplitudeAction)OtherAction.Reverse (), Duration, Rate);
		}

	}


	#region Action state

	public class CCAccelAmplitudeState : CCActionIntervalState
	{
		public float Rate { get; set; }

		protected CCAmplitudeActionState OtherActionState { get; private set; }


		public CCAccelAmplitudeState (CCAccelAmplitude action, CCNode target) : base (action, target)
		{
			Rate = action.Rate;
			OtherActionState = (CCAmplitudeActionState)action.OtherAction.StartAction (target);
		}

		protected internal override void Update (float time)
		{
			OtherActionState.AmplitudeRate = (float)Math.Pow (time, Rate);
			OtherActionState.Update (time);
		}
	}

	#endregion Action state
}