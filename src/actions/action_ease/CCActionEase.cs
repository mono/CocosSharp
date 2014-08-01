namespace CocosSharp
{
    public class CCActionEase : CCFiniteTimeAction
    {
        protected internal CCFiniteTimeAction InnerAction { get; private set; }


        #region Constructors

        public CCActionEase(CCFiniteTimeAction action) : base (action.Duration)
        {
            InnerAction = action;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCActionEaseState (this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCActionEase ((CCFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    internal class CCActionEaseState : CCFiniteTimeActionState
    {
        protected CCFiniteTimeActionState InnerActionState { get; private set; }

        public CCActionEaseState (CCActionEase action, CCNode target) : base (action, target)
        {
            InnerActionState = (CCFiniteTimeActionState)action.InnerAction.StartAction (target);
        }

        protected internal override void Stop ()
        {
            InnerActionState.Stop ();
            base.Stop ();
        }

        public override void Update (float time)
        {
            InnerActionState.Update (time);
        }
    }

    #endregion Action state
}