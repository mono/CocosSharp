namespace CocosSharp
{
	public class CCEaseElastic : CCActionEase
	{
		public float Period { get; private set; }


		#region Constructors

        public CCEaseElastic (CCFiniteTimeAction pAction, float fPeriod) : base (pAction)
		{
			Period = fPeriod;
		}

        public CCEaseElastic (CCFiniteTimeAction pAction) : this (pAction, 0.3f)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseElasticState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			//assert(0);
			return null;
		}
	}


	#region Action state

	internal class CCEaseElasticState : CCActionEaseState
	{
		protected float Period { get; private set; }

		public CCEaseElasticState (CCEaseElastic action, CCNode target) : base (action, target)
		{
			Period = action.Period;
		}
	}

	#endregion Action state
}