using System.Diagnostics;

namespace CocosSharp
{
    public class CCSpeed : CCAction
    {
        public float Speed { get; private set; }
        protected internal CCActionInterval InnerAction { get; private set; }


        #region Constructors

        public CCSpeed(CCActionInterval action, float fRate)
        {
            InnerAction = action;
            Speed = fRate;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCSpeedState(this, target);
        }

        public virtual CCActionInterval Reverse()
        {
            return (CCActionInterval) (CCAction) new CCSpeed((CCActionInterval)InnerAction.Reverse(), Speed);
        }
    }


    #region Action state

    public class CCSpeedState : CCActionState
    {
        protected CCActionIntervalState InnerActionState { get; private set; }

        protected CCSpeed SpeedAction
        {
            get { return Action as CCSpeed; }
        }

        public override bool IsDone
        {
            get { return InnerActionState.IsDone; }
        }

        public CCSpeedState(CCSpeed action, CCNode target) : base(action, target)
        {
            InnerActionState = (CCActionIntervalState)action.InnerAction.StartAction(target);
        }

        public override void Stop()
        {
            InnerActionState.Stop();
            base.Stop();
        }

        public override void Step(float dt)
        {
            InnerActionState.Step(dt * SpeedAction.Speed);
        }
    }

    #endregion Action state
}