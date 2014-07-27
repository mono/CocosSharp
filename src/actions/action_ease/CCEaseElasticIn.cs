using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
	public class CCEaseElasticIn : CCEaseElastic
	{
		#region Constructors

		public CCEaseElasticIn (CCActionInterval pAction) : this (pAction, 0.3f)
		{
		}

		public CCEaseElasticIn (CCActionInterval pAction, float fPeriod) : base (pAction, fPeriod)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseElasticInState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCEaseElasticOut ((CCActionInterval)InnerAction.Reverse (), Period);
		}
	}


	#region Action state

	internal class CCEaseElasticInState : CCEaseElasticState
	{
		public CCEaseElasticInState (CCEaseElasticIn action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.ElasticIn (time, Period));
		}
	}

	#endregion Action state
}