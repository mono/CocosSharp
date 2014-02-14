namespace CocosSharp
{
    public class CCRepeat : CCActionInterval
    {
		public bool ActionInstant { get; private set; }
        protected float m_fNextDt;
		public CCFiniteTimeAction InnerAction { get; private set; }
		public uint Times { get; private set; }
		public uint Total { get; private set; }

        #region Constructors

        public CCRepeat(CCFiniteTimeAction action, uint times) : base(action.Duration * times)
        {
            InitWithAction(action, times);
        }

        // Perform deep copy of CCRepeat
        protected CCRepeat(CCRepeat repeat) : base(repeat)
        {
            InitWithAction(new CCFiniteTimeAction(repeat.InnerAction), repeat.Times);
        }

        private void InitWithAction(CCFiniteTimeAction action, uint times)
        {
            Times = times;
            InnerAction = action;

            ActionInstant = action is CCActionInstant;
            //an instant action needs to be executed one time less in the update method since it uses startWithTarget to execute the action
            if (ActionInstant)
            {
                Times -= 1;
            }
            Total = 0;
        }

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCRepeatState (this, target);

		}

		// Take me out later - See comments in CCAction
		public override bool HasState 
		{ 
			get { return true; }
		}


//        protected internal override void StartWithTarget(CCNode target)
//        {
//            Total = 0;
//            m_fNextDt = InnerAction.Duration / m_fDuration;
//            base.StartWithTarget(target);
//            InnerAction.StartWithTarget(target);
//        }
//
//        public override void Stop()
//        {
//            InnerAction.Stop();
//            base.Stop();
//        }
//
//        // issue #80. Instead of hooking step:, hook update: since it can be called by any 
//        // container action like Repeat, Sequence, AccelDeccel, etc..
//        public override void Update(float dt)
//        {
//            if (dt >= m_fNextDt)
//            {
//                while (dt > m_fNextDt && Total < Times)
//                {
//                    InnerAction.Update(1.0f);
//                    Total++;
//
//                    InnerAction.Stop();
//                    InnerAction.StartWithTarget(m_pTarget);
//                    m_fNextDt += InnerAction.Duration / m_fDuration;
//                }
//
//                // fix for issue #1288, incorrect end value of repeat
//                if (dt >= 1.0f && Total < Times)
//                {
//                    Total++;
//                }
//
//                // don't set an instant action back or update it, it has no use because it has no duration
//                if (!m_bActionInstant)
//                {
//                    if (Total == Times)
//                    {
//                        InnerAction.Update(1f);
//                        InnerAction.Stop();
//                    }
//                    else
//                    {
//                        // issue #390 prevent jerk, use right update
//                        InnerAction.Update(dt - (m_fNextDt - InnerAction.Duration / m_fDuration));
//                    }
//                }
//            }
//            else
//            {
//                InnerAction.Update((dt * Times) % 1.0f);
//            }
//        }
//
//        public override bool IsDone
//        {
//            get { return Total == Times; }
//        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCRepeat(InnerAction.Reverse(), Times);
        }
    }

	public class CCRepeatState : CCActionIntervalState
	{

		protected bool ActionInstant { get; set; }
		protected float NextDt { get; set; }
		protected CCFiniteTimeAction InnerAction { get; set; }
		protected CCFiniteTimeActionState InnerActionState { get; set; }
		protected uint Times { get; set; }
		protected uint Total { get; set; }

		public CCRepeatState (CCRepeat action, CCNode target)
			: base(action, target)
		{ 

			InnerAction = action.InnerAction;
			Times = action.Times;
			Total = action.Total;
			ActionInstant = action.ActionInstant;

			NextDt = InnerAction.Duration / Duration;
			if (!InnerAction.HasState) 
			{
				action.StartWithTarget (target);
				InnerAction.StartWithTarget (target);
			} 
			else 
			{
				InnerActionState = (CCFiniteTimeActionState) InnerAction.StartAction (target);
			}
		}

		public override void Stop()
		{
			InnerAction.Stop();
			base.Stop();
		}

		// issue #80. Instead of hooking step:, hook update: since it can be called by any 
		// container action like Repeat, Sequence, AccelDeccel, etc..
		public override void Update(float dt)
		{
			if (dt >= NextDt)
			{
				if (!InnerAction.HasState)
					while (dt > NextDt && Total < Times)
					{
						InnerAction.Update(1.0f);
						Total++;

						InnerAction.Stop();
						InnerAction.StartWithTarget(Target);
						NextDt += InnerAction.Duration / m_fDuration;
					}
				else
					while (dt > NextDt && Total < Times)
					{
						InnerActionState.Update(1.0f);
						Total++;

						InnerActionState.Stop();
						InnerActionState = (CCFiniteTimeActionState) InnerAction.StartAction(Target);
						NextDt += InnerAction.Duration / m_fDuration;
					}

				// fix for issue #1288, incorrect end value of repeat
				if (dt >= 1.0f && Total < Times)
				{
					Total++;
				}

				// don't set an instant action back or update it, it has no use because it has no duration
				if (!ActionInstant)
				{
					if (!InnerAction.HasState)
						if (Total == Times)
						{
							InnerAction.Update(1f);
							InnerAction.Stop();
						}
						else
						{
							// issue #390 prevent jerk, use right update
							InnerAction.Update(dt - (NextDt - InnerAction.Duration / m_fDuration));
						}
					else
						if (Total == Times)
						{
							InnerActionState.Update(1f);
							InnerActionState.Stop();
						}
						else
						{
							// issue #390 prevent jerk, use right update
							InnerActionState.Update(dt - (NextDt - InnerAction.Duration / m_fDuration));
						}

				}
			}
			else
			{
				if (!InnerAction.HasState)
					InnerAction.Update((dt * Times) % 1.0f);
				else
					InnerActionState.Update((dt * Times) % 1.0f);
			}
		}

		public override bool IsDone
		{
			get { return Total == Times; }
		}

	}

}