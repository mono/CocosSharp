using System.Diagnostics;

namespace CocosSharp
{
    public class CCSpeed : CCAction
    {
        public float Speed { get; private set; }

        protected internal CCFiniteTimeAction InnerAction { get; private set; }


        #region Constructors

        public CCSpeed (CCFiniteTimeAction action, float fRate)
        {
            InnerAction = action;
            Speed = fRate;
        }

        #endregion Constructors


        internal override CCActionState StartAction(CCNode target)
        {
            return new CCSpeedState (this, target);
        }

        public virtual CCFiniteTimeAction Reverse ()
        {
            return (CCFiniteTimeAction)(CCAction)new CCSpeed ((CCFiniteTimeAction)InnerAction.Reverse(), Speed);
        }
    }


    #region Action state

    internal class CCSpeedState : CCActionState
    {
        #region Properties

        public float Speed { get; private set; }

        protected CCFiniteTimeActionState InnerActionState { get; private set; }

        public override bool IsDone 
        {
            get { return InnerActionState.IsDone; }
        }

        #endregion Properties


        public CCSpeedState (CCSpeed action, CCNode target) : base (action, target)
        {
            InnerActionState = (CCFiniteTimeActionState)action.InnerAction.StartAction (target);
            Speed = action.Speed;
        }

        protected internal override void Stop ()
        {
            InnerActionState.Stop ();
            base.Stop ();
        }

        protected internal override void Step (float dt)
        {
            InnerActionState.Step (dt * Speed);
        }
    }

    #endregion Action state
}