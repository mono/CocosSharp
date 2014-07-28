using System;

namespace CocosSharp
{
	public class CCEaseSineOut : CCActionEase
	{
		#region Constructors

        public CCEaseSineOut (CCFiniteTimeAction pAction) : base (pAction)
		{
		}

		#endregion Constructors


		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseSineOutState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
            return new CCEaseSineIn ((CCFiniteTimeAction)InnerAction.Reverse ());
		}
	}


	#region Action state

	internal class CCEaseSineOutState : CCActionEaseState
	{
		public CCEaseSineOutState (CCEaseSineOut action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.SineOut (time));
		}
	}

	#endregion Action state
}