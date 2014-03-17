using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CocosSharp
{
    public class CCParallel : CCActionInterval
    {
		public CCFiniteTimeAction[] Actions { get; private set; }

        #region Constructors

        public CCParallel(params CCFiniteTimeAction[] actions) : base()
        {
            // Can't call base(duration) because max action duration needs to be determined here
            float maxDuration = actions.OrderByDescending (action => action.Duration).First().Duration;
            Duration = maxDuration;

			Actions = actions;

			for (int i = 0; i < Actions.Length; i++)
			{
				var actionDuration = Actions[i].Duration;
				if (actionDuration < Duration)
				{
					Actions[i] = new CCSequence(Actions[i], new CCDelayTime(Duration - actionDuration));
				}
			}
        }

        #endregion Constructors


		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCParallelState (this, target);

		}

        /// <summary>
        /// Reverses the current parallel sequence.
        /// </summary>
        /// <returns></returns>
        public override CCFiniteTimeAction Reverse()
        {
            CCFiniteTimeAction[] rev = new CCFiniteTimeAction[Actions.Length];
            for (int i = 0; i < Actions.Length; i++)
            {
                rev[i] = Actions[i].Reverse();
            }

            return new CCParallel(rev);
        }

    }

	public class CCParallelState : CCActionIntervalState
	{

		protected CCFiniteTimeAction[] Actions { get; set; }
		protected CCFiniteTimeActionState[] ActionStates { get; set; }

		public CCParallelState (CCParallel action, CCNode target)
			: base(action, target)
		{	
			Actions = action.Actions;
			ActionStates = new CCFiniteTimeActionState[Actions.Length];

			for (int i = 0; i < Actions.Length; i++)
			{
				ActionStates [i] = (CCFiniteTimeActionState) Actions [i].StartAction (target);
			}
		}

		public override void Stop()
		{
			for (int i = 0; i < Actions.Length; i++)
			{
				ActionStates [i].Stop ();
			}
			base.Stop();
		}

		public override void Update(float time)
		{
			for (int i = 0; i < Actions.Length; i++)
			{
				ActionStates [i].Update (time);
			}
		}
	}
}