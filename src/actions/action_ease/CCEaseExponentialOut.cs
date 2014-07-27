using System;

namespace CocosSharp
{
	public class CCEaseExponentialOut : CCActionEase
	{
		#region Constructors

        public CCEaseExponentialOut (CCFiniteTimeAction pAction) : base (pAction)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseExponentialOutState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
            return new CCEaseExponentialIn ((CCFiniteTimeAction)InnerAction.Reverse ());
		}
	}


	#region Action state

	internal class CCEaseExponentialOutState : CCActionEaseState
	{
		public CCEaseExponentialOutState (CCEaseExponentialOut action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.ExponentialOut (time));
		}
	}

	#endregion Action state
}