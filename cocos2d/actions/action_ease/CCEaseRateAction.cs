namespace CocosSharp
{
    public class CCEaseRateAction : CCActionEase
    {
        public float Rate { get; private set; }


        #region Constructors

        public CCEaseRateAction(CCActionInterval pAction, float fRate) : base(pAction)
        {
            Rate = fRate;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseRateActionState(this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseRateAction((CCActionInterval)InnerAction.Reverse(), 1 / Rate);
        }
    }


    #region Action state

    public class CCEaseRateActionState : CCActionEaseState
    {
        protected CCEaseRateAction EaseRateAction 
        { 
            get { return Action as CCEaseRateAction; } 
        }

        public CCEaseRateActionState(CCEaseRateAction action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            InnerActionState.Update(CCEaseMath.ExponentialOut(time));
        }
    }

    #endregion Action state
}