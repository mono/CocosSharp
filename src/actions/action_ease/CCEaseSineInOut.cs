using System;

namespace CocosSharp
{
	public class CCEaseSineInOut : CCActionEase
	{
		#region Constructors

        public CCEaseSineInOut (CCFiniteTimeAction pAction) : base (pAction)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseSineInOutState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
            return new CCEaseSineInOut ((CCFiniteTimeAction)InnerAction.Reverse ());
		}
	}


	#region Action state

	internal class CCEaseSineInOutState : CCActionEaseState
	{
		public CCEaseSineInOutState (CCEaseSineInOut action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.SineInOut (time));
		}
	}

	#endregion Action state
}