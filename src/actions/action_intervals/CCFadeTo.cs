namespace CocosSharp
{
	public class CCFadeTo : CCActionInterval
	{
		public byte ToOpacity { get; private set; }


		#region Constructors

		public CCFadeTo (float duration, byte opacity) : base (duration)
		{
			ToOpacity = opacity;
		}

		#endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCFadeToState (this, target);

		}
	}

	public class CCFadeToState : CCActionIntervalState
	{
		protected byte FromOpacity { get; set; }

		protected byte ToOpacity { get; set; }

		public CCFadeToState (CCFadeTo action, CCNode target)
			: base (action, target)
		{	           
			ToOpacity = action.ToOpacity;

			var pRGBAProtocol = target as ICCColor;
			if (pRGBAProtocol != null) {
				FromOpacity = pRGBAProtocol.Opacity;
			}
		}

		public override void Update (float time)
		{
			var pRGBAProtocol = Target as ICCColor;
			if (pRGBAProtocol != null) {
				pRGBAProtocol.Opacity = (byte)(FromOpacity + (ToOpacity - FromOpacity) * time);
			}
		}
	}


}