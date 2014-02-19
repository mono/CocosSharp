using System;
using System.Diagnostics;

namespace CocosSharp
{
    public class CCSpawn : CCActionInterval
    {
		public CCFiniteTimeAction ActionOne { get; protected set; }
		public CCFiniteTimeAction ActionTwo { get; protected set; }


        #region Constructors

        protected CCSpawn(CCFiniteTimeAction action1, CCFiniteTimeAction action2) : base(Math.Max(action1.Duration, action2.Duration))
        {
            InitCCSpawn(action1, action2);
        }

        public CCSpawn(params CCFiniteTimeAction[] actions)
        {
            CCFiniteTimeAction prev = actions[0];
            CCFiniteTimeAction next = null;

            if (actions.Length == 1)
            {
                next = new CCExtraAction();
            }
            else
            {
				// We create a nested set of CCSpawnActions out of all of the actions
                for (int i = 1; i < actions.Length - 1; i++)
                {
                    prev = new CCSpawn(prev, actions[i]);
                }

                next = actions[actions.Length - 1];
            }

            // Can't call base(duration) because we need to determine max duration
            // Instead call base's init method here
            if(prev != null && next != null)
            {
                Duration = Math.Max(prev.Duration, next.Duration);
                InitCCSpawn(prev, next);
            }
        }

        private void InitCCSpawn(CCFiniteTimeAction action1, CCFiniteTimeAction action2)
        {
            Debug.Assert(action1 != null);
            Debug.Assert(action2 != null);

            float d1 = action1.Duration;
            float d2 = action2.Duration;

            ActionOne = action1;
            ActionTwo = action2;

            if (d1 > d2)
            {
                ActionTwo = new CCSequence(action2, new CCDelayTime(d1 - d2));
            }
            else if (d1 < d2)
            {
                ActionOne = new CCSequence(action1, new CCDelayTime(d2 - d1));
            }
        }

        #endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCSpawnState (this, target);

		}

        public override CCFiniteTimeAction Reverse()
        {
            return new CCSpawn(ActionOne.Reverse(), ActionTwo.Reverse());
        }
    }

	public class CCSpawnState : CCActionIntervalState
	{

		protected CCFiniteTimeAction ActionOne { get; set; }
		private CCFiniteTimeActionState ActionStateOne { get; set; }
		protected CCFiniteTimeAction ActionTwo { get; set; }
		private CCFiniteTimeActionState ActionStateTwo { get; set; }

		public CCSpawnState (CCSpawn action, CCNode target)
			: base(action, target)
		{ 
			ActionOne = action.ActionOne;
			ActionTwo = action.ActionTwo;

			ActionStateOne = (CCFiniteTimeActionState)ActionOne.StartAction (target);
			ActionStateTwo = (CCFiniteTimeActionState)ActionTwo.StartAction (target);
		}

		public override void Stop()
		{
			ActionStateOne.Stop();
			ActionStateTwo.Stop ();

			base.Stop();
		}

		public override void Update(float time)
		{
			if (ActionOne != null)
			{
				ActionStateOne.Update (time);
			}

			if (ActionTwo != null)
			{
				ActionStateTwo.Update (time);
			}
		}

	}

}
