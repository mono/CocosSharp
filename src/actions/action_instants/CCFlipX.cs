namespace CocosSharp
{
	public class CCFlipX : CCActionInstant
	{
		public bool FlipX { get; private set; }


		#region Constructors

		public CCFlipX (bool x)
		{
			FlipX = x;
		}

		#endregion Constructors

		/// <summary>
		/// Start the flip operation on the given target which must be a CCSprite.
		/// </summary>
		/// <param name="target"></param>
		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCFlipXState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCFlipX (!FlipX);
		}

	}

	public class CCFlipXState : CCActionInstantState
	{

		public CCFlipXState (CCFlipX action, CCNode target)
			: base (action, target)
		{	

			if (!(target is CCSprite))
			{
				throw (new System.NotSupportedException ("FlipX and FlipY actions only work on CCSprite instances."));
			}
			((CCSprite)(target)).FlipX = action.FlipX;		
		}

	}

}