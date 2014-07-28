namespace CocosSharp
{
	public class CCSkewBy : CCSkewTo
	{
		#region Constructors

		public CCSkewBy (float t, float deltaSkewX, float deltaSkewY) : base (t, deltaSkewX, deltaSkewY)
		{
			SkewX = deltaSkewX;
			SkewY = deltaSkewY;
		}

		public CCSkewBy (float t, float deltaSkewXY) : this (t, deltaSkewXY, deltaSkewXY)
		{
		}

		#endregion Constructors

		public float SkewByX {
			get { return SkewX; }
		}

		public float SkewByY {
			get { return SkewY; }
		}

		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCSkewByState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCSkewBy (Duration, -SkewX, -SkewY);
		}
	}

	internal class CCSkewByState : CCSkewToState
	{

		public CCSkewByState (CCSkewBy action, CCNode target)
			: base (action, target)
		{ 

			DeltaX = SkewX = action.SkewByX;
			DeltaY = SkewY = action.SkewByY;
			EndSkewX = StartSkewX + DeltaX;
			EndSkewY = StartSkewY + DeltaY;
		}
	}
}