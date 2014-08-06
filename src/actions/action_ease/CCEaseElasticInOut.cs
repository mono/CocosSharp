using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCEaseElasticInOut : CCEaseElastic
    {
        #region Constructors

        public CCEaseElasticInOut (CCFiniteTimeAction action) : this (action, 0.3f)
        {
        }

        public CCEaseElasticInOut (CCFiniteTimeAction action, float period) : base (action, period)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseElasticInOutState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCEaseElasticInOut ((CCFiniteTimeAction)InnerAction.Reverse (), Period);
        }
    }


    #region Action state

    public class CCEaseElasticInOutState : CCEaseElasticState
    {
        public CCEaseElasticInOutState (CCEaseElasticInOut action, CCNode target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            InnerActionState.Update (CCEaseMath.ElasticInOut (time, Period));
        }
    }

    #endregion Action state
}