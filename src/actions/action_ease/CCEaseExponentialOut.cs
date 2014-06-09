using System;

namespace CocosSharp
{
	public class CCEaseExponentialOut : CCActionEase
	{
		#region Constructors

		public CCEaseExponentialOut (CCActionInterval pAction) : base (pAction)
		{
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCEaseExponentialOutState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCEaseExponentialIn ((CCActionInterval)InnerAction.Reverse ());
		}
	}


	#region Action state

	public class CCEaseExponentialOutState : CCActionEaseState
	{
		public CCEaseExponentialOutState (CCEaseExponentialOut action, CCNode target) : base (action, target)
		{
		}

		protected internal override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.ExponentialOut (time));
		}
	}

	#endregion Action state
}