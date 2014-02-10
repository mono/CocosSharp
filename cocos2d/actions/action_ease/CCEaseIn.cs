using System;

namespace CocosSharp
{
    public class CCEaseIn : CCEaseRateAction
    {
        #region Constructors

        public CCEaseIn(CCActionInterval pAction, float fRate) : base(pAction, fRate)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseInState(this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseIn((CCActionInterval)InnerAction.Reverse(), 1 / Rate);
        }
    }


    #region Action state

    public class CCEaseInState : CCEaseRateActionState
    {
        public CCEaseInState(CCEaseIn action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            InnerActionState.Update((float)Math.Pow(time, EaseRateAction.Rate));
        }
    }

    #endregion Action state

}