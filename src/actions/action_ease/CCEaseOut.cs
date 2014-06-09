using System;

namespace CocosSharp
{
	public class CCEaseOut : CCEaseRateAction
	{
		#region Constructors

		public CCEaseOut (CCActionInterval pAction, float fRate) : base (pAction, fRate)
		{
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCEaseOutState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCEaseOut ((CCActionInterval)InnerAction.Reverse (), 1 / Rate);
		}
	}


	#region Action state

	public class CCEaseOutState : CCEaseRateActionState
	{
		public CCEaseOutState (CCEaseOut action, CCNode target) : base (action, target)
		{
		}

		protected internal override void Update (float time)
		{
			InnerActionState.Update ((float)(Math.Pow (time, 1 / Rate)));      
		}
	}

	#endregion Action state
}