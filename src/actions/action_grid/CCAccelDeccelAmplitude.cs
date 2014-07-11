using System;

namespace CocosSharp
{
	public class CCAccelDeccelAmplitude : CCAccelAmplitude
	{
		#region Constructors

		public CCAccelDeccelAmplitude (CCAmplitudeAction pAction, float duration, float accDeccRate = 1.0f)
			: base (pAction, duration, accDeccRate)
		{
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCAccelDeccelAmplitudeState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCAccelDeccelAmplitude ((CCAmplitudeAction)OtherAction.Reverse (), Duration, Rate);
		}
	}


	#region Action state

	public class CCAccelDeccelAmplitudeState : CCAccelAmplitudeState
	{
		public CCAccelDeccelAmplitudeState (CCAccelDeccelAmplitude action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			float f = time * 2;

			if (f > 1)
			{
				f -= 1;
				f = 1 - f;
			}

			OtherActionState.AmplitudeRate = (float)Math.Pow (f, Rate);
		}
	}

	#endregion Action state
}