namespace CocosSharp
{
    public class CCEaseRateAction : CCActionEase
    {
        public float Rate { get; private set; }


        #region Constructors

        public CCEaseRateAction (CCFiniteTimeAction action, float rate) : base (action)
        {
            Rate = rate;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseRateActionState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCEaseRateAction ((CCFiniteTimeAction)InnerAction.Reverse (), 1 / Rate);
        }
    }


    #region Action state

    public class CCEaseRateActionState : CCActionEaseState
    {
        protected float Rate { get; private set; }

        public CCEaseRateActionState (CCEaseRateAction action, CCNode target) : base (action, target)
        {
            Rate = action.Rate;
        }

        public override void Update (float time)
        {
            InnerActionState.Update (CCEaseMath.ExponentialOut (time));
        }
    }

    #endregion Action state
}