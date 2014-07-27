namespace CocosSharp
{
	public class CCEaseBackIn : CCActionEase
	{
		#region Constructors

		public CCEaseBackIn (CCActionInterval pAction) : base (pAction)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseBackInState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCEaseBackOut ((CCActionInterval)InnerAction.Reverse ());
		}
	}


	#region Action state

	internal class CCEaseBackInState : CCActionEaseState
	{
		public CCEaseBackInState (CCEaseBackIn action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.BackIn (time));
		}
	}

	#endregion Action state
}