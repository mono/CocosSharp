using System;
using System.Collections.Generic;

namespace CocosSharp
{
	public class CCRemoveSelf : CCActionInstant
	{
		public bool IsNeedCleanUp { get; private set; }

		#region Constructors

		public CCRemoveSelf ()
			: this (true)
		{
		}

		public CCRemoveSelf (bool isNeedCleanUp)
		{
			IsNeedCleanUp = isNeedCleanUp;
		}

		#endregion Constructors

		/// <summary>
		/// Start the hide operation on the given target.
		/// </summary>
		/// <param name="target"></param>
		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCRemoveSelfState (this, target);

		}


		public override CCFiniteTimeAction Reverse ()
		{
			return new CCRemoveSelf (IsNeedCleanUp);
		}
	}

	public class CCRemoveSelfState : CCActionInstantState
	{
		protected bool IsNeedCleanUp { get; set; }

		public CCRemoveSelfState (CCRemoveSelf action, CCNode target)
			: base (action, target)
		{	
			IsNeedCleanUp = action.IsNeedCleanUp;
		}

		public override void Update (float time)
		{
			Target.RemoveFromParent (IsNeedCleanUp);
		}

	}

}