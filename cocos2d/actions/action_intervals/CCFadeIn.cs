namespace CocosSharp
{
    public class CCFadeIn : CCActionInterval
    {
        #region Constructors

        public CCFadeIn(float d) : base(d)
        {
        }

        #endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCFadeInState (this, target);

		}

		// Take me out later - See comments in CCAction
		public override bool HasState 
		{ 
			get { return true; }
		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCFadeOut(Duration);
        }
    }

	public class CCFadeInState : CCActionIntervalState
	{

		protected uint Times { get; set; }
		protected bool OriginalState { get; set; }

		public CCFadeInState (CCFadeIn action, CCNode target)
			: base(action, target)
		{	}

		public override void Update(float time)
		{
			var pRGBAProtocol = Target as ICCColor;
			if (pRGBAProtocol != null)
			{
				pRGBAProtocol.Opacity = (byte) (255 * time);
			}
		}
	}

}