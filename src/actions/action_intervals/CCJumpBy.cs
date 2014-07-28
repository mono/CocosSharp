namespace CocosSharp
{
	public class CCJumpBy : CCFiniteTimeAction
	{
		public CCPoint Position { get; protected set; }

		public float Height { get; protected set; }

		public uint Jumps { get; protected set; }

		#region Constructors

		public CCJumpBy (float duration, CCPoint position, float height, uint jumps) : base (duration)
		{
			Position = position;
			Height = height;
			Jumps = jumps;
		}

		#endregion Constructors

		protected internal override CCActionState StartAction(CCNode target)
		{
			return new CCJumpByState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCJumpBy (Duration, new CCPoint (-Position.X, -Position.Y), Height, Jumps);
		}
	}

	internal class CCJumpByState : CCFiniteTimeActionState
	{
		protected CCPoint Delta;
		protected float Height;
		protected uint Jumps;
		protected CCPoint StartPosition;
		protected CCPoint P;

		public CCJumpByState (CCJumpBy action, CCNode target)
			: base (action, target)
		{ 
			Delta = action.Position;
			Height = action.Height;
			Jumps = action.Jumps;
			P = StartPosition = target.Position;
		}

		public override void Update (float time)
		{
			if (Target != null)
			{
				// Is % equal to fmodf()???
				float frac = (time * Jumps) % 1f;
				float y = Height * 4f * frac * (1f - frac);
				y += Delta.Y * time;
				float x = Delta.X * time;

				CCPoint currentPos = Target.Position;

				CCPoint diff = currentPos - P;
				StartPosition = diff + StartPosition;

				CCPoint newPos = StartPosition + new CCPoint (x, y);
				Target.Position = newPos;

				P = newPos;
			}
		}
	}

}