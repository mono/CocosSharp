namespace CocosSharp
{
	public class CCFadeIn : CCFiniteTimeAction
	{
		#region Constructors

		public CCFadeIn (float d) : base (d)
		{
		}

		#endregion Constructors


		internal override CCActionState StartAction(CCNode target)
		{
			return new CCFadeInState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCFadeOut (Duration);
		}
	}

	internal class CCFadeInState : CCFiniteTimeActionState
	{

		protected uint Times { get; set; }

		protected bool OriginalState { get; set; }

		public CCFadeInState (CCFadeIn action, CCNode target)
			: base (action, target)
		{
		}

		public override void Update (float time)
		{
			var pRGBAProtocol = Target;
			if (pRGBAProtocol != null)
			{
				pRGBAProtocol.Opacity = (byte)(255 * time);
			}
		}
	}

}