using System;

namespace CocosSharp
{
    public class CCEaseIn : CCEaseRateAction
    {
        #region Constructors

        public CCEaseIn (CCFiniteTimeAction action, float rate) : base (action, rate)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseInState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCEaseIn ((CCFiniteTimeAction)InnerAction.Reverse (), 1 / Rate);
        }
    }


    #region Action state

    internal class CCEaseInState : CCEaseRateActionState
    {
        public CCEaseInState (CCEaseIn action, CCNode target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update ((float)Math.Pow (time, Rate));
        }
    }

    #endregion Action state

}