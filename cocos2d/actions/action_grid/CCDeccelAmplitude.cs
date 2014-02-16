using System;

namespace CocosSharp
{
    public class CCDeccelAmplitude : CCAccelAmplitude
    {
        #region Constructors

        public CCDeccelAmplitude(CCAction pAction, float duration) : base(pAction, duration)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCDeccelAmplitudeState(this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCDeccelAmplitude(OtherAction.Reverse(), Duration);
        }
    }


    #region Action state

    public class CCDeccelAmplitudeState : CCAccelAmplitudeState
    {
        protected CCDeccelAmplitude DeccelAmplitudeAction
        {
            get { return Action as CCDeccelAmplitude; }
        }

        public CCDeccelAmplitudeState(CCDeccelAmplitude action, CCNode target) : base(action, target)
        {
        }

        public override void Update(float time)
        {
            OtherActionState.StateAmplitudeRate = (float)Math.Pow((1 - time), DeccelAmplitudeAction.Rate);
            OtherActionState.Update(time);
        }
    }

    #endregion Action state
}