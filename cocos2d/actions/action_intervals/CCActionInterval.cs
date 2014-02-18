using System;
using System.Diagnostics;

namespace CocosSharp
{
    // Extra action for making a CCSequence or CCSpawn when only adding one action to it.
    internal class CCExtraAction : CCFiniteTimeAction
    {
        public override CCAction Copy()
        {
            return new CCExtraAction();
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCExtraAction();
        }

        public override void Step(float dt)
        {
        }

        public override void Update(float time)
        {
        }
    }

    public class CCActionInterval : CCFiniteTimeAction
    {
        protected bool m_bFirstTick = true;
        protected float m_elapsed;

        public float Elapsed
        {
            get { return m_elapsed; }
        }

        public override bool IsDone
        {
            get { return m_elapsed >= Duration; }
        }

        // Used by CCSequence and CCParallel
        // In general though, subclasses should aim to call the base constructor, rather than this explicitly
        public override float Duration
        {
            get
            {
                return base.Duration;
            }
            set
            {
                float newDuration = value;
                // prevent division by 0
                // This comparison could be in step:, but it might decrease the performance
                // by 3% in heavy based action games.
                if (newDuration == 0)
                {
                    newDuration = float.Epsilon;
                }

                base.Duration = newDuration;
                m_elapsed = 0;
                m_bFirstTick = true;
            }
        }


        #region Constructors

        protected CCActionInterval()
        {
        }

        public CCActionInterval(float d) : base(d)
        {
            this.Duration = d;
        }

        // Perform a deep copy of CCACtionInterval
        protected internal CCActionInterval(CCActionInterval actionInterval) : base(actionInterval)
        {
            this.Duration = actionInterval.Duration;
        }

        #endregion Constructors


        public override object Copy(ICCCopyable zone)
        {
            return new CCActionInterval(this);
        }

        public override void Step(float dt)
        {
            if (m_bFirstTick)
            {
                m_bFirstTick = false;
                m_elapsed = 0f;
            }
            else
            {
                m_elapsed += dt;
            }

            Update(Math.Max(0f,
                            Math.Min(1, m_elapsed /
                                        Math.Max(m_fDuration, float.Epsilon)
                                )
                       )
                );
        }

        protected internal override void StartWithTarget(CCNode target)
        {
            base.StartWithTarget(target);
            m_elapsed = 0.0f;
            m_bFirstTick = true;
        }

		protected internal override CCActionState StartAction (CCNode target)
		{
			return null; //new CCActionIntervalState (this, target);

		}

        public override CCFiniteTimeAction Reverse()
        {
            throw new NotImplementedException();
        }

        public virtual float AmplitudeRate
        {
            get
            {
				// We need to look at this closer.  Am commenting the Dubug.Assert out for now
				//Debug.Assert(false);
                return 0;
            }
            protected set { Debug.Assert(false); }
        }
    }

	public class CCActionIntervalState : CCFiniteTimeActionState
	{

		protected bool m_bFirstTick = true;
		protected float m_elapsed;

		public float Elapsed
		{
			get { return m_elapsed; }
		}

		public override bool IsDone
		{
			get { return m_elapsed >= Duration; }
		}

        protected CCActionInterval IntervalAction
        {
            get { return Action as CCActionInterval; }
        }

        // Amplitude rate can dynamically change as action is run (e.g. CCAccelAmplitude)
        // Therefore, we need to store the rate as a state variable
        protected internal float StateAmplitudeRate { get; set; }

		public CCActionIntervalState (CCActionInterval action, CCNode target)
			: base(action, target)
		{ 
			m_elapsed = 0.0f;
			m_bFirstTick = true;
            StateAmplitudeRate = 1.0f;
		}

		public override void Step(float dt)
		{
			if (m_bFirstTick)
			{
				m_bFirstTick = false;
				m_elapsed = 0f;
			}
			else
			{
				m_elapsed += dt;
			}

			Update(Math.Max(0f,
				Math.Min(1, m_elapsed /
					Math.Max(m_fDuration, float.Epsilon)
				)
			)
			);
		}
	}

}