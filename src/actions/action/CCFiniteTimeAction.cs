namespace CocosSharp
{
    public class CCFiniteTimeAction : CCAction
    {
        public virtual float Duration { get; set; }


        #region Constructors

        protected CCFiniteTimeAction()
        {
        }

        protected CCFiniteTimeAction (float d)
        {
            Duration = d;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction (CCNode target)
        {
            return new CCFiniteTimeActionState (this, target);

        }

        public virtual CCFiniteTimeAction Reverse ()
        {
            CCLog.Log ("CocosSharp: FiniteTimeAction#reverse: Implement me");
            return null;
        }
    }

    public class CCFiniteTimeActionState : CCActionState
    {
        public CCFiniteTimeActionState (CCFiniteTimeAction action, CCNode target)
            : base (action, target)
        { 
            Duration = action.Duration;
        }

        public virtual float Duration { get; set; }
    }
}