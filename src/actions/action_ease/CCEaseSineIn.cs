using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
	public class CCEaseSineIn : CCActionEase
	{
		#region Constructors

        public CCEaseSineIn (CCFiniteTimeAction pAction) : base (pAction)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCEaseSineInState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
            return new CCEaseSineOut ((CCFiniteTimeAction)InnerAction.Reverse ());
		}
	}


	#region Action state

	internal class CCEaseSineInState : CCActionEaseState
	{
		public CCEaseSineInState (CCEaseSineIn action, CCNode target) : base (action, target)
		{
		}

		public override void Update (float time)
		{
			InnerActionState.Update (CCEaseMath.SineIn (time));
		}
	}

	#endregion Action state
}