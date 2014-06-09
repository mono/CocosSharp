using System.Diagnostics;

namespace CocosSharp
{
	public class CCSequence : CCActionInterval
	{
		public CCFiniteTimeAction[] Actions { get; private set; }

		#region Constructors

		public CCSequence (CCFiniteTimeAction action1, CCFiniteTimeAction action2) : base (action1.Duration + action2.Duration)
		{
			Actions = new CCFiniteTimeAction[2];
			InitCCSequence (action1, action2);
		}

		public CCSequence (params CCFiniteTimeAction[] actions) : base ()
		{

			Actions = new CCFiniteTimeAction[2];

			var prev = actions [0];

			// Can't call base(duration) because we need to calculate duration here
			float combinedDuration = 0.0f;
			foreach (CCFiniteTimeAction action in actions)
			{
				combinedDuration += action.Duration;
			}
			Duration = combinedDuration;

			if (actions.Length == 1)
			{
				InitCCSequence (prev, new CCExtraAction ());
			}
			else
			{
				// Basically what we are doing here is creating a whole bunch of 
				// nested CCSequences from the actions.
				for (int i = 1; i < actions.Length - 1; i++)
				{
					prev = new CCSequence (prev, actions [i]);
				}

				InitCCSequence (prev, actions [actions.Length - 1]);
			}

		}

		private void InitCCSequence (CCFiniteTimeAction actionOne, CCFiniteTimeAction actionTwo)
		{
			Debug.Assert (actionOne != null);
			Debug.Assert (actionTwo != null);

			Actions [0] = actionOne;
			Actions [1] = actionTwo;

		}

		#endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCSequenceState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			return new CCSequence (Actions [1].Reverse (), Actions [0].Reverse ());
		}
	}

	public class CCSequenceState : CCActionIntervalState
	{
		protected int last;
		protected CCFiniteTimeAction[] actionSequences = new CCFiniteTimeAction[2];
		protected CCFiniteTimeActionState[] actionStates = new CCFiniteTimeActionState[2];
		protected float split;
		private bool hasInfiniteAction = false;

		public CCSequenceState (CCSequence action, CCNode target)
			: base (action, target)
		{ 
			actionSequences = action.Actions;
			hasInfiniteAction = (actionSequences [0] is CCRepeatForever) || (actionSequences [1] is CCRepeatForever);
			split = actionSequences [0].Duration / Duration;
			last = -1;

		}

		protected internal override bool IsDone {
			get {
				if (hasInfiniteAction && actionSequences [last] is CCRepeatForever)
				{
					return (false);
				}
				return base.IsDone;
			}
		}


		protected internal override void Stop ()
		{
			// Issue #1305
			if (last != -1)
			{
				actionStates [last].Stop ();
			}
		}

		protected internal override void Step (float dt)
		{
			if (last > -1 && (actionSequences [last] is CCRepeat || actionSequences [last] is CCRepeatForever))
			{
				actionStates [last].Step (dt);
			}
			else
			{
				base.Step (dt);
			}
		}

		public override void Update (float time)
		{
			bool bRestart = false;
			int found;
			float new_t;

			if (time < split)
			{
				// action[0]
				found = 0;
				if (split != 0)
					new_t = time / split;
				else
					new_t = 1;
			}
			else
			{
				// action[1]
				found = 1;
				if (split == 1)
					new_t = 1;
				else
					new_t = (time - split) / (1 - split);
			}

			if (found == 1)
			{
				if (last == -1)
				{
					// action[0] was skipped, execute it.
					actionStates [0] = (CCFiniteTimeActionState)actionSequences [0].StartAction (Target);
					actionStates [0].Update (1.0f);
					actionStates [0].Stop ();
				}
				else if (last == 0)
					{
						actionStates [0].Update (1.0f);
						actionStates [0].Stop ();
					}
			}
			else if (found == 0 && last == 1)
				{
					// Reverse mode ?
					// XXX: Bug. this case doesn't contemplate when _last==-1, found=0 and in "reverse mode"
					// since it will require a hack to know if an action is on reverse mode or not.
					// "step" should be overriden, and the "reverseMode" value propagated to inner Sequences.
					actionStates [1].Update (0);
					actionStates [1].Stop ();

				}

			// Last action found and it is done.
			if (found == last && actionStates [found].IsDone)
			{
				return;
			}


			// Last action found and it is done
			if (found != last || bRestart)
			{
				actionStates [found] = (CCFiniteTimeActionState)actionSequences [found].StartAction (Target);
			}

			actionStates [found].Update (new_t);
			last = found;

		}


	}

}