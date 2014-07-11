namespace CocosSharp
{
	public class CCSkewTo : CCActionInterval
	{
		protected float EndSkewX;
		protected float EndSkewY;
		protected float SkewX;
		protected float SkewY;

		#region Constructors

		public CCSkewTo (float t, float sx, float sy) : base (t)
		{
			EndSkewX = sx;
			EndSkewY = sy;
		}

		public CCSkewTo (float t, float skewXY) : this (t, skewXY, skewXY)
		{
		}

		#endregion Constructors

		public float SkewToX {
			get { return EndSkewX; }
		}

		public float SkewToY {
			get { return EndSkewY; }
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCSkewToState (this, target);
		}
	}

	public class CCSkewToState : CCActionIntervalState
	{

		protected float DeltaX;
		protected float DeltaY;
		protected float EndSkewX;
		protected float EndSkewY;
		protected float SkewX;
		protected float SkewY;
		protected float StartSkewX;
		protected float StartSkewY;

		public CCSkewToState (CCSkewTo action, CCNode target)
			: base (action, target)
		{ 
			EndSkewX = action.SkewToX;
			EndSkewY = action.SkewToY;

			StartSkewX = target.SkewX;

			if (StartSkewX > 0)
			{
				StartSkewX = StartSkewX % 180f;
			}
			else
			{
				StartSkewX = StartSkewX % -180f;
			}

			DeltaX = EndSkewX - StartSkewX;

			if (DeltaX > 180)
			{
				DeltaX -= 360;
			}
			if (DeltaX < -180)
			{
				DeltaX += 360;
			}

			StartSkewY = target.SkewY;

			if (StartSkewY > 0)
			{
				StartSkewY = StartSkewY % 360f;
			}
			else
			{
				StartSkewY = StartSkewY % -360f;
			}

			DeltaY = EndSkewY - StartSkewY;

			if (DeltaY > 180)
			{
				DeltaY -= 360;
			}
			if (DeltaY < -180)
			{
				DeltaY += 360;
			}
		}

		public override void Update (float time)
		{
			Target.SkewX = StartSkewX + DeltaX * time;
			Target.SkewY = StartSkewY + DeltaY * time;
		}

	}

}