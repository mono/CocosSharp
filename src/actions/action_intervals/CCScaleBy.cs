namespace CocosSharp
{
	public class CCScaleBy : CCScaleTo
	{
		#region Constructors


		public CCScaleBy (float duration, float s) : base (duration, s)
		{
		}

		public CCScaleBy (float duration, float sx, float sy) : base (duration, sx, sy)
		{
		}

		#endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
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