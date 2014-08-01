using System;

namespace CocosSharp
{
    public class CCEaseInOut : CCEaseRateAction
    {
        #region Constructors

        public CCEaseInOut (CCFiniteTimeAction action, float rate) : base (action, rate)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseInOutState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCEaseInOut ((CCFiniteTimeAction)InnerAction.Reverse (), Rate);
        }
    }


    #region Action state

    internal class CCEaseInOutState : CCEaseRateActionState
    {
        public CCEaseInOutState (CCEaseInOut action, CCNode target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            float actionRate = Rate;
            time *= 2;

            if (time < 1)
            {
                InnerActionState.Update (0.5f * (float)Math.Pow (time, actionRate));
            }
            else
            {
                InnerActionState.Update (1.0f - 0.5f * (float)Math.Pow (2 - time, actionRate));
            }        
        }
    }

    #endregion Action state
}