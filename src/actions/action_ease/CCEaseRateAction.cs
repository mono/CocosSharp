namespace CocosSharp
{
	public class CCEaseRateAction : CCActionEase
	{
		public float Rate { get; private set; }


		#region Constructors

        public CCEaseRateAction (CCFiniteTimeAction pAction, float fRate) : base (pAction)
		{
			Rate = fRate;
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseRateActionState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
            return new CCEaseRateAction ((CCFiniteTimeAction)InnerAction.Reverse (), 1 / Rate);
		}
	}


	#region Action state

	internal class CCEaseRateActionState : CCActionEaseState
	{
		protected float Rate { get; private set; }

		public CCEaseRateActionState (CCEaseRateAction action, CCNode target) : base (action, target)
		{
			Rate = action.Rate;
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.ExponentialOut (time));
		}
	}

	#endregion Action state
}