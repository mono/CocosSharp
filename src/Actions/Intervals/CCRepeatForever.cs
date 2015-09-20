using System.Diagnostics;

namespace CocosSharp
{
    public class CCRepeatForever : CCFiniteTimeAction
    {
        public CCFiniteTimeAction InnerAction { get; private set; }


        #region Constructors

        public CCRepeatForever (params CCFiniteTimeAction[] actions)
        {
            Debug.Assert (actions != null);
            InnerAction = new CCSequence (actions);

        }

        public CCRepeatForever (CCFiniteTimeAction action)
        {
            Debug.Assert (action != null);
            InnerAction = action;
        }

        #endregion Constructors

        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCRepeatForeverState (this, target);

        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCRepeatForever (InnerAction.Reverse () as CCFiniteTimeAction);
        }
    }

    public class CCRepeatForeverState : CCFiniteTimeActionState
    {

        private CCFiniteTimeAction InnerAction { get; set; }

        private CCFiniteTimeActionState InnerActionState { get; set; }

        public CCRepeatForeverState (CCRepeatForever action, CCNode target)
            : base (action, target)
        { 
            InnerAction = action.InnerAction;
            InnerActionState = (CCFiniteTimeActionState)InnerAction.StartAction (target);
        }

        protected internal override void Step (float dt)
        {
            InnerActionState.Step (dt);

            if (InnerActionState.IsDone)
            {
                float diff = InnerActionState.Elapsed - InnerActionState.Duration;
                InnerActionState = (CCFiniteTimeActionState)InnerAction.StartAction (Target);
                InnerActionState.Step (0f);
                InnerActionState.Step (diff);
            }
        }

        public override bool IsDone {
            get { return false; }
        }

    }
}