using System;

namespace CocosSharp
{
    public class CCDeccelAmplitude : CCAccelAmplitude
    {
        #region Constructors

        public CCDeccelAmplitude (CCAmplitudeAction action, float duration, float deccRate = 1.0f)
            : base (action, duration, deccRate)
        {
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCDeccelAmplitudeState (this, target);
        }

        public override CCFiniteTimeAction Reverse ()
        {
            return new CCDeccelAmplitude ((CCAmplitudeAction)OtherAction.Reverse (), Duration, Rate);
        }
    }


    #region Action state

    internal class CCDeccelAmplitudeState : CCAccelAmplitudeState
    {
        public CCDeccelAmplitudeState (CCDeccelAmplitude action, CCNode target) : base (action, target)
        {
        }

        public override void Update (float time)
        {
            OtherActionState.AmplitudeRate = (float)Math.Pow ((1 - time), Rate);
            OtherActionState.Update (time);
        }
    }

    #endregion Action state
}