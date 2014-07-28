using System;

namespace CocosSharp
{
    public abstract class CCFiniteTimeAction : CCAction
    {
        float duration;

        #region Properties

        public virtual float Duration 
        {
            get 
            {
                return duration;
            }
            set 
            {
                float newDuration = value;

                // Prevent division by 0
                if (newDuration == 0)
                {
                    newDuration = float.Epsilon;
                }

                duration = newDuration;
            }
        }

        #endregion Properties


        #region Constructors

        protected CCFiniteTimeAction() 
            : this (0)
        {
        }

        protected CCFiniteTimeAction (float duration)
        {
            Duration = duration;
        }

        #endregion Constructors


        public abstract CCFiniteTimeAction Reverse();

        internal override CCActionState StartAction(CCNode target)
        {
            return new CCFiniteTimeActionState (this, target);
        }
    }

    public class CCFiniteTimeActionState : CCActionState
    {
        bool firstTick;

        #region Properties

        public virtual float Duration { get; set; }
        public float Elapsed { get; private set; }

        public override bool IsDone 
        {
            get { return Elapsed >= Duration; }
        }

        #endregion Properties


        public CCFiniteTimeActionState (CCFiniteTimeAction action, CCNode target)
            : base (action, target)
        { 
            Duration = action.Duration;
            Elapsed = 0.0f;
            firstTick = true;
        }

        protected internal override void Step(float dt)
        {
            if (firstTick)
            {
                firstTick = false;
                Elapsed = 0f;
            }
            else
            {
                Elapsed += dt;
            }

            Update (Math.Max (0f,
                Math.Min (1, Elapsed / Math.Max (Duration, float.Epsilon)
                )
            )
            );
        }

    }
}