using System;

namespace CocosSharp
{
    public class CCAccelAmplitude : CCActionInterval
    {
        public float Rate { get; private set; }
        protected internal CCActionInterval OtherAction { get; private set; }

        // Take me out later - See comments in CCAction
        public override bool HasState 
        { 
            get { return true; }
        }
            
        #region Constructors

        public CCAccelAmplitude(CCAction pAction, float duration) : base(duration)
        {
            Rate = 1.0f;
            OtherAction = pAction as CCActionInterval;
        }

        #endregion Constructors


        protected internal override CCActionState StartAction(CCNode target)
        {
            return new CCAccelAmplitudeState(this, target);
        }

        public override CCFiniteTimeAction Reverse()
        {
            return new CCAccelAmplitude(OtherAction.Reverse(), Duration);
        }

    }


    #region Action state

    public class CCAccelAmplitudeState : CCActionIntervalState
    {
        public float Rate { get; set; }
        protected CCActionIntervalState OtherActionState { get; private set; }
            
        public CCAccelAmplitudeState(CCAccelAmplitude action, CCNode target) : base(action, target)
        {
            Rate = action.Rate;
            OtherActionState = (CCActionIntervalState)action.OtherAction.StartAction(target);
        }

        public override void Update(float time)
        {
            OtherActionState.StateAmplitudeRate = (float)Math.Pow(time, Rate);
            OtherActionState.Update(time);
        }
    }

    #endregion Action state
}