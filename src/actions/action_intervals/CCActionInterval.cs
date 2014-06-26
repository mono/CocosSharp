using System;
using System.Diagnostics;

namespace CocosSharp
{
	// Extra action for making a CCSequence or CCSpawn when only adding one action to it.
	internal class CCExtraAction : CCFiniteTimeAction
	{
		public override CCFiniteTimeAction Reverse ()
		{
			return new CCExtraAction ();
		}

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCExtraActionState (this, target);

		}

		#region Internal State

		public class CCExtraActionState : CCFiniteTimeActionState
		{

			public CCExtraActionState (CCExtraAction action, CCNode target)
				: base (action, target)
			{
			}

			protected internal override void Step (float dt)
			{
			}

			public override void Update (float time)
			{
			}
		}

		#endregion

	}

	public class CCActionInterval : CCFiniteTimeAction
	{
		protected bool FirstTick = true;

		public float Elapsed { get; protected set; }

		// Used by CCSequence and CCParallel
		// In general though, subclasses should aim to call the base constructor, rather than this explicitly
		public override float Duration {
			get {
				return base.Duration;
			}
			set {
				float newDuration = value;
				// prevent division by 0
				// This comparison could be in step:, but it might decrease the performance
				// by 3% in heavy based action games.
				if (newDuration == 0)
				{
					newDuration = float.Epsilon;
				}

				base.Duration = newDuration;
				Elapsed = 0;
				FirstTick = true;
			}
		}


		#region Constructors

		protected CCActionInterval ()
		{
		}

		public CCActionInterval (float d) : base (d)
		{
			this.Duration = d;
		}

		#endregion Constructors

		protected internal override CCActionState StartAction (CCNode target)
		{
			return new CCActionIntervalState (this, target);

		}

		public override CCFiniteTimeAction Reverse ()
		{
			throw new NotImplementedException ();
		}
	}

	public class CCActionIntervalState : CCFiniteTimeActionState
	{
		protected bool FirstTick { get; private set; }

		public float Elapsed { get; private set; }


		public override bool IsDone {
			get { return Elapsed >= Duration; }
		}

		public CCActionIntervalState (CCActionInterval action, CCNode target)
			: base (action, target)
		{ 
			Elapsed = 0.0f;
			FirstTick = true;
		}

		protected internal override void Step (float dt)
		{
			if (FirstTick)
			{
				FirstTick = false;
				Elapsed = 0f;
			}
			else
			{
				Elapsed += dt;
			}

			Update (Math.Max (0f,
				                 Math.Min (1, Elapsed /
					                 Math.Max (Duration, float.Epsilon)
				                 )
				)
			);
		}
	}

}