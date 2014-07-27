namespace CocosSharp
{

	public class CCTintTo : CCActionInterval
	{
		public CCColor3B ColorTo { get; private set; }


		#region Constructors

		public CCTintTo (float duration, byte red, byte green, byte blue) : base (duration)
		{
			ColorTo = new CCColor3B (red, green, blue);
		}

		#endregion Constructors

		internal override CCActionState StartAction(CCNode target)
		{
			return new CCTintToState (this, target);

		}
	}

	internal class CCTintToState : CCActionIntervalState
	{
		protected CCColor3B ColorFrom { get; set; }

		protected CCColor3B ColorTo { get; set; }

		public CCTintToState (CCTintTo action, CCNode target)
			: base (action, target)
		{	
			ColorTo = action.ColorTo;
			var protocol = Target;
			if (protocol != null)
			{
				ColorFrom = protocol.Color;
			}
		}

		public override void Update (float time)
		{
			var protocol = Target;
			if (protocol != null)
			{
				protocol.Color = new CCColor3B ((byte)(ColorFrom.R + (ColorTo.R - ColorFrom.R) * time),
				                                (byte)(ColorFrom.G + (ColorTo.G - ColorFrom.G) * time),
				                                (byte)(ColorFrom.B + (ColorTo.B - ColorFrom.B) * time));
			}
		}

	}

}