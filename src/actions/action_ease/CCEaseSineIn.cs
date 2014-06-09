using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
	public class CCEaseSineIn : CCActionEase
	{
		#region Constructors

		public CCEaseSineIn (CCActionInterval pAction) : base (pAction)
		{
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCEaseSineInState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCEaseSineOut ((CCActionInterval)InnerAction.Reverse ());
		}
	}


	#region Action state

	public class CCEaseSineInState : CCActionEaseState
	{
		public CCEaseSineInState (CCEaseSineIn action, CCNode target) : base (action, target)
		{
		}

		protected internal override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.SineIn (time));
		}
	}

	#endregion Action state
}