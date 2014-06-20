namespace CocosSharp
{

	public class CCTintBy : CCActionInterval
	{
		public short DeltaB { get; private set; }

		public short DeltaG { get; private set; }

		public short DeltaR { get; private set; }


		#region Constructors

		public CCTintBy (float duration, short deltaRed, short deltaGreen, short deltaBlue) : base (duration)
		{
			DeltaR = deltaRed;
			DeltaG = deltaGreen;
			DeltaB = deltaBlue;
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCTintByState (this, target);
		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCTintBy (Duration, (short)-DeltaR, (short)-DeltaG, (short)-DeltaB);
		}
	}


	public class CCTintByState : CCActionIntervalState
	{
		protected short DeltaB { get; set; }

		protected short DeltaG { get; set; }

		protected short DeltaR { get; set; }

		protected short FromB { get; set; }

		protected short FromG { get; set; }

		protected short FromR { get; set; }

		public CCTintByState (CCTintBy action, CCNode target)
			: base (action, target)
		{	
			DeltaB = action.DeltaB;
			DeltaG = action.DeltaG;
			DeltaR = action.DeltaR;

			var protocol = target as ICCColorable;
			if (protocol != null)
			{
				var color = protocol.Color;
				FromR = color.R;
				FromG = color.G;
				FromB = color.B;
			}
		}

		protected internal override void Update (float time)
		{
			var protocol = Target as ICCColorable;
			if (protocol != null)
			{
				protocol.Color = new CCColor3B ((byte)(FromR + DeltaR * time),
				                                (byte)(FromG + DeltaG * time),
				                                (byte)(FromB + DeltaB * time));
			}
		}

	}

}