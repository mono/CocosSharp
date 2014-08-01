using System;

namespace CocosSharp
{
    public class CCEaseExponentialIn : CCActionEase
    {
        #region Constructors

        public CCEaseExponentialIn (CCFiniteTimeAction action) : base (action)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseExponentialInState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCEaseExponentialOut ((CCFiniteTimeAction)InnerAction.Reverse ());
        }
    }


    #region Action state

    internal class CCEaseExponentialInState : CCActionEaseState
    {
        public CCEaseExponentialInState (CCEaseExponentialIn action, CCNode target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (CCEaseMath.ExponentialIn (time));
        }
    }

    #endregion Action state
}