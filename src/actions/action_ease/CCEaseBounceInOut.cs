namespace CocosSharp
{
	public class CCEaseBounceInOut : CCActionEase
	{
		#region Constructors

        public CCEaseBounceInOut (CCFiniteTimeAction pAction) : base (pAction)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseBounceInOutState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
            return new CCEaseBounceInOut ((CCFiniteTimeAction)InnerAction.Reverse ());
		}
	}


	#region Action state

	internal class CCEaseBounceInOutState : CCActionEaseState
	{
		public CCEaseBounceInOutState (CCEaseBounceInOut action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.BounceInOut (time));
		}
	}

	#endregion Action state
}