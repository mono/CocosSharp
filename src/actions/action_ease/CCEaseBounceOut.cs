namespace CocosSharp
{
	public class CCEaseBounceOut : CCActionEase
	{
		#region Constructors

        public CCEaseBounceOut (CCFiniteTimeAction pAction) : base (pAction)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseBounceOutState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
            return new CCEaseBounceIn ((CCFiniteTimeAction)InnerAction.Reverse ());
		}
	}


	#region Action state

	internal class CCEaseBounceOutState : CCActionEaseState
	{
		public CCEaseBounceOutState (CCEaseBounceOut action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.BounceOut (time));
		}
	}

	#endregion Action state
}