namespace CocosSharp
{
    public class CCFadeOut : CCActionInterval
    {
        #region Constructors

        public CCFadeOut(float d) : base(d)
        {
        }

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCFadeOutState (this, target);

		}

		// Take me out later - See comments in CCAction
		public override bool HasState 
		{ 
			get { return true; }
		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFadeIn(Duration);
        }
    }

	public class CCFadeOutState : CCActionIntervalState
	{

		public CCFadeOutState (CCFadeOut action, CCNode target)
			: base(action, target)
		{	}

		public override void Update(float time)
		{
			var pRGBAProtocol = Target as ICCColor;
			if (pRGBAProtocol != null)
			{
				pRGBAProtocol.Opacity = (byte) (255 * (1 - time));
			}
		}

	}

}