using System;
using Microsoft.Xna.Framework;

namespace CocosSharp
{
    public class CCEaseElasticInOut : CCEaseElastic
    {
        #region Constructors

        public CCEaseElasticInOut(CCActionInterval pAction) : this(pAction, 0.3f)
        {
        }

        public CCEaseElasticInOut(CCActionInterval pAction, float fPeriod) : base(pAction, fPeriod)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCEaseElasticInOutState(this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCEaseElasticInOut((CCActionInterval)InnerAction.Reverse(), Period);
        }
    }


    #region Action state

    public class CCEaseElasticInOutState : CCEaseElasticState
    {
        public CCEaseElasticInOutState(CCEaseElasticInOut action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            InnerActionState.Update(CCEaseMath.ElasticInOut(time, Period));
        }
    }

    #endregion Action state
}