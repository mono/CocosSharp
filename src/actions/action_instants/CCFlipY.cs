namespace CocosSharp
{
	public class CCFlipY : CCActionInstant
	{
		public bool FlipY { get; private set; }


		#region Constructors

		public CCFlipY (bool y)
		{
			FlipY = y;
		}

		#endregion Constructors


		/// <summary>
		/// Start the flip operation on the given target which must be a CCSprite.
		/// </summary>
		/// <param name="target"></param>
		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCFlipYState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCFlipY (!FlipY);
		}

	}

	internal class CCFlipYState : CCActionInstantState
	{

		public CCFlipYState (CCFlipY action, CCNode target)
			: base (action, target)
		{	

			if (!(target is CCSprite))
			{
				throw (new System.NotSupportedException ("FlipX and FlipY actions only work on CCSprite instances."));
			}
			((CCSprite)(target)).FlipY = action.FlipY;		
		}

	}

}