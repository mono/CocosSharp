namespace CocosSharp
{
	public class CCProgressFromTo : CCActionInterval
	{
		public float PercentFrom { get; protected set; }

		public float PercentTo { get; protected set; }

		#region Constructors

		/// <summary>
		/// Creates and initializes the action with a duration, a "from" percentage and a "to" percentage
		/// </summary>
		public CCProgressFromTo (float duration, float fromPercentage, float toPercentage) : base (duration)
		{
			PercentTo = toPercentage;
			PercentFrom = fromPercentage;
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCProgressFromToState (this, target);
		}
	}

	internal class CCProgressFromToState : CCActionIntervalState
	{
		protected float PercentFrom { get; set; }

		protected float PercentTo { get; set; }

		public CCProgressFromToState (CCProgressFromTo action, CCNode target)
			: base (action, target)
		{ 
			PercentTo = action.PercentTo;
			PercentFrom = action.PercentFrom;

			((CCProgressTimer)(Target)).Percentage = PercentFrom;
		}

		public override void Update (float time)
		{
			((CCProgressTimer)(Target)).Percentage = PercentFrom + (PercentTo - PercentFrom) * time;
		}

	}

}