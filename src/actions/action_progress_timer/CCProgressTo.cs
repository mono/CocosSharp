namespace CocosSharp
{
	public class CCProgressTo : CCProgressFromTo
	{

		#region Constructors

		/// <summary>
		/// Creates and initializes with a duration and a percent
		/// </summary>
		public CCProgressTo (float duration, float percentTo) : base (duration, 0.0f, percentTo)
		{
		}

		#endregion Constructors


		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCProgressToState (this, target);
		}

	}

	internal class CCProgressToState : CCProgressFromToState
	{

		public CCProgressToState (CCProgressTo action, CCNode target)
			: base (action, target)
		{ 
			PercentTo = action.PercentTo;
			PercentFrom = ((CCProgressTimer)(target)).Percentage;
			// XXX: Is this correct ?
			// Adding it to support CCRepeat
			if (PercentFrom == 100)
			{
				PercentFrom = 0;
			}
		}

	}

}