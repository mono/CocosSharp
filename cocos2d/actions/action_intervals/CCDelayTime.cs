namespace CocosSharp
{
    public class CCDelayTime : CCActionInterval
    {
        #region Constructors

        public CCDelayTime(float d) : base(d)
        {
        }

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCDelayTimeState (this, target);

		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCDelayTime(m_fDuration);
        }
    }

	public class CCDelayTimeState : CCActionIntervalState
	{

		public CCDelayTimeState (CCDelayTime action, CCNode target)
			: base(action, target)
		{ }

		public override void Update(float time)
		{
		}

	}
}