namespace CocosSharp
{
	public class CCScaleBy : CCScaleTo
	{
		#region Constructors


        public CCScaleBy (float duration, float scale) : base (duration, scale)
		{
		}

        public CCScaleBy (float duration, float scaleX, float scaleY) : base (duration, scaleX, scaleY)
		{
		}

		#endregion Constructors

		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCScaleByState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCScaleBy (Duration, 1 / EndScaleX, 1 / EndScaleY);
		}

	}

    public class CCScaleByState : CCScaleToState
	{

		public CCScaleByState (CCScaleTo action, CCNode target)
			: base (action, target)
		{ 
			DeltaX = StartScaleX * EndScaleX - StartScaleX;
			DeltaY = StartScaleY * EndScaleY - StartScaleY;
		}

	}

}